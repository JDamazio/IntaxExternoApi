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
        CreateMap<ItemRelatorioDeCreditoPerse, ItemRelatorioDeCreditoPerseDto>().ReverseMap()
            .ForMember(dest => dest.DataEmissao, opt => opt.MapFrom(src =>
                ConvertToUtc(src.DataEmissao)));

        // RelatorioDeCreditoPerse Mappings
        CreateMap<RelatorioDeCreditoPerse, RelatorioDeCreditoPersePostDto>()
            .ReverseMap()
            .ForMember(dest => dest.Itens, opt => opt.Ignore())
            .ForMember(dest => dest.DataEmissao, opt => opt.MapFrom(src =>
                ConvertToUtc(src.DataEmissao)));

        CreateMap<RelatorioDeCreditoPerse, RelatorioDeCreditoPerseGetDto>()
            .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente.Nome))
            .ForMember(dest => dest.QuantidadeItens, opt => opt.MapFrom(src => src.Itens != null ? src.Itens.Count : 0));

        CreateMap<RelatorioDeCreditoPerse, RelatorioDeCreditoPerseGetDetailsDto>();

        CreateMap<RelatorioDeCreditoPerse, RelatorioDeCreditoPersePutDto>()
            .ReverseMap()
            .ForMember(dest => dest.Itens, opt => opt.Ignore())
            .ForMember(dest => dest.DataEmissao, opt => opt.MapFrom(src =>
                ConvertToUtc(src.DataEmissao)));
    }

    // Método helper para converter DateTime para UTC corretamente
    // Trata a data como "date-only" (sem timezone), mantendo o mesmo dia/mês/ano
    // Usa 12:00:00 (meio-dia) para evitar que conversões de timezone mudem o dia
    private static DateTime ConvertToUtc(DateTime dateTime)
    {
        // SEMPRE normaliza para meio-dia UTC, independente do Kind
        // Isso garante consistência e evita problemas de timezone
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 0, 0, DateTimeKind.Utc);
    }
}
