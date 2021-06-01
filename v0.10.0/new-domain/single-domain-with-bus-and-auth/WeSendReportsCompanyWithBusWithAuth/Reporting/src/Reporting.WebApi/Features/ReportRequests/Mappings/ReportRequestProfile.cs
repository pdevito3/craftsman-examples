namespace Reporting.WebApi.Features.ReportRequests.Mappings
{
    using Reporting.Core.Dtos.ReportRequest;
    using AutoMapper;
    using Reporting.Core.Entities;

    public class ReportRequestProfile : Profile
    {
        public ReportRequestProfile()
        {
            //createmap<to this, from this>
            CreateMap<ReportRequest, ReportRequestDto>()
                .ReverseMap();
            CreateMap<ReportRequestForCreationDto, ReportRequest>();
            CreateMap<ReportRequestForUpdateDto, ReportRequest>()
                .ReverseMap();
        }
    }
}