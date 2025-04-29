using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PaginationService : IPaginationService
    {
        private readonly ILogger<PaginationService> _logger;

        public PaginationService(ILogger<PaginationService> logger)
        {
            _logger = logger;
        }
        public PaginatedResultDTO<T> Paginate<T>(IQueryable<T> Source, int pagenumber, int pagesize)
        {
            if (pagenumber < 0 || pagesize < 0) _logger.LogError("page number size must be more than 0");
            var totalcount = Source.Count();
            var PaginationData = Source.Skip((pagenumber - 1)* pagesize)
                .Take(pagesize)
                .ToList();
            var totalpages = (int)Math.Ceiling((double)totalcount / pagesize);
            return new PaginatedResultDTO<T>
            {
                Data = PaginationData,
                TotalCount = totalcount,
                TotalPages = totalpages,
                PageSize = pagesize,
                PageNumber = pagenumber
            };


        }
    }
}
