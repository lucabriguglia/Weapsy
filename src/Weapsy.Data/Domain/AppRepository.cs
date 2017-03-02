using System;
using System.Linq;
using AutoMapper;
using Weapsy.Domain.Apps;
using AppDbEntity = Weapsy.Data.Entities.App;

namespace Weapsy.Data.Domain
{
    public class AppRepository : IAppRepository
    {
        private readonly IContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public AppRepository(IContextFactory dbContextFactor, IMapper mapper)
        {
            _dbContextFactory = dbContextFactor;
            _mapper = mapper;
        }

        public App GetById(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Apps
                .FirstOrDefault(x => x.Id == id && x.Status != AppStatus.Deleted);
                return dbEntity != null ? _mapper.Map<App>(dbEntity) : null;
            }
        }

        public App GetByName(string name)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Apps
                    .FirstOrDefault(x => x.Name == name && x.Status != AppStatus.Deleted);
                return dbEntity != null ? _mapper.Map<App>(dbEntity) : null;
            }
        }

        public App GetByFolder(string folder)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Apps
                    .FirstOrDefault(x => x.Folder == folder && x.Status != AppStatus.Deleted);
                return dbEntity != null ? _mapper.Map<App>(dbEntity) : null;
            }
        }

        public void Create(App app)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<AppDbEntity>(app);
                context.Apps.Add(dbEntity);
                context.SaveChanges();
            }
        }

        public void Update(App app)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Apps.FirstOrDefault(x => x.Id.Equals(app.Id));
                dbEntity = _mapper.Map(app, dbEntity);
                context.SaveChanges();
            }
        }
    }
}
