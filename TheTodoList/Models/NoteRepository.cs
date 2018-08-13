using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheTodoList.Entities;

namespace TheTodoList.Models
{
    public class NoteRepository : INoteRepository
    {
        TodoContext ctx;

        public NoteRepository(TodoContext ctx) => this.ctx = ctx;

        public async Task<List<Note>> Get()
        {
            return await ctx.Notes
                .Include(i => i.Reminders)
                .ToListAsync();
        }

        public async Task<Note> Get(long id)
        {
            return await ctx.Notes
                .Include(i => i.Reminders)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<int> Insert(Note item)
        {
            ctx.Notes.Add(item);
            return await ctx.SaveChangesAsync();
        }

        public async Task<int> Update(Note item)
        {
            ctx.Notes.Update(item);
            return await ctx.SaveChangesAsync();
        }

        public async Task<int> Delete(long id)
        {
            var item = ctx.Notes.FirstOrDefault(i => i.Id == id);

            if (item != null)
                ctx.Notes.Remove(item);

            return await ctx.SaveChangesAsync();
        }
    }
}
