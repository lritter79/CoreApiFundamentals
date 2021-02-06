using AutoMapper;
using CoreCodeCamp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCodeCamp.Data
{
    public class TalkProfile : Profile
    {
        public TalkProfile()
        {
            this.CreateMap<Talk, TalkModel>()
                .ForMember(a => a.Speaker, f => f.MapFrom(v => v.Speaker));
        }
            
            
    }
}
