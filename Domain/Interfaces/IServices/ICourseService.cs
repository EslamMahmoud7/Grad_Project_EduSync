using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface ICourseService
    {
        Task<CourseDTO> Add(CourseDTO courseDTO);
        Task<CourseDTO> Update(CourseDTO courseDTO);
        Task Delete(int id);
        Task<CourseDTO> Get(int id);
        Task<IReadOnlyList<CourseDTO>> GetAll();
    }
}
