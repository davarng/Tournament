using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Tournament.Data.Data;

public class TournamentMappings : Profile
{
    public TournamentMappings()
    {
        CreateMap<TournamentDetails, TournamentDto>().ReverseMap();
        CreateMap<TournamentCreateDto, TournamentDto>().ReverseMap();
        CreateMap<TournamentUpdateDto, TournamentDto>().ReverseMap();
        CreateMap<TournamentPatchDto, TournamentDto>().ReverseMap();
        CreateMap<Game, GameDto>().ReverseMap();
        CreateMap<GameCreateDto, Game>().ReverseMap();
        CreateMap<GameUpdateDto, Game>().ReverseMap();
        CreateMap<GamePatchDto, Game>().ReverseMap();

        // CreateMap<GameCreateDto, Game>();
        // CreateMap<TournamentDetailsCreateDto, Game>();
        // CreateMap<Company, CompanyDto>().ForMember(dest => dest.Address, opt => opt.MapFrom
        //    (src => $"{src.Address}{(string.IsNullOrEmpty(src.Country) ? string.Empty : ", ")}{src.Country}"));
    }
}
