using ModelLayer.DTOs;
using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICollaboratorService
    {
        Collaborator AddCollaborator(AddCollaboratorDto dto);
        List<Collaborator> GetCollaborators(int noteId);
        bool RemoveCollaborator(int collaboratorId);
        bool RemoveCollaboratorByEmail(int noteId, string email);
        List<Notes> GetSharedNotes(int userId);
    }
}
