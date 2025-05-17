using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface ILectureService
    {
        Task<LectureDTO> Add(AddLectureDTO lectureDTO);
        Task<LectureDTO> Update(AddLectureDTO lectureDTO );
        Task Delete(int id);
        Task<LectureDTO> Get(int id);
        Task<IReadOnlyList<LectureDTO>> GetAll();
        Task<PaginatedResultDTO<LectureDTO>> GetAllPaginated(int pagenumber = 1, int pagesize = 3);
    }
}
