using System.Collections.Generic;
using AutoMapper;
using Weapsy.Infrastructure.Queries;
using System.Threading.Tasks;
using Weapsy.Reporting.Users.Queries;
using Weapsy.Reporting.Users;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace Weapsy.Data.Reporting.Users
{
    public class GetUsersAdminViewModelHandler : IQueryHandlerAsync<GetUsersAdminViewModel, UsersAdminViewModel>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetUsersAdminViewModelHandler(IDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<UsersAdminViewModel> RetrieveAsync(GetUsersAdminViewModel query)
        {
            using (var context = _contextFactory.Create())
            {
                var totalRecords = context.Users.Count();

                var q = context.Users
                    .OrderBy(x => x.Email)
                    .Skip(query.StartIndex);

                if (query.NumberOfUsers > 0)
                    q = q.Take(query.NumberOfUsers);

                var users = await q.ToListAsync();

                var viewModel = new UsersAdminViewModel
                {
                    Users = _mapper.Map<IList<UserAdminModel>>(users),
                    TotalRecords = totalRecords,
                    NumberOfPages = (int)Math.Ceiling((double)totalRecords / query.NumberOfUsers)
                };

                return viewModel;
            }
        }
    }
}
