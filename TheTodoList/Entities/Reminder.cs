using System;

namespace TheTodoList.Entities
{
    public class Reminder
    {
        public long Id { get; set; }
        public DateTime Time { get; set; }
        public Note Note { get; set; }
    }
}