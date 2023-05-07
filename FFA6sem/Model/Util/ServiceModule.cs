using DAL.Interfaces;
using DAL.Repository;
using Ninject.Modules;

namespace FFA6sem.Model.Util
{
    public class ServiceModule : NinjectModule
    {
        private IConfiguration _configuration;
        public ServiceModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void Load()
        {
            Bind<IDbRepos>().To<DbRepos>().InThreadScope().WithConstructorArgument(_configuration);

        }

    }
}
