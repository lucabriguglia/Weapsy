using AutoMapper;
using Weapsy.Apps.Text.Domain;
using TextModuleDbEntity = Weapsy.Apps.Text.Data.SqlServer.Entities.TextModule;
using TextVersionDbEntity = Weapsy.Apps.Text.Data.SqlServer.Entities.TextVersion;
using TextLocalisationDbEntity = Weapsy.Apps.Text.Data.SqlServer.Entities.TextLocalisation;

namespace Weapsy.Apps.Text.Data.SqlServer
{
    public class AutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<TextModule, TextModuleDbEntity>();
            CreateMap<TextModuleDbEntity, TextModule>().ConstructUsing(x => new TextModule());

            CreateMap<TextVersion, TextVersionDbEntity>();
            CreateMap<TextVersionDbEntity, TextVersion>().ConstructUsing(x => new TextVersion());

            CreateMap<TextLocalisation, TextLocalisationDbEntity>();
            CreateMap<TextLocalisationDbEntity, TextLocalisation>().ConstructUsing(x => new TextLocalisation());
        }
    }
}