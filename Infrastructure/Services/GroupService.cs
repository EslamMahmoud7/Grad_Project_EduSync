using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGenericRepository<Group> _groupRepo;
        private readonly MainDbContext _db;
        private readonly UserManager<User> _userManager;

        public GroupService(
            IGenericRepository<Group> groupRepo,
            MainDbContext db,
            UserManager<User> userManager)
        {
            _groupRepo = groupRepo;
            _db = db;
            _userManager = userManager;
        }

        public async Task<IReadOnlyList<StudentInGroupDTO>> GetStudentsByGroupIdAsync(string groupId)
        {
            var groupExists = await _db.Groups.AnyAsync(g => g.Id == groupId);
            if (!groupExists)
            {
                throw new ArgumentException($"Group with ID '{groupId}' not found.");
            }

            var students = await _db.GroupStudents
                .Where(gs => gs.GroupId == groupId)
                .Include(gs => gs.Student)
                .Select(gs => new StudentInGroupDTO
                {
                    StudentId = gs.Student.Id,
                    FirstName = gs.Student.FirstName,
                    LastName = gs.Student.LastName,
                    Email = gs.Student.Email ?? "N/A"
                })
                .AsNoTracking()
                .ToListAsync();

            return students;
        }

        public async Task<GroupDTO> AddGroupAsync(CreateGroupDTO dto)
        {
            var course = await _db.Courses.FindAsync(dto.CourseId);
            if (course == null)
                throw new ArgumentException($"Course with ID '{dto.CourseId}' not found.");

            if (!string.IsNullOrWhiteSpace(dto.InstructorId))
            {
                var instructorUser = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Id == dto.InstructorId && u.Role == UserRole.Instructor);
                if (instructorUser == null)
                    throw new ArgumentException($"Instructor (User) with ID '{dto.InstructorId}' not found or is not an Instructor.");
            }

            var group = new Group
            {
                Id = Guid.NewGuid().ToString(),
                CourseId = dto.CourseId,
                Label = dto.Label,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Location = dto.Location,
                InstructorId = dto.InstructorId
            };

            await _groupRepo.Add(group);
            return await GetGroupByIdAsync(group.Id);
        }

        public async Task<GroupDTO> UpdateGroupAsync(string id, UpdateGroupDTO dto)
        {
            var group = await _db.Groups.FindAsync(id);
            if (group == null)
                throw new ArgumentException($"Group with ID '{id}' not found.");

            if (!string.IsNullOrWhiteSpace(dto.CourseId) && group.CourseId != dto.CourseId)
            {
                var course = await _db.Courses.FindAsync(dto.CourseId);
                if (course == null)
                    throw new ArgumentException($"Course with ID '{dto.CourseId}' not found.");
                group.CourseId = dto.CourseId;
            }

            if (dto.InstructorId != group.InstructorId)
            {
                if (!string.IsNullOrWhiteSpace(dto.InstructorId))
                {
                    var instructorUser = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.Id == dto.InstructorId && u.Role == UserRole.Instructor);
                    if (instructorUser == null)
                        throw new ArgumentException($"Instructor (User) with ID '{dto.InstructorId}' not found or is not an Instructor.");
                    group.InstructorId = dto.InstructorId;
                }
                else 
                {
                    group.InstructorId = null;
                }
            }

            if (dto.Label != null)
                group.Label = dto.Label;

            if (dto.Location != null)
                group.Location = dto.Location;

            await _groupRepo.Update(group);
            return await GetGroupByIdAsync(id);
        }

        private GroupDTO MapToDto(Group group)
        {
            return new GroupDTO
            {
                Id = group.Id,
                CourseId = group.CourseId,
                CourseTitle = group.Course?.Title ?? "N/A",
                CourseDescription = group.Course?.Description,
                CourseCredits = group.Course?.Credits ?? 0,
                CourseLevel = group.Course?.Level ?? 0, 
                Label = group.Label,
                StartTime = group.StartTime,
                EndTime = group.EndTime,
                Location = group.Location,
                Instructor = group.Instructor != null ? new InstructorDTO 
                {
                    Id = group.Instructor.Id,
                    FirstName = group.Instructor.FirstName,
                    LastName = group.Instructor.LastName,
                    Email = group.Instructor.Email 
                } : null,
                NumberOfStudents = group.GroupStudents?.Count ?? 0
            };
        }

        public async Task<IReadOnlyList<GroupDTO>> GetGroupsByStudentIdAsync(string studentId)
        {
            var groups = await _db.GroupStudents
               .Where(gs => gs.StudentId == studentId)
               .Include(gs => gs.Group).ThenInclude(g => g.Course)
               .Include(gs => gs.Group).ThenInclude(g => g.Instructor)
               .Include(gs => gs.Group).ThenInclude(g => g.GroupStudents)
               .Select(gs => gs.Group)
               .Distinct()
               .AsNoTracking().ToListAsync();
            return groups.Select(g => MapToDto(g!)).ToList();
        }

        public async Task DeleteGroupAsync(string id)
        {
            var group = await _groupRepo.GetById(id); 
            if (group == null) throw new ArgumentException($"Group with ID '{id}' not found.");

            var groupStudents = _db.GroupStudents.Where(gs => gs.GroupId == id);
            if (await groupStudents.AnyAsync())
            {
                _db.GroupStudents.RemoveRange(groupStudents);
            }
            await _groupRepo.Delete(group);
        }

        public async Task<IReadOnlyList<GroupDTO>> GetAllGroupsAsync()
        {
            var groups = await _db.Groups
                .Include(g => g.Course)
                .Include(g => g.Instructor)
                .Include(g => g.GroupStudents)
                .AsNoTracking().ToListAsync();
            return groups.Select(MapToDto).ToList();
        }

        public async Task<GroupDTO> GetGroupByIdAsync(string id)
        {
            var group = await _db.Groups.Where(g => g.Id == id)
                .Include(g => g.Course)
                .Include(g => g.Instructor) 
                .Include(g => g.GroupStudents)
                .AsNoTracking().FirstOrDefaultAsync();
            if (group == null) throw new ArgumentException($"Group with ID '{id}' not found.");
            return MapToDto(group);
        }
        public async Task RemoveStudentsFromGroupAsync(string groupId, RemoveStudentsFromGroupDTO dto)
        {
            var groupExists = await _db.Groups.AnyAsync(g => g.Id == groupId);
            if (!groupExists)
            {
                throw new ArgumentException($"Group with ID '{groupId}' not found.");
            }

            var studentsToRemove = await _db.GroupStudents
                .Where(gs => gs.GroupId == groupId && dto.StudentIds.Contains(gs.StudentId))
                .ToListAsync();

            if (studentsToRemove.Any())
            {
                _db.GroupStudents.RemoveRange(studentsToRemove);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyList<GroupDTO>> GetGroupsByCourseIdAsync(string courseId)
        {
            var groups = await _db.Groups.Where(g => g.CourseId == courseId)
                .Include(g => g.Course)
                .Include(g => g.Instructor) 
                .Include(g => g.GroupStudents)
                .AsNoTracking().ToListAsync();
            return groups.Select(MapToDto).ToList();
        }

        public async Task<IReadOnlyList<GroupDTO>> GetGroupsByInstructorIdAsync(string instructorId)
        {
            var groups = await _db.Groups.Where(g => g.InstructorId == instructorId)
                .Include(g => g.Course)
                .Include(g => g.Instructor)
                .Include(g => g.GroupStudents)
                .AsNoTracking().ToListAsync();
            return groups.Select(MapToDto).ToList();
        }
    }
}