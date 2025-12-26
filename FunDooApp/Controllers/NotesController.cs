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
    private readonly ILogger<NotesController> _logger;

    public NotesController(
        INotesService notesService,
        IRedisCacheService cache,
        ILogger<NotesController> logger)
    {
        _notesService = notesService;
        _cache = cache;
        _logger = logger;
    }

    [HttpGet("{noteId}")]
    public async Task<IActionResult> GetNote(int noteId)
    {
        _logger.LogInformation("GetNote API called with NoteId: {NoteId}", noteId);

        string cacheKey = $"note_{noteId}";

        var cachedNote = await _cache.GetAsync(cacheKey);
        if (cachedNote != null)
        {
            _logger.LogInformation("NoteId {NoteId} found in Redis cache", noteId);

            var noteFromCache = JsonConvert.DeserializeObject<Notes>(cachedNote);
            return Ok(new
            {
                source = "Redis Cache",
                data = noteFromCache
            });
        }

        _logger.LogInformation("Cache miss for NoteId {NoteId}. Fetching from database", noteId);

        var noteFromDb = _notesService.GetNoteById(noteId);
        if (noteFromDb == null)
        {
            _logger.LogWarning("Note not found for NoteId: {NoteId}", noteId);
            return NotFound("Note not found");
        }

        var noteJson = JsonConvert.SerializeObject(noteFromDb);
        await _cache.SetAsync(cacheKey, noteJson, TimeSpan.FromMinutes(5));

        _logger.LogInformation("NoteId {NoteId} stored in Redis cache", noteId);

        return Ok(new
        {
            source = "Database",
            data = noteFromDb
        });
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
    [HttpPut("{noteId}")]
    public IActionResult UpdateNote(int noteId, Notes updatedNote)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var result = _notesService.UpdateNote(userId, noteId, updatedNote);

        if (!result)
            return NotFound("Note not found");

        return Ok("Note updated successfully");
    }
    [HttpDelete("{noteId}")]
    public IActionResult DeleteNote(int noteId)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var result = _notesService.DeleteNote(userId, noteId);

        if (!result)
            return NotFound("Note not found");

        return Ok("Note deleted successfully");
    }
}

