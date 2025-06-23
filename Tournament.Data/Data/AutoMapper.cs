using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Tournament.Core.Entities;

namespace Tournament.Data.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // CreateMap<TournamentDetails, TournamentDetailsDto>();
        // CreateMap<Game, GameDto>();
        // CreateMap<GameCreateDto, Game>();
        // CreateMap<TournamentDetailsCreateDto, Game>();
        // CreateMap<Company, CompanyDto>().ForMember(dest => dest.Address, opt => opt.MapFrom
        //    (src => $"{src.Address}{(string.IsNullOrEmpty(src.Country) ? string.Empty : ", ")}{src.Country}"));
    }
}
