using System;
using System.Collections.Concurrent;
using System.Linq;
using Castle.MicroKernel;

namespace F.T.Windsor
{
    public class FtwHandlerSelector : IHandlerSelector, IHandlersFilter
    {
        readonly IKernel kernel;
        readonly ConcurrentBag<Type> selectorTypesToLookFor = new ConcurrentBag<Type>();
        readonly ConcurrentBag<Type> filterTypesToLookFor = new ConcurrentBag<Type>();

        public FtwHandlerSelector(IKernel kernel)
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

        /// <summary>
        /// Implemented for handlers filter
        /// </summary>
        public bool HasOpinionAbout(Type service)
        {
            return filterTypesToLookFor.Contains(service);
        }

        public IHandler SelectHandler(string key, Type service, IHandler[] handlers)
        {
            var selectors = (ISelectHandlerFor[])kernel.ResolveAll(typeof(ISelectHandlerFor<>).MakeGenericType(service));

            return selectors.Select(selector => selector.Select(handlers)).FirstOrDefault(result => result != null);
        }

        public IHandler[] SelectHandlers(Type service, IHandler[] handlers)
        {
            var selectors = (IFilterHandlersFor[])kernel.ResolveAll(typeof(IFilterHandlersFor<>).MakeGenericType(service));

            return selectors.Select(selector => selector.Select(handlers)).FirstOrDefault(result => result != null);
        }

        public void RegisterSelector(Type selectorType)
        {
            selectorTypesToLookFor.Add(selectorType);
        }

        public void RegisterFilter(Type filterType)
        {
            filterTypesToLookFor.Add(filterType);
        }
    }
}