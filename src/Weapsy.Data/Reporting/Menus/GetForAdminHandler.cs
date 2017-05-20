using System;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.Menus.Queries;

namespace Weapsy.Data.Reporting.Menus
{
    public class GetForAdminHandler : IQueryHandlerAsync<GetForAdmin, MenuAdminModel>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetForAdminHandler(IContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<MenuAdminModel> RetrieveAsync(GetForAdmin query)
        {
            throw new NotImplementedException();
        }
    }
}
