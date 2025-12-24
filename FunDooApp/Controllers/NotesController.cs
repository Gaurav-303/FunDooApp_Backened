using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using Newtonsoft.Json;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly INotesService _notesService;
    private readonly IRedisCacheService _cache;

    public NotesController(INotesService notesService, IRedisCacheService cache)
    {
        _notesService = notesService;
        _cache = cache;
    }

    [HttpGet("{noteId}")]
    public async Task<IActionResult> GetNote(int noteId)
    {
        string cacheKey = $"note_{noteId}";

        var cachedNote = await _cache.GetAsync(cacheKey);
        if (cachedNote != null)
        {
            var note = JsonConvert.DeserializeObject<Notes>(cachedNote);
            return Ok(new { source = "Redis Cache", data = note });
        }

        var noteFromDb = _notesService.GetNoteById(noteId);
        if (noteFromDb == null)
        {
            return NotFound("Note not found");
        }

        var noteJson = JsonConvert.SerializeObject(noteFromDb);
        await _cache.SetAsync(cacheKey, noteJson, TimeSpan.FromMinutes(5));

        return Ok(new { source = "Database", data = noteFromDb });
    }

    [HttpPost]
    public IActionResult AddNote(Notes note)
    {
        int userId = GetUserId();
        _notesService.AddNote(userId, note);

        return Ok("Note created");
    }

    [HttpGet]
    public IActionResult GetMyNotes()
    {
        int userId = GetUserId();
        return Ok(_notesService.GetMyNotes(userId));
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
