using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
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
        private readonly IGenericRepository<Instructor> _instructorRepo;
        private readonly MainDbContext _db;

        public GroupService(
            IGenericRepository<Group> groupRepo,
            IGenericRepository<Instructor> instructorRepo,
            MainDbContext db)
        {
            _groupRepo = groupRepo;
            _instructorRepo = instructorRepo;
            _db = db;
        }

        public async Task<GroupDTO> AddGroupAsync(CreateGroupDTO dto)
        {
            if (!string.IsNullOrEmpty(dto.InstructorId))
            {
                var instructor = await _instructorRepo.GetById(dto.InstructorId);
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

        public async Task DeleteGroupAsync(string id)
        {
            var group = await _groupRepo.GetById(id);
            if (group == null)
            {
                throw new ArgumentException($"Group with ID '{id}' not found.");
            }

            var groupStudents = _db.GroupStudents.Where(gs => gs.GroupId == id);
            _db.GroupStudents.RemoveRange(groupStudents);
            await _db.SaveChangesAsync();

            await _groupRepo.Delete(group);
        }

        public async Task<IReadOnlyList<GroupDTO>> GetAllGroupsAsync()
        {
            var groups = await _db.Groups
                .Include(g => g.Course)
                .Include(g => g.Instructor)
                .Include(g => g.GroupStudents)
                .AsNoTracking()
                .ToListAsync();

            return groups.Select(g => MapToDto(g)).ToList();
        }

        public async Task<GroupDTO> GetGroupByIdAsync(string id)
        {
            var group = await _db.Groups
                .Where(g => g.Id == id)
                .Include(g => g.Course)
                .Include(g => g.Instructor)
                .Include(g => g.GroupStudents)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (group == null)
            {
                throw new ArgumentException($"Group with ID '{id}' not found.");
            }

            return MapToDto(group);
        }

        public async Task<IReadOnlyList<GroupDTO>> GetGroupsByCourseIdAsync(string courseId)
        {
            var groups = await _db.Groups
                .Where(g => g.CourseId == courseId)
                .Include(g => g.Course)
                .Include(g => g.Instructor)
                .Include(g => g.GroupStudents)
                .AsNoTracking()
                .ToListAsync();

            return groups.Select(g => MapToDto(g)).ToList();
        }

        public async Task<IReadOnlyList<GroupDTO>> GetGroupsByInstructorIdAsync(string instructorId)
        {
            var groups = await _db.Groups
                .Where(g => g.InstructorId == instructorId)
                .Include(g => g.Course)
                .Include(g => g.Instructor)
                .Include(g => g.GroupStudents)
                .AsNoTracking()
                .ToListAsync();

            return groups.Select(g => MapToDto(g)).ToList();
        }

        public async Task<GroupDTO> UpdateGroupAsync(string id, UpdateGroupDTO dto)
        {
            var group = await _db.Groups
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (group == null)
            {
                throw new ArgumentException($"Group with ID '{id}' not found.");
            }

            if (!string.IsNullOrEmpty(dto.InstructorId))
            {
                var instructor = await _instructorRepo.GetById(dto.InstructorId);
                if (instructor == null)
                {
                    throw new ArgumentException($"Instructor with ID '{dto.InstructorId}' not found.");
                }
            }

            group.Label = dto.Label;
            group.StartTime = dto.StartTime;
            group.EndTime = dto.EndTime;
            group.Location = dto.Location;
            group.InstructorId = dto.InstructorId;

            await _db.SaveChangesAsync();

            return await GetGroupByIdAsync(group.Id);
        }
        public async Task<IReadOnlyList<GroupDTO>> GetGroupsByStudentIdAsync(string studentId)
        {
            var groups = await _db.GroupStudents
                .Where(gs => gs.StudentId == studentId)
                .Include(gs => gs.Group)
                    .ThenInclude(g => g.Course)
                .Include(gs => gs.Group.Instructor)
                .Include(gs => gs.Group.GroupStudents) 
                .Select(gs => gs.Group) 
                .AsNoTracking() 
                .ToListAsync();

            return groups.Select(g => MapToDto(g)).ToList();
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
                CourseResourceLink = group.Course?.ResourceLink,
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
    }
}