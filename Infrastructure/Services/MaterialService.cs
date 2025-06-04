using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly MainDbContext _context;

        public MaterialService(MainDbContext context)
        {
            _context = context;
        }

        private MaterialDTO MapMaterialToDto(Material material)
        {
            return new MaterialDTO
            {
                Id = material.Id,
                Title = material.Title,
                Description = material.Description,
                FileUrl = material.FileUrl,
                Type = material.Type,
                GroupId = material.GroupId,
                GroupLabel = material.Group?.Label ?? "N/A",
                CourseTitle = material.Group?.Course?.Title ?? "N/A",
                UploadedById = material.UploadedById,
                UploadedByName = $"{material.UploadedBy?.FirstName} {material.UploadedBy?.LastName}".Trim(),
                DateUploaded = material.DateUploaded
            };
        }

        public async Task<MaterialDTO> AddMaterialAsync(CreateMaterialDTO dto)
        {
            var group = await _context.Groups.FindAsync(dto.GroupId);
            if (group == null)
                throw new ArgumentException($"Group with ID '{dto.GroupId}' not found.");

            var uploader = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.UploadingInstructorId && u.Role == UserRole.Instructor);
            if (uploader == null)
                throw new ArgumentException($"Uploading user (Instructor) with ID '{dto.UploadingInstructorId}' not found or is not an instructor.");


            var material = new Material
            {
                Title = dto.Title,
                Description = dto.Description,
                FileUrl = dto.FileUrl,
                Type = dto.Type,
                GroupId = dto.GroupId,
                UploadedById = dto.UploadingInstructorId,
                DateUploaded = DateTime.UtcNow
            };

            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            await _context.Entry(material).Reference(m => m.Group).LoadAsync();
            if (material.Group != null) await _context.Entry(material.Group).Reference(g => g.Course).LoadAsync();
            await _context.Entry(material).Reference(m => m.UploadedBy).LoadAsync();

            return MapMaterialToDto(material);
        }

        public async Task<MaterialDTO?> GetMaterialByIdAsync(string materialId)
        {
            var material = await _context.Materials
                .Include(m => m.Group).ThenInclude(g => g!.Course)
                .Include(m => m.UploadedBy)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == materialId);

            return material == null ? null : MapMaterialToDto(material);
        }

        public async Task<IReadOnlyList<MaterialDTO>> GetMaterialsByGroupIdAsync(string groupId)
        {
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == groupId);
            if (!groupExists)
            {
                throw new ArgumentException($"Group with ID '{groupId}' not found, cannot fetch materials.");
            }

            var materials = await _context.Materials
                .Where(m => m.GroupId == groupId)
                .Include(m => m.Group).ThenInclude(g => g!.Course)
                .Include(m => m.UploadedBy)
                .OrderByDescending(m => m.DateUploaded)
                .AsNoTracking()
                .ToListAsync();

            return materials.Select(MapMaterialToDto).ToList();
        }

        public async Task<MaterialDTO> UpdateMaterialAsync(string materialId, UpdateMaterialDTO dto)
        {
            var material = await _context.Materials
                .Include(m => m.Group)
                .FirstOrDefaultAsync(m => m.Id == materialId);

            if (material == null)
                throw new ArgumentException($"Material with ID '{materialId}' not found.");

            var updater = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.UpdatingInstructorId && u.Role == UserRole.Instructor);
            if (updater == null)
                throw new ArgumentException($"Updating user (Instructor) with ID '{dto.UpdatingInstructorId}' not found or is not an instructor.");


            if (dto.Title != null) material.Title = dto.Title;
            material.Description = dto.Description ?? material.Description;
            if (dto.FileUrl != null) material.FileUrl = dto.FileUrl;
            if (dto.Type.HasValue) material.Type = dto.Type.Value;


            _context.Materials.Update(material);
            await _context.SaveChangesAsync();

            await _context.Entry(material).Reference(m => m.Group).LoadAsync();
            if (material.Group != null) await _context.Entry(material.Group).Reference(g => g.Course).LoadAsync();
            await _context.Entry(material).Reference(m => m.UploadedBy).LoadAsync();

            return MapMaterialToDto(material);
        }

        public async Task<bool> DeleteMaterialAsync(string materialId)
        {
            var material = await _context.Materials
                .Include(m => m.Group)
                .FirstOrDefaultAsync(m => m.Id == materialId);

            if (material == null)
                return false;

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}