using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTodoList.ViewModels
{
    public class NoteViewModel
    {
        public long ItemId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
