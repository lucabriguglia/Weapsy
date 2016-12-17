using AutoMapper;
using Weapsy.Apps.Text.Domain;
using TextModuleDbEntity = Weapsy.Apps.Text.Data.Entities.TextModule;
using TextVersionDbEntity = Weapsy.Apps.Text.Data.Entities.TextVersion;
using TextLocalisationDbEntity = Weapsy.Apps.Text.Data.Entities.TextLocalisation;

namespace Weapsy.Apps.Text.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
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