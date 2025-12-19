using AutoMapper;
using IntaxExterno.Application.DTOs.Parceiro;
using IntaxExterno.Application.DTOs.Cliente;
using IntaxExterno.Application.DTOs.Proposta;
using IntaxExterno.Application.DTOs.Teses;
using IntaxExterno.Application.DTOs.Oportunidade;
using IntaxExterno.Application.DTOs.RelatorioDeCreditoPerse;
using IntaxExterno.Application.DTOs.ExclusaoIcms;
using IntaxExterno.Domain.Entities;
using IntaxExterno.Domain.Enums;

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

        // Oportunidade Mappings
        CreateMap<Oportunidade, OportunidadePostDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.TesesIds, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (StatusOportunidade)src.Status))
            .ForMember(dest => dest.DataInicio, opt => opt.MapFrom(src => src.DataInicio.HasValue ? ConvertToUtc(src.DataInicio.Value) : (DateTime?)null))
            .ForMember(dest => dest.OportunidadeTeses, opt => opt.Ignore())
            .ForMember(dest => dest.Cliente, opt => opt.Ignore())
            .ForMember(dest => dest.Parceiro, opt => opt.Ignore())
            .ForMember(dest => dest.ResultadosExclusaoIcms, opt => opt.Ignore())
            .ForMember(dest => dest.SpedContribuicoes, opt => opt.Ignore())
            .ForMember(dest => dest.SpedFiscais, opt => opt.Ignore());

        CreateMap<Oportunidade, OportunidadeGetDto>()
            .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : string.Empty))
            .ForMember(dest => dest.ParceiroNome, opt => opt.MapFrom(src => src.Parceiro != null ? src.Parceiro.Nome : null))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.StatusDescricao, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.QuantidadeTeses, opt => opt.MapFrom(src => src.OportunidadeTeses != null ? src.OportunidadeTeses.Count : 0));

        CreateMap<Oportunidade, OportunidadeGetDetailsDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.StatusDescricao, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Teses, opt => opt.MapFrom(src => src.OportunidadeTeses.Select(ot => ot.Teses).ToList()));

        CreateMap<Oportunidade, OportunidadePutDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.TesesIds, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (StatusOportunidade)src.Status))
            .ForMember(dest => dest.DataInicio, opt => opt.MapFrom(src => src.DataInicio.HasValue ? ConvertToUtc(src.DataInicio.Value) : (DateTime?)null))
            .ForMember(dest => dest.DataFechamento, opt => opt.MapFrom(src => src.DataFechamento.HasValue ? ConvertToUtc(src.DataFechamento.Value) : (DateTime?)null))
            .ForMember(dest => dest.OportunidadeTeses, opt => opt.Ignore())
            .ForMember(dest => dest.Cliente, opt => opt.Ignore())
            .ForMember(dest => dest.Parceiro, opt => opt.Ignore())
            .ForMember(dest => dest.ResultadosExclusaoIcms, opt => opt.Ignore())
            .ForMember(dest => dest.SpedContribuicoes, opt => opt.Ignore())
            .ForMember(dest => dest.SpedFiscais, opt => opt.Ignore());

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

        // SPED Contribuições Mappings
        CreateMap<SpedContribuicoes, SpedContribuicoesPostDto>()
            .ReverseMap()
            .ForMember(dest => dest.DataInicial, opt => opt.MapFrom(src => src.DataInicial.HasValue ? ConvertToUtc(src.DataInicial.Value) : (DateTime?)null))
            .ForMember(dest => dest.Oportunidade, opt => opt.Ignore());

        CreateMap<SpedContribuicoes, SpedContribuicoesGetDto>();

        CreateMap<SpedContribuicoes, SpedContribuicoesDto>()
            .ReverseMap()
            .ForMember(dest => dest.DataInicial, opt => opt.MapFrom(src => src.DataInicial.HasValue ? ConvertToUtc(src.DataInicial.Value) : (DateTime?)null));

        // SPED Fiscal Mappings
        CreateMap<SpedFiscal, SpedFiscalPostDto>()
            .ReverseMap()
            .ForMember(dest => dest.DataInicial, opt => opt.MapFrom(src => src.DataInicial.HasValue ? ConvertToUtc(src.DataInicial.Value) : (DateTime?)null))
            .ForMember(dest => dest.Oportunidade, opt => opt.Ignore());

        CreateMap<SpedFiscal, SpedFiscalGetDto>();

        CreateMap<SpedFiscal, SpedFiscalDto>()
            .ReverseMap()
            .ForMember(dest => dest.DataInicial, opt => opt.MapFrom(src => src.DataInicial.HasValue ? ConvertToUtc(src.DataInicial.Value) : (DateTime?)null));

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
