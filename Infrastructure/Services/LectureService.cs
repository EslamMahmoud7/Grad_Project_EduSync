using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IServices;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class LectureService : ILectureService
    {
        private readonly IGenericRepository<Lecture> _genericRepository;
        private readonly IPaginationService _paginationService;

        public LectureService(IGenericRepository<Lecture> genericRepository, IPaginationService paginationService)
        {
            _genericRepository = genericRepository;
            _paginationService = paginationService;
        }
        public async Task<LectureDTO> Add(AddLectureDTO lectureDTO)
        {
            var Lecture = new Lecture()
            {
                Description = lectureDTO.Description,
                Type = lectureDTO.Type,
                Time = lectureDTO.Time,
                Feedback = lectureDTO.Feedback,
                CourseId = lectureDTO.CourseId,
                Location = lectureDTO.Location,
                Video = lectureDTO.Video,
                Title = lectureDTO.Title,
            };
            await _genericRepository.Add(Lecture);
            return ReturnToLectureDTO(Lecture);
        }

        public LectureDTO ReturnToLectureDTO(Lecture lecture)
        {
            return new LectureDTO()
            {
                Id = lecture.Id,
                CourseId = lecture.CourseId,
                Title = lecture.Title,
                Video = lecture.Video,
                Feedback = lecture.Feedback,
                Description = lecture.Description,
                Type = lecture.Type,
                Time = lecture.Time,
                Location = lecture.Location,
                CourseName = lecture.Course?.CourseName ?? "unknoiwn"

            };
        }

        public async Task Delete(int id)
        {
            var Lecture = await _genericRepository.GetById(id);
            if (Lecture == null) throw new ArgumentException("Lecture not exist");
            await _genericRepository.Delete(Lecture);
        }

        public async Task<LectureDTO> Get(int id)
        {
            var Lecture = await _genericRepository.GetById(id);
            if (Lecture == null) throw new ArgumentException("Lecture not exist");
            return ReturnToLectureDTO(Lecture);
        }

        public async Task<IReadOnlyList<LectureDTO>> GetAll()
        {
            var Lecture = await _genericRepository.GetAll();
            return Lecture.Select(ReturnToLectureDTO).ToList();
        }

        public async Task<PaginatedResultDTO<LectureDTO>> GetAllPaginated(int pagenumber = 1, int pagesize = 3)
        {
            var Lecture = await _genericRepository.GetAll();
            var IQuerableCoursed = Lecture.AsQueryable();

            var LectureDTO = IQuerableCoursed.Select(c => ReturnToLectureDTO(c)).ToList().AsQueryable();

            var PaginatedResult = _paginationService.Paginate(LectureDTO, pagenumber, pagesize);
            return PaginatedResult;
        }

        public async Task<LectureDTO> Update(AddLectureDTO lectureDTO)
        {
            var Lecture = await _genericRepository.GetById(lectureDTO.Id);
            if (Lecture == null) throw new ArgumentException("course is null");
            Lecture.Feedback = lectureDTO.Feedback;
            //Lecture.Course.CourseName = lec;
            Lecture.Id = lectureDTO.Id;
            Lecture.CourseId = lectureDTO.CourseId;
            Lecture.Video = lectureDTO.Video;
            Lecture.Description = lectureDTO.Description;
            Lecture.Type = lectureDTO.Type;
            Lecture.Time = lectureDTO.Time;
            Lecture.Duration = lectureDTO.Duration;
            Lecture.Location = lectureDTO.Location;
            await _genericRepository.Update(Lecture);
            return ReturnToLectureDTO(Lecture);
        }
    }
}
