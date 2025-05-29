using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.IServices;
using Domain.Interfaces.Repositories;

public class AnnouncementService : IAnnouncementService
{
    private readonly IGenericRepository<Announcement> _repo;

    public AnnouncementService(IGenericRepository<Announcement> repo)
        => _repo = repo;

    public async Task<AnnouncementDTO> AddAnnouncementAsync(CreateAnnouncementDTO dto)
    {
        var n = new Announcement
        {
            Title = dto.Title,
            Message = dto.Message,
            Date = DateTime.UtcNow
        };
        await _repo.Add(n);
        return new AnnouncementDTO
        {
            Title = n.Title,
            Message = n.Message,
            Date = n.Date
        };
    }

    public async Task<IReadOnlyList<AnnouncementDTO>> GetAllAnnouncementsAsync()
    {
        var all = await _repo.GetAll();
        return all.OrderByDescending(a => a.Date)
            .Select(a => new AnnouncementDTO
            {
                Title = a.Title,
                Message = a.Message,
                Date = a.Date
            }).ToList();
    }
}
