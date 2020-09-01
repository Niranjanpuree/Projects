using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using Microsoft.Practices.Unity;

namespace AutoCare.Product.Application.RepositoryServices
{
    public class VcdbUnitOfWork : IVcdbUnitOfWork
    {
        private readonly DbContext _vcdbContext;
        private readonly ITextSerializer _serializer;
        private readonly IUnityContainer _container;
        private readonly IDictionary<Type, object> _repositories = null;

        public VcdbUnitOfWork(DbContext vcdbContext, ITextSerializer serializer, IUnityContainer container)
        {
            _vcdbContext = vcdbContext;
            _serializer = serializer;
            _container = container;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepositoryService<T> GetRepositoryService<T>() where T : class
        {
            object repository;
            if (!_repositories.TryGetValue(typeof(T), out repository))
            {
                if (typeof (T).IsInterface)
                {
                    _container.Resolve<T>(new ParameterOverrides
                    {
                        {"context", _vcdbContext},
                        {"serializer", _serializer}
                    });
                }
                else
                {
                    repository = _container.Resolve<IVcdbSqlServerEfRepositoryService<T>>(new ParameterOverrides
                {
                    { "context", _vcdbContext},
                    { "serializer", _serializer }
                });
                }
                
                //repository = new VcdbSqlServerEfRepositoryService<T>(_vcdbContext, _serializer);
                _repositories.Add(typeof(T), repository);
            }


            return (IRepositoryService<T>)repository;
        }

        public T GetRepositoryService<T, TModel>() 
            where T : IRepositoryService<TModel> 
            where TModel : class
        {
            object repository;
            if (!_repositories.TryGetValue(typeof(T), out repository))
            {
                repository = _container.Resolve<T>(new ParameterOverrides
                    {
                        {"context", _vcdbContext},
                        {"serializer", _serializer}
                    });

                _repositories.Add(typeof(T), repository);
            }

            return (T)repository;
        }

        public object GetRepositoryService(Type entityType)
        {
            object repository;
            if (!_repositories.TryGetValue(entityType, out repository))
            {
                repository = _container.Resolve(entityType, new ParameterOverrides
                    {
                        {"context", _vcdbContext},
                        {"serializer", _serializer}
                    });

                //repository = Utility.GetGenericTypeInstance(typeof(VcdbSqlServerEfRepositoryService<>),
                //    new Type[] { entityType }, _vcdbContext, _serializer);

                _repositories.Add(entityType, repository);
            }


            return repository;
        }

        public int SaveChanges()
        {
            return _vcdbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _vcdbContext.SaveChangesAsync();
        }
    }
}
