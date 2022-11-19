using Microsoft.Extensions.DependencyInjection;
using SeerbitHackaton.Core.DataAccess.EfCore;
using SeerbitHackaton.Core.DataAccess.Repository;

namespace SeerbitHackaton.Core.Utils
{
    public static class ServiceCollectionExtentions
    {
        public static void RegisterGenericRepos(this IServiceCollection self, Type dbContextType)
        {
            var repositoryInterface = typeof(IRepository<>);
            var repositoryInterfaceWithPrimaryKey = typeof(IRepository<,>);
            var repositoryImplementation = typeof(EfCoreRepository<,>);
            var repositoryImplementationWithPrimaryKey = typeof(EfCoreRepository<,,>);

            foreach (var entityTypeInfo in EfCoreDbContextEntityFinder.GetEntityTypeInfos(dbContextType))
            {
                var primaryKeyType = EntityHelper.GetPrimaryKeyType(entityTypeInfo.EntityType);
                if (primaryKeyType == typeof(int))
                {
                    var genericRepositoryType = repositoryInterface.MakeGenericType(entityTypeInfo.EntityType);
                    //if ( !iocManager.AddIfNotContains<.Contains(genericRepositoryType)) {
                    var implType = repositoryImplementation.GetGenericArguments().Length == 1
                        ? repositoryImplementation.MakeGenericType(entityTypeInfo.EntityType)
                        : repositoryImplementation.MakeGenericType(entityTypeInfo.DeclaringType,
                            entityTypeInfo.EntityType);

                    self.AddTransient(genericRepositoryType, implType);
                    //}
                }
                var genericRepositoryTypeWithPrimaryKey = repositoryInterfaceWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType);
                //if (!ioc.Contains(genericRepositoryTypeWithPrimaryKey)) {
                var implTypeWithKey = repositoryImplementationWithPrimaryKey.GetGenericArguments().Length == 2
                    ? repositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType, primaryKeyType)
                    : repositoryImplementationWithPrimaryKey.MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.EntityType, primaryKeyType);

                self.AddTransient(genericRepositoryTypeWithPrimaryKey, implTypeWithKey);

               
            }

            
        }
    }
}