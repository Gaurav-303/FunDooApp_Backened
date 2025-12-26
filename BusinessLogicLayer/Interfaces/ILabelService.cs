using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Entity;


namespace BusinessLogicLayer.Interfaces
{
    public interface ILabelService
    {
        Label CreateLabel(int userId, string labelName);
        List<Label> GetMyLabels(int userId);
     
        void AssignLabelToNote(int noteId, int labelId);
       
    }
}
