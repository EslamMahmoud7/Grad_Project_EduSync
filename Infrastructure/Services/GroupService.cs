using Domain.DTOs;
using Domain.Entities; // Required for UserRole enum
using Domain.Interfaces.IServices;
using Domain.Interfaces.Repositories; // IGenericRepository<Group>
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity; // Required for UserManager<User>
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
        private readonly UserManager<User> _userManager; // NEW: Replaces _instructorRepo

        public GroupService(
            IGenericRepository<Group> groupRepo,
            MainDbContext db,
            UserManager<User> userManager) // MODIFIED: Added UserManager, removed IGenericRepository<Instructor>
        {
            _groupRepo = groupRepo;
            _db = db;
            _userManager = userManager; // NEW
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
                // Validate against Users table with Instructor role
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
                InstructorId = dto.InstructorId // This is now a UserId
            };

            await _groupRepo.Add(group);
            // Ensure GetGroupByIdAsync includes the User navigation property for Instructor
            return await GetGroupByIdAsync(group.Id);
        }

        public async Task<GroupDTO> UpdateGroupAsync(string id, UpdateGroupDTO dto)
        {
            var group = await _db.Groups.FindAsync(id); // Fetch without includes for update
            if (group == null)
                throw new ArgumentException($"Group with ID '{id}' not found.");

            // CourseId update logic (assuming it can be updated)
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
                else // dto.InstructorId is null or empty, meaning unassign
                {
                    group.InstructorId = null;
                }
            }

            if (dto.Label != null) // Allow clearing the label if an empty string is passed
                group.Label = dto.Label;

            if (dto.Location != null) // Allow clearing the location
                group.Location = dto.Location;

            await _groupRepo.Update(group);
            // Ensure GetGroupByIdAsync includes the User navigation property for Instructor
            return await GetGroupByIdAsync(id);
        }

        private GroupDTO MapToDto(Group group)
        {
            // The group.Instructor navigation property should now be of type User
            // and should be included when fetching the group for this mapping.
            return new GroupDTO
            {
                Id = group.Id,
                CourseId = group.CourseId,
                CourseTitle = group.Course?.Title ?? "N/A",
                CourseDescription = group.Course?.Description,
                CourseCredits = group.Course?.Credits ?? 0,
                CourseLevel = group.Course?.Level ?? 0, // Assuming CourseLevel is part of Course entity/DTO
                Label = group.Label,
                StartTime = group.StartTime,
                EndTime = group.EndTime,
                Location = group.Location,
                Instructor = group.Instructor != null ? new InstructorDTO // Instructor is now a User
                {
                    Id = group.Instructor.Id,
                    FirstName = group.Instructor.FirstName,
                    LastName = group.Instructor.LastName,
                    Email = group.Instructor.Email // Assuming User entity has these properties
                } : null,
                NumberOfStudents = group.GroupStudents?.Count ?? 0 // Requires GroupStudents to be included
            };
        }

        public async Task<IReadOnlyList<GroupDTO>> GetGroupsByStudentIdAsync(string studentId)
        {
            var groups = await _db.GroupStudents
               .Where(gs => gs.StudentId == studentId)
               .Include(gs => gs.Group).ThenInclude(g => g.Course)
               .Include(gs => gs.Group).ThenInclude(g => g.Instructor) // Instructor is now User
               .Include(gs => gs.Group).ThenInclude(g => g.GroupStudents)
               .Select(gs => gs.Group)
               .Distinct()
               .AsNoTracking().ToListAsync();
            return groups.Select(g => MapToDto(g!)).ToList();
        }

        public async Task DeleteGroupAsync(string id)
        {
            var group = await _groupRepo.GetById(id); // GetById should ideally not track for deletion
            if (group == null) throw new ArgumentException($"Group with ID '{id}' not found.");

            // Manually remove associations if not handled by cascade or if specific logic needed
            var groupStudents = _db.GroupStudents.Where(gs => gs.GroupId == id);
            if (await groupStudents.AnyAsync())
            {
                _db.GroupStudents.RemoveRange(groupStudents);
                // Consider if SaveChangesAsync is needed here before deleting group,
                // if _groupRepo.Delete doesn't handle it or if there are constraints.
            }
            await _groupRepo.Delete(group); // This should call SaveChangesAsync
        }

        public async Task<IReadOnlyList<GroupDTO>> GetAllGroupsAsync()
        {
            var groups = await _db.Groups
                .Include(g => g.Course)
                .Include(g => g.Instructor) // Instructor is now User
                .Include(g => g.GroupStudents)
                .AsNoTracking().ToListAsync();
            return groups.Select(MapToDto).ToList();
        }

        public async Task<GroupDTO> GetGroupByIdAsync(string id)
        {
            var group = await _db.Groups.Where(g => g.Id == id)
                .Include(g => g.Course)
                .Include(g => g.Instructor) // Instructor is now User
                .Include(g => g.GroupStudents)
                .AsNoTracking().FirstOrDefaultAsync();
            if (group == null) throw new ArgumentException($"Group with ID '{id}' not found.");
            return MapToDto(group);
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
            // instructorId is now a UserId
            var groups = await _db.Groups.Where(g => g.InstructorId == instructorId)
                .Include(g => g.Course)
                .Include(g => g.Instructor) // Instructor is now User
                .Include(g => g.GroupStudents)
                .AsNoTracking().ToListAsync();
            return groups.Select(MapToDto).ToList();
        }
    }
}