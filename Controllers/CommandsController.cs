using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController:ControllerBase
    {
        private readonly ICommanderRepo _repositery;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repositery,IMapper mapper)
        {
            _repositery = repositery;
            _mapper = mapper;
        }
       [HttpGet]
       public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
       {
           var commandItems = _repositery.GetAllCommands();
           return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
       }

       [HttpGet("{id}", Name="GetCommandById")]
       public ActionResult<CommandReadDto> GetCommandById(int id)
       {
           var commandItem = _repositery.GetCommandById(id);
           if(commandItem!= null)
           {
                 return Ok(_mapper.Map<CommandReadDto>(commandItem));
           }
           return NotFound();
       }
       //api/commands
       [HttpPost]
       public ActionResult<CommandReadDto> CreateCommand (CommandCreateDto commandCreateDto)
       {
           var commandModel = _mapper.Map<Command>(commandCreateDto);
           _repositery.CreateCommand(commandModel);
           _repositery.SaveChanges();
           var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);
           return CreatedAtRoute(nameof(GetCommandById),
               new {Id=commandReadDto.Id},commandReadDto);
           //return Ok(commandReadDto);
       }
       [HttpPut("{id}")]
       public ActionResult UpdateCommand(int id,CommandUpdateDto commandUpdateDto)
       {
           var commandModelFromRepo = _repositery.GetCommandById(id);
           if(commandModelFromRepo == null){
               return NotFound();
           }
           _mapper.Map(commandUpdateDto,commandModelFromRepo);

           _repositery.UpdateCommand(commandModelFromRepo);

           _repositery.SaveChanges();

           return NoContent();
       }

       //PATCH api/commands/{id}
       [HttpPatch("{id}")]
       public ActionResult PartialCommandupdate(int id,JsonPatchDocument<CommandUpdateDto> patchDoc)
       {
           var commandModelFromRepo = _repositery.GetCommandById(id);
           if(commandModelFromRepo == null){
               return NotFound();
           }
           var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
           patchDoc.ApplyTo(commandToPatch, ModelState);
           if(!TryValidateModel(commandToPatch))
           {
               return ValidationProblem(ModelState);
           }
           _mapper.Map(commandToPatch,commandModelFromRepo);

           _repositery.UpdateCommand(commandModelFromRepo);

           _repositery.SaveChanges();

           return NoContent();
       }
       [HttpDelete("{id}")]
       public ActionResult<Command> DeleteCommand(int id)
       {
           var commandModelFromRepo = _repositery.GetCommandById(id);
           if(commandModelFromRepo == null){
               return NotFound();
           }
           _repositery.DeleteCommand(commandModelFromRepo);

           _repositery.SaveChanges();

           return NoContent();
       }

    }
}