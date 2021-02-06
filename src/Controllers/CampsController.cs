using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace CoreCodeCamp.Controllers
{
    //[controller] converts to the word that comes before "Controller" in the class name 
    //route + verb is how you get an action
    //the action is the end point
    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;

        public CampsController(ICampRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        //get individual model

        //Iactionresult because we return action from this method

        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get()
        {
            try
            {
                //if you can an sync function, the action should be async too
                //this is a task of camps, not an operation
                var results = await _repository.GetAllCampsAsync();

                CampModel[] models = _mapper.Map<CampModel[]>(results);

                //if (false) return badrequest("bad stuff");
                //if (false) return notfound("bad stuff");

                return Ok(models);
            }
            catch (Exception e){
                var y = e.Message;
                //to return any status code
                //return this.StatusCode(StatusCodes.xxx);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }
    }
}
