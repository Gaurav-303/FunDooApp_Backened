using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Entity;

namespace ModelLayer.Entity
{
    public class Collaborator
    {
        [Key]
        public int CollaboratorId { get; set; }

        [Required]
        public int NoteId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(NoteId))]
        public Notes Note { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users User { get; set; }
    }


}
