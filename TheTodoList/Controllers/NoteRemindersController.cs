using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheTodoList.Entities;
using TheTodoList.Models;
using TheTodoList.ViewModels;

namespace TheTodoList.Controllers
{
    [Route("/api/Notes/{noteId:long}/Reminders")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NoteRemindersController : Controller
    {
        private readonly INoteRepository repository;
        private readonly ILogger<NoteRemindersController> logger;
        private readonly IMapper mapper;

        public NoteRemindersController(INoteRepository repository, ILogger<NoteRemindersController> logger, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(long noteId)
        {
            try
            {
                var note = await repository.Get(noteId);

                if (note != null)
                {
                    return Ok(mapper.Map<IEnumerable<Reminder>, IEnumerable<ReminderViewModel>>(note.Reminders));
                }
                else
                {
                    return NotFound($"Note {noteId} not Found");
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to get reminders for note {noteId}");
                return BadRequest($"Failed to get reminders for note {noteId}");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long noteId, long id)
        {
            try
            {
                var note = await repository.Get(noteId);

                if (note != null)
                {
                    var reminder = note.Reminders.FirstOrDefault(r => r.Id == id);

                    if (reminder != null)
                    {
                        return Ok(mapper.Map<Reminder, ReminderViewModel>(reminder));
                    }
                    else
                    {
                        return NotFound($"Reminder {id} not Found");
                    }
                }
                else
                {
                    return NotFound($"Note {noteId} not Found");
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to get reminder {id} for note {noteId}");
                return BadRequest($"Failed to get reminder {id} for note {noteId}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(long noteId, ReminderViewModel reminder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var note = await repository.Get(noteId);

                    if (note != null)
                    {
                        var reminderEntity = mapper.Map<ReminderViewModel, Reminder>(reminder);

                        note.Reminders.Add(reminderEntity);
                        await repository.Update(note);

                        reminder = mapper.Map<Reminder, ReminderViewModel>(reminderEntity);

                        return Created($"/api/Notes/{noteId}/Reminders/{reminder.Id}", reminder);
                    }
                    else
                    {
                        return NotFound($"Note {noteId} not Found");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to insert reminder for note {noteId}");
                return BadRequest($"Failed to insert reminder for note {noteId}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long noteId, long id, ReminderViewModel reminder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var note = await repository.Get(noteId);

                    if (note != null)
                    {
                        var reminderEntity = note.Reminders.FirstOrDefault(r => r.Id == id);

                        if (reminderEntity != null)
                        {
                            reminder.Id = id;
                            mapper.Map(reminder, reminderEntity);

                            return Ok(await repository.Update(note));
                        }
                        else
                        {
                            return NotFound($"Reminder {id} not Found in Note {noteId}");
                        }
                    }
                    else
                    {
                        return NotFound($"Note {noteId} not Found");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to insert reminder for note {noteId}");
                return BadRequest($"Failed to insert reminder for note {noteId}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long noteId, long id)
        {
            try
            {
                var note = await repository.Get(noteId);

                if (note != null)
                {
                    var reminderEntity = note.Reminders.FirstOrDefault(r => r.Id == id);

                    if (reminderEntity != null)
                    {
                        note.Reminders.Remove(reminderEntity);

                        return Ok(await repository.Update(note));
                    }
                    else
                    {
                        return NotFound($"Reminder {id} not Found in Note {noteId}");
                    }
                }
                else
                {
                    return NotFound($"Note {noteId} not Found");
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Failed to insert reminder for note {noteId}");
                return BadRequest($"Failed to insert reminder for note {noteId}");
            }
        }
    }
}
