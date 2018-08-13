using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheTodoList.Entities;
using TheTodoList.Models;
using TheTodoList.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheTodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotesController : Controller
    {
        private readonly INoteRepository rep;
        private readonly ILogger<NotesController> logger;
        private readonly IMapper mapper;

        public NotesController(INoteRepository rep, ILogger<NotesController> logger, IMapper mapper)
        {
            this.rep = rep;
            this.logger = logger;
            this.mapper = mapper;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteViewModel>>> Get()
        {
            try
            {
                return Ok(mapper.Map<IEnumerable<Note>,IEnumerable<NoteViewModel>>(await rep.Get()));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get Items");
                return BadRequest("Failed to get Items!");
            }

        }

        // GET api/<controller>/5
        [HttpGet("{id:long}")]
        public async Task<ActionResult<NoteViewModel>> Get(long id)
        {
            try
            {
                var result = await rep.Get(id);

                if (result != null)
                {
                    return Ok(mapper.Map<Note,NoteViewModel>(result));
                }
                else
                {
                    return NotFound($"Note {id} not Found");
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to get Item {id}");
                return BadRequest($"Failed to get Item {id}");
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]NoteViewModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var item = mapper.Map<NoteViewModel, Note>(value);

                    await rep.Insert(item);

                    value = mapper.Map<Note, NoteViewModel>(item);

                    return Created($"/api/Notes/{value.ItemId}", value);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to insert Note");
                return BadRequest("Failed to insert Note");
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id:long}")]
        public async Task<ActionResult<int>> Put(long id, [FromBody]NoteViewModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var item = await rep.Get(id);

                    if(item != null)
                    {
                        value.ItemId = id;
                        mapper.Map<NoteViewModel, Note>(value, item);
                        return Ok(await rep.Update(item));
                    }
                    else
                    {
                        return NotFound($"Note {id} Not Found");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to update Item {id}");
                return BadRequest($"Failed to update Item {id}");
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id:long}")]
        public async Task<ActionResult<int>> Delete(long id)
        {
            try
            {
                var note = await rep.Get(id);

                if(note != null)
                {
                    return Ok(await rep.Delete(id));
                }
                else
                {
                    return NotFound($"Note {id} Not Found");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to delete Item {id}");
                return BadRequest($"Failed to delete Item {id}");
            }
        }
    }
}
