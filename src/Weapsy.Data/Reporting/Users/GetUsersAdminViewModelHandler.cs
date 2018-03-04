using System.Collections.Generic;
using AutoMapper;
using System.Threading.Tasks;
using Weapsy.Reporting.Users.Queries;
using Weapsy.Reporting.Users;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Data.Reporting.Users
{
    public class GetUsersAdminViewModelHandler : IQueryHandlerAsync<GetUsersAdminViewModel, UsersAdminViewModel>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public GetUsersAdminViewModelHandler(IContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<UsersAdminViewModel> RetrieveAsync(GetUsersAdminViewModel query)
        {
            using (var context = _contextFactory.Create())
            {
                var totalRecords = context.Users.Count();

                var allRoles = new List<string>(); //await context.Roles.ToListAsync();

                var q = context.Users
                    //.Include(x => x.Roles)
                    .OrderBy(x => x.Email)
                    .Skip(query.StartIndex);

                if (query.NumberOfUsers > 0)
                    q = q.Take(query.NumberOfUsers);

                var users = await q.ToListAsync();
                var list = _mapper.Map<IEnumerable<UserAdminListModel>>(users);

                foreach (var user in list)
                {
                    var userRoleNames = new List<string>();
                    foreach (var roleId in user.Roles)
                    {
                        var userRole = allRoles.FirstOrDefault(x => x.ToString() == roleId);
                        if (userRole != null)
                        {
                            userRoleNames.Add(userRole);
                        }
                    }
                    user.Roles = userRoleNames;
                }

                var viewModel = new UsersAdminViewModel
                {
                    Users = list,
                    TotalRecords = totalRecords,
                    NumberOfPages = (int)Math.Ceiling((double)totalRecords / query.NumberOfUsers)
                };

                return viewModel;
            }
        }
    }
}
