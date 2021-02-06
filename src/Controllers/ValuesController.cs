
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers
{
    //[controller] converts to the word that comes before "Controller" in the class name 
    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {

        //HttpGetAttribute 
        public Object Get()
        {
            //return an anonymous object
            return new { Moniker = "ATL2018", Name = "Atll code camp" };
        }
    }
}
