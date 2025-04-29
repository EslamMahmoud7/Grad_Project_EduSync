using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPaginationService
    {
        PaginatedResultDTO<T> Paginate<T>(IQueryable<T> Source, int PageNumber, int PageSize);
    }
}
