using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class Notes
    {
        [Key]
        public int NoteId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public ICollection<NoteLabel> NoteLabels { get; set; }


    }
}
