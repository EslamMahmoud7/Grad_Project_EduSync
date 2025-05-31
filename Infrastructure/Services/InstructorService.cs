// Infrastructure/Services/InstructorService.cs
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.IServices;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IGenericRepository<Instructor> _instructorRepo;

        public InstructorService(IGenericRepository<Instructor> instructorRepo)
        {
            _instructorRepo = instructorRepo;
        }

        public async Task<InstructorDTO> AddInstructorAsync(CreateInstructorDTO dto)
        {
            var existingInstructor = (await _instructorRepo.GetAll()).FirstOrDefault(i => i.Email == dto.Email);
            if (existingInstructor != null)
            {
                throw new ArgumentException($"Instructor with email '{dto.Email}' already exists.");
            }

            var instructor = new Instructor
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            await _instructorRepo.Add(instructor);
            return MapToDto(instructor);
        }

        public async Task DeleteInstructorAsync(string id)
        {
            var instructor = await _instructorRepo.GetById(id);
            if (instructor == null)
            {
                throw new ArgumentException($"Instructor with ID '{id}' not found.");
            }

            await _instructorRepo.Delete(instructor);
        }

        public async Task<IReadOnlyList<InstructorDTO>> GetAllInstructorsAsync()
        {
            var instructors = await _instructorRepo.GetAll();
            return instructors.Select(MapToDto).ToList();
        }

        public async Task<InstructorDTO> GetInstructorByIdAsync(string id)
        {
            var instructor = await _instructorRepo.GetById(id);
            if (instructor == null)
            {
                throw new ArgumentException($"Instructor with ID '{id}' not found.");
            }
            return MapToDto(instructor);
        }

        public async Task<InstructorDTO> UpdateInstructorAsync(string id, UpdateInstructorDTO dto)
        {
            var instructor = await _instructorRepo.GetById(id);
            if (instructor == null)
            {
                throw new ArgumentException($"Instructor with ID '{id}' not found.");
            }

            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != instructor.Email)
            {
                var existingInstructor = (await _instructorRepo.GetAll()).FirstOrDefault(i => i.Email == dto.Email && i.Id != id);
                if (existingInstructor != null)
                {
                    throw new ArgumentException($"Instructor with email '{dto.Email}' already exists.");
                }
            }

            if (dto.FirstName != null) instructor.FirstName = dto.FirstName;
            if (dto.LastName != null) instructor.LastName = dto.LastName;
            if (dto.Email != null) instructor.Email = dto.Email;
            if (dto.PhoneNumber != null) instructor.PhoneNumber = dto.PhoneNumber;

            await _instructorRepo.Update(instructor);
            return MapToDto(instructor);
        }

        private InstructorDTO MapToDto(Instructor instructor)
        {
            return new InstructorDTO
            {
                Id = instructor.Id,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                Email = instructor.Email,
                PhoneNumber = instructor.PhoneNumber
            };
        }
    }
}