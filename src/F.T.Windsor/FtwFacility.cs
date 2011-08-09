using System;
using System.Linq;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;

namespace F.T.Windsor
{
    public class FtwFacility : AbstractFacility
    {
        protected override void Init()
        {
            var ftwHandlerSelector = new FtwHandlerSelector(Kernel);
            Kernel.AddHandlerSelector(ftwHandlerSelector);
            Kernel.AddHandlersFilter(ftwHandlerSelector);
            Kernel.ComponentRegistered += (key, handler) => ComponentRegistered(handler, ftwHandlerSelector);
        }

        void ComponentRegistered(IHandler handler, FtwHandlerSelector ftwHandlerSelector)
        {
            var componentModel = handler.ComponentModel;

            RegisterSelectors(ftwHandlerSelector, componentModel);

            RegisterFilters(ftwHandlerSelector, componentModel);
        }

        void RegisterFilters(FtwHandlerSelector ftwHandlerSelector, ComponentModel componentModel)
        {
            var filters = componentModel.Services
                .Where(s => s.IsGenericType && s.GetGenericTypeDefinition() == typeof(IFilterHandlersFor<>))
                .ToList();

            if (!filters.Any()) return;

            Console.WriteLine("whee filters: {0}", string.Join(", ", filters));

            foreach (var filterType in filters)
            {
                ftwHandlerSelector.RegisterFilter(filterType.GetGenericArguments()[0]);
            }
        }

        void RegisterSelectors(FtwHandlerSelector ftwHandlerSelector, ComponentModel componentModel)
        {
            var selectors = componentModel.Services
                .Where(s => s.IsGenericType && s.GetGenericTypeDefinition() == typeof (ISelectHandlerFor<>))
                .ToList();

            if (!selectors.Any()) return;

            Console.WriteLine("whee selectors: {0}", string.Join(", ", selectors));

            foreach (var selectorType in selectors)
            {
                ftwHandlerSelector.RegisterSelector(selectorType.GetGenericArguments()[0]);
            }
        }
    }
}