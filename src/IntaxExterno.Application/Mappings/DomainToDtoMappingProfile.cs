using AutoMapper;
using IntaxExterno.Application.DTOs.Parceiro;
using IntaxExterno.Application.DTOs.Cliente;
using IntaxExterno.Application.DTOs.Proposta;
using IntaxExterno.Application.DTOs.Teses;
using IntaxExterno.Application.DTOs.RelatorioDeCreditoPerse;
using IntaxExterno.Domain.Entities;

namespace IntaxExterno.Application.Mappings;

public class DomainToDtoMappingProfile : Profile
{
    public DomainToDtoMappingProfile()
    {
        // Parceiro Mappings
        CreateMap<Parceiro, ParceiroPostDto>().ReverseMap();
        CreateMap<Parceiro, ParceiroGetDto>().ReverseMap();
        CreateMap<Parceiro, ParceiroGetDetailsDto>().ReverseMap();
        CreateMap<Parceiro, ParceiroPutDto>().ReverseMap();

        // Cliente Mappings
        CreateMap<Cliente, ClientePostDto>().ReverseMap();
        CreateMap<Cliente, ClienteGetDto>().ReverseMap();
        CreateMap<Cliente, ClienteGetDetailsDto>().ReverseMap();
        CreateMap<Cliente, ClientePutDto>().ReverseMap();

        // Teses Mappings
        CreateMap<Teses, TesesPostDto>().ReverseMap();
        CreateMap<Teses, TesesGetDto>().ReverseMap();
        CreateMap<Teses, TesesGetDetailsDto>().ReverseMap();
        CreateMap<Teses, TesesPutDto>().ReverseMap();

        // Proposta Mappings
        CreateMap<Proposta, PropostaPostDto>()
            .ForMember(dest => dest.TesesIds, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.PropostaTeses, opt => opt.Ignore());

        CreateMap<Proposta, PropostaGetDto>()
            .ForMember(dest => dest.ParceiroNome, opt => opt.MapFrom(src => src.Parceiro != null ? src.Parceiro.Nome : null))
            .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente.Nome))
            .ForMember(dest => dest.QuantidadeTeses, opt => opt.MapFrom(src => src.PropostaTeses != null ? src.PropostaTeses.Count : 0));

        CreateMap<Proposta, PropostaGetDetailsDto>()
            .ForMember(dest => dest.Teses, opt => opt.MapFrom(src => src.PropostaTeses.Select(pt => pt.Teses).ToList()));

        CreateMap<Proposta, PropostaPutDto>()
            .ForMember(dest => dest.TesesIds, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.PropostaTeses, opt => opt.Ignore());

        // ItemRelatorioDeCreditoPerse Mappings
        CreateMap<ItemRelatorioDeCreditoPerse, ItemRelatorioDeCreditoPerseDto>().ReverseMap();

        // RelatorioDeCreditoPerse Mappings
        CreateMap<RelatorioDeCreditoPerse, RelatorioDeCreditoPersePostDto>()
            .ReverseMap()
            .ForMember(dest => dest.Itens, opt => opt.Ignore());

        CreateMap<RelatorioDeCreditoPerse, RelatorioDeCreditoPerseGetDto>()
            .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente.Nome))
            .ForMember(dest => dest.QuantidadeItens, opt => opt.MapFrom(src => src.Itens != null ? src.Itens.Count : 0));

        CreateMap<RelatorioDeCreditoPerse, RelatorioDeCreditoPerseGetDetailsDto>();

        CreateMap<RelatorioDeCreditoPerse, RelatorioDeCreditoPersePutDto>()
            .ReverseMap()
            .ForMember(dest => dest.Itens, opt => opt.Ignore());
    }
}
