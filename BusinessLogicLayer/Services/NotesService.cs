using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Interfaces;
using Microsoft.Extensions.Logging;
using ModelLayer.Entity;
using System.Collections.Generic;

namespace BusinessLogicLayer.Services
{
    public class NotesService : INotesService
    {
        private readonly INotesRepository _notesRepository;
        private readonly ILogger<NotesService> _logger;

        public NotesService(
            INotesRepository notesRepository,
            ILogger<NotesService> logger)
        {
            _notesRepository = notesRepository;
            _logger = logger;
        }

        public void AddNote(int userId, Notes note)
        {
            _logger.LogInformation("Adding note for UserId {UserId}", userId);
            _notesRepository.AddNote(userId, note);
        }

        public List<Notes> GetMyNotes(int userId)
        {
            _logger.LogInformation("Fetching notes for UserId {UserId}", userId);
            return _notesRepository.GetNotesByUserId(userId);
        }

        public Notes GetNoteById(int noteId)
        {
            _logger.LogInformation("Fetching note with NoteId {NoteId}", noteId);

            var note = _notesRepository.GetNoteById(noteId);

            if (note == null)
            {
                _logger.LogWarning("No note found for NoteId {NoteId}", noteId);
            }

            return note;
        }
    }
}
