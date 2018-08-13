using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheTodoList.Entities
{
    public class Note : IHaveAnOwner
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<Reminder> Reminders { get; set; }

        public string OwnerId { get; set; }
    }
}
