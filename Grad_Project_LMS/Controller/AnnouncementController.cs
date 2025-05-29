using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementController : ControllerBase
{
    private readonly IAnnouncementService _svc;
    public AnnouncementController(IAnnouncementService svc) => _svc = svc;
    
    [HttpPost]
    public async Task<ActionResult<AnnouncementDTO>> Post(CreateAnnouncementDTO dto)
        => Ok(await _svc.AddAnnouncementAsync(dto));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AnnouncementDTO>>> GetAll()
        => Ok(await _svc.GetAllAnnouncementsAsync());
}
