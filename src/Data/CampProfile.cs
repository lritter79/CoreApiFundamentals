using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            //default version
            //create mapping between classes
            //this.CreateMap<Camp, CampModel>();

            this.CreateMap<Camp, CampModel>()               
                            //1st param: pick property in the object
                .ForMember(c => c.Venue, o =>
                ////.MapFrom() = "where is this actually coming from?"
                o.MapFrom(m => m.Location.VenueName))
                .ForMember(c => c.Talks, o => o.MapFrom(m => m.Talks));

            
        }
        
    }
}
