using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CollaboratorsController : ControllerBase
{
    private readonly ICollaboratorService _service;

    public CollaboratorsController(ICollaboratorService service)
    {
        _service = service;
    }

    // POST /api/collaborators
    [HttpPost]
    public IActionResult AddCollaborator(AddCollaboratorDto dto)
    {
        var result = _service.AddCollaborator(dto);
        if (result == null) return BadRequest("User not found");

        return Ok(result);
    }

    // GET /api/collaborators/{noteId}
    [HttpGet("{noteId}")]
    public IActionResult GetCollaborators(int noteId)
    {
        return Ok(_service.GetCollaborators(noteId));
    }

    // DELETE /api/collaborators/{collaboratorId}
    [HttpDelete("{collaboratorId}")]
    public IActionResult RemoveCollaborator(int collaboratorId)
    {
        return Ok(_service.RemoveCollaborator(collaboratorId));
    }

    // DELETE /api/collaborators/{noteId}/{email}
    [HttpDelete("{noteId}/{email}")]
    public IActionResult RemoveCollaboratorByEmail(int noteId, string email)
    {
        return Ok(_service.RemoveCollaboratorByEmail(noteId, email));
    }

    // GET /api/collaborators/shared-notes
    [HttpGet("shared-notes")]
    public IActionResult GetSharedNotes()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        return Ok(_service.GetSharedNotes(userId));
    }
}
