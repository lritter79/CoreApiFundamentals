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

        [HttpGet("search")]
        public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime theDate, bool includeTalks = false)
        {
            try
            {
                var result = await _repository.GetAllCampsByEventDate(theDate, includeTalks);

                if (!result.Any()) return NotFound();

                //if (false) return badrequest("bad stuff");
                //if (false) return notfound("bad stuff");

                return _mapper.Map<CampModel[]>(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);

            }
        }

        //returning related dat to the entity


        //get individual model
        [HttpGet("{moniker}")]
        //[HttpGet("{moniker:int}")] to specify type
        public async Task<ActionResult<CampModel>> Get(string moniker)
        {
            try
            {
                var result = await _repository.GetCampAsync(moniker);

                if (result == null) return NotFound();

                //if (false) return badrequest("bad stuff");
                //if (false) return notfound("bad stuff");

                return _mapper.Map<CampModel>(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);

            }
        }

        /// <summary>
        /// HTTP get with qstring to include talks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get(bool includeTalks = false)
        {
            try
            {
                //if you can an sync function, the action should be async too
                //this is a task of camps, not an operation
                var results = await _repository.GetAllCampsAsync(includeTalks);

                CampModel[] models = _mapper.Map<CampModel[]>(results);

                //if (false) return badrequest("bad stuff");
                //if (false) return notfound("bad stuff");

                return Ok(models);
            }
            catch (Exception e)
            {
                var y = e.Message;
                //to return any status code
                //return this.StatusCode(StatusCodes.xxx);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        //Iactionresult because we return action from this method

        /// <summary>
        /// Basic http get
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public async Task<ActionResult<CampModel[]>> Get()
        //{
        //    try
        //    {
        //        //if you can an sync function, the action should be async too
        //        //this is a task of camps, not an operation
        //        var results = await _repository.GetAllCampsAsync();

        //        CampModel[] models = _mapper.Map<CampModel[]>(results);

        //        //if (false) return badrequest("bad stuff");
        //        //if (false) return notfound("bad stuff");

        //        return Ok(models);
        //    }
        //    catch (Exception e){
        //        var y = e.Message;
        //        //to return any status code
        //        //return this.StatusCode(StatusCodes.xxx);
        //        return this.StatusCode(StatusCodes.Status500InternalServerError);
        //    }

        //}


        /// 

    }
}
