using System.Collections.Generic;
using System.Linq;
using Siege.Repository;
using Siege.Security.Providers;
using Siege.Security.SQL.Repository;

namespace Siege.Security.SQL.Providers
{
    public class SqlUserProvider : SqlProvider<User>, IUserProvider
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
                provider.Create(item);

                var loadedUser = repository.Query<User>(query => query.Where(u => u.Name == item.Name && u.Consumer == item.Consumer)).FindFirstOrDefault();
                
                loadedUser.Roles = item.Roles;
                loadedUser.Groups = item.Groups;
                loadedUser.Consumer = item.Consumer;

                repository.Save(loadedUser);

                return loadedUser;
            }

            repository.Save(item);
            return item;
        }
        
        public User FindByUserName(string userName)
        {
            return repository.Query<User>(q => q.Where(u => u.Name == userName)).FindFirstOrDefault();
        }

        public User Find(int? id, bool includeHiddenPermissions)
        {
            var user = repository.Get<User>(id);

            if (!includeHiddenPermissions && user.Permissions.Any(p => p.ExcludeFromAssignment)) return null;

            return user;
        }

        public virtual IList<User> GetForApplicationAndConsumer(Application application, Consumer consumer, bool includeHiddenPermissions)
        {
            var list = repository.Query<User>(query => query.Where(p => p.Consumer.Applications.Contains(application))).Find();

            if (!includeHiddenPermissions && list.Any(r => r.Permissions.Any(p => p.ExcludeFromAssignment)))
            {
                var newList = new List<User>();
                newList.AddRange(list.Where(r => r.Permissions.Any(p => !p.ExcludeFromAssignment)).ToList());

                return newList;
            }

            return list;
        }
    }
}