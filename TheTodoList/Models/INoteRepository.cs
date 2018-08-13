using System.Collections.Generic;
using System.Threading.Tasks;
using TheTodoList.Entities;

namespace TheTodoList.Models
{
    public interface INoteRepository
    {
        Task<int> Delete(long id);
        Task<List<Note>> Get();
        Task<Note> Get(long id);
        Task<int> Insert(Note item);
        Task<int> Update(Note item);
    }
}