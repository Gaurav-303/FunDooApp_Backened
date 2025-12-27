using DataLogicLayer.Context;
using DataLogicLayer.Interfaces;
using ModelLayer.Entity;
using Microsoft.EntityFrameworkCore;

public class CollaboratorRepository : ICollaboratorRepository
{
    private readonly FundooContext _context;

    public CollaboratorRepository(FundooContext context)
    {
        _context = context;
    }

    public Collaborator Add(int noteId, int userId)
    {
        var collaborator = new Collaborator
        {
            NoteId = noteId,
            UserId = userId
        };

        _context.Collaborators.Add(collaborator);
        _context.SaveChanges();
        return collaborator;
    }

    public List<Collaborator> GetByNoteId(int noteId)
    {
        return _context.Collaborators
            .Include(c => c.User)
            .Where(c => c.NoteId == noteId)
            .ToList();
    }

    public bool RemoveById(int collaboratorId)
    {
        var collaborator = _context.Collaborators.Find(collaboratorId);
        if (collaborator == null) return false;

        _context.Collaborators.Remove(collaborator);
        _context.SaveChanges();
        return true;
    }

    public bool RemoveByEmail(int noteId, string email)
    {
        var collaborator = _context.Collaborators
            .Include(c => c.User)
            .FirstOrDefault(c => c.NoteId == noteId && c.User.Email == email);

        if (collaborator == null) return false;

        _context.Collaborators.Remove(collaborator);
        _context.SaveChanges();
        return true;
    }

    public List<Notes> GetSharedNotes(int userId)
    {
        return _context.Collaborators
            .Include(c => c.Note)
            .Where(c => c.UserId == userId)
            .Select(c => c.Note)
            .ToList();
    }
}
