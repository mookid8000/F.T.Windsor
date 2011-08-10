using System;
using System.Collections.Concurrent;
using System.Linq;
using Castle.MicroKernel;

namespace F.T.Windsor
{
    public class Ftw : IHandlerSelector, IHandlersFilter
    {
        readonly IKernel kernel;
        readonly ConcurrentBag<Type> selectorTypesToLookFor = new ConcurrentBag<Type>();
        readonly ConcurrentBag<Type> filterTypesToLookFor = new ConcurrentBag<Type>();

        public Ftw(IKernel kernel)
        {
            this.kernel = kernel;
        }

        /// <summary>
        /// Implemented for handler selector
        /// </summary>
        public bool HasOpinionAbout(string key, Type service)
        {
            return selectorTypesToLookFor.Contains(service);
        }

        public IHandler SelectHandler(string key, Type service, IHandler[] handlers)
        {
            var selectors = (ISelectHandlerFor[])kernel.ResolveAll(typeof(ISelectHandlerFor<>).MakeGenericType(service));

            using (new Releaser(kernel, selectors))
            {
                return selectors.Select(selector => selector.Select(handlers)).FirstOrDefault(result => result != null);
            }
        }

        /// <summary>
        /// Implemented for handlers filter
        /// </summary>
        public bool HasOpinionAbout(Type service)
        {
            return filterTypesToLookFor.Contains(service);
        }

        public IHandler[] SelectHandlers(Type service, IHandler[] handlers)
        {
            var selectors = (IFilterHandlersFor[])kernel.ResolveAll(typeof(IFilterHandlersFor<>).MakeGenericType(service));

            using (new Releaser(kernel, selectors))
            {
                return selectors.Select(selector => selector.Select(handlers)).FirstOrDefault(result => result != null);
            }
        }

        public void RegisterSelector(Type selectorType)
        {
            selectorTypesToLookFor.Add(selectorType);
        }

        public void RegisterFilter(Type filterType)
        {
            filterTypesToLookFor.Add(filterType);
        }

        class Releaser : IDisposable
        {
            readonly IKernel kernel;
            readonly object[] objectsToRelease;

            public Releaser(IKernel kernel, object[] objectsToRelease)
            {
                this.kernel = kernel;
                this.objectsToRelease = objectsToRelease;
            }

            public void Dispose()
            {
                foreach(var obj in objectsToRelease)
                {
                    kernel.ReleaseComponent(obj);
                }
            }
        }
    }
}