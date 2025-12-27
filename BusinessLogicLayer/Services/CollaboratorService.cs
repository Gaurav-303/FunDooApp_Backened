using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Context;
using DataLogicLayer.Interfaces;

using ModelLayer.DTOs;
using ModelLayer.Entity;

public class CollaboratorService : ICollaboratorService
{
    private readonly ICollaboratorRepository _repo;
    private readonly FundooContext _context;

    public CollaboratorService(ICollaboratorRepository repo, FundooContext context)
    {
        _repo = repo;
        _context = context;
    }

    public Collaborator AddCollaborator(AddCollaboratorDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
        if (user == null) return null;

        return _repo.Add(dto.NoteId, user.UserId);
    }

    public List<Collaborator> GetCollaborators(int noteId)
    {
        return _repo.GetByNoteId(noteId);
    }

    public bool RemoveCollaborator(int collaboratorId)
    {
        return _repo.RemoveById(collaboratorId);
    }

    public bool RemoveCollaboratorByEmail(int noteId, string email)
    {
        return _repo.RemoveByEmail(noteId, email);
    }

    public List<Notes> GetSharedNotes(int userId)
    {
        return _repo.GetSharedNotes(userId);
    }
}
