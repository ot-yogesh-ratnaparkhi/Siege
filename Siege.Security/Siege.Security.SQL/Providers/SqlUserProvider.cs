using System;
using System.Collections.Generic;
using System.Linq;
using Siege.Repository;
using Siege.Security.Providers;
using Siege.Security.SQL.Repository;

namespace Siege.Security.SQL.Providers
{
    public class SqlUserProvider : SqlProvider<User, Guid?>, IUserProvider
    {
        private readonly IIdentityProvider provider;

        public SqlUserProvider(IIdentityProvider provider, IRepository<SecurityDatabase> repository)  : base(repository)
        {
            this.provider = provider;
        }

        public override User Save(User item)
        {
            if (item.ID == null)
            {
                provider.Create(item.Name, item.Password, null, null, null, item.IsActive, null);
                
                var loadedUser = repository.Query<AspNetUser>(query => query.Where(u => u.Name == item.Name && u.Application == item.Application)).FindFirstOrDefault();
                
                loadedUser.Roles = item.Roles;
                loadedUser.Groups = item.Groups;
                loadedUser.Application = item.Application;

                repository.Save(loadedUser);

                return loadedUser;
            }

            repository.Save(item);
            return item;
        }
        
        public User FindByUserName(string userName)
        {
            return repository.Query<AspNetUser>(q => q.Where(u => u.Name == userName)).FindFirstOrDefault();
        }

        public virtual IList<User> GetForApplication(Application application)
        {
            return repository.Query<User>(query => query.Where(p => p.Application == application)).Find();
        }
    }
}