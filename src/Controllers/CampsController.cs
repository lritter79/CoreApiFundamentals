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
using Microsoft.AspNetCore.Routing;

namespace CoreCodeCamp.Controllers
{
    //[controller] converts to the word that comes before "Controller" in the class name 
    //route + verb is how you get an action
    //the action is the end point
    [Route("api/[controller]")]
    //[ApiController] tell the program we're using it as an API and adjust its expectation accordingly, so we're not returning views or html
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public CampsController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;

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
        //from body tells it to map request from its body
        public async Task<ActionResult<CampModel>> Post([FromBody]CampModel model)
        {
            try
            {
                //validation done on the model to make sure it doenst already exist
                var existing = await _repository.GetCampAsync(model.Moniker);
                if (existing != null)
                {
                    return BadRequest("Moniker in use");
                }


                //return the uri for the new camp w/out hard coding
                //name of the action, controler, and route values as new object with an anonymous type to store moniker to use get by moniker
                var location = _linkGenerator.GetPathByAction("Get", "Camps", new{ moniker = model.Moniker });
                if (String.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Coundnt set current moniker");
                }

                var camp = _mapper.Map<Camp>(model);
                _repository.Add(camp);
                
                if(await _repository.SaveChangesAsync())
                {
                    return Created($"/api/camps/{camp.Moniker}", _mapper.Map<CampModel>(camp));
                }
                return Ok();
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return BadRequest();
        }

        //put for rpelacing whole object as oppsoed to individual field
        [HttpPut("{moniker}")]
        public async Task<ActionResult<CampModel>> Put(string moniker, CampModel model)
        {
            try
            {
                var oldCamp = await _repository.GetCampAsync(model.Moniker);
                if (oldCamp == null) return NotFound($"Cound not find camp with moniker of {moniker}");

                //oldCamp.Name = model.Name;
                _mapper.Map(model, oldCamp);

                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<CampModel>(oldCamp);
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failed");
            }

            return BadRequest();
        }

        [HttpDelete("{moniker}")]
        public async Task<IActionResult> Delete(string moniker)
        {
            try
            {
                var oldCamp = await _repository.GetCampAsync(moniker);
                if (oldCamp == null) return NotFound($"Cound not find camp with moniker of {moniker}");

                _repository.Delete(oldCamp);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Delete failed");
            }

            return BadRequest();
        }
    }
}
