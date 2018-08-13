using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTodoList.Entities;
using TheTodoList.ViewModels;

namespace TheTodoList.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Note, NoteViewModel>()
                .ForMember(ivm => ivm.ItemId, ex => ex.MapFrom(i => i.Id))
                .ReverseMap();

            CreateMap<Reminder, ReminderViewModel>()
                .ReverseMap();
        }
    }
}
