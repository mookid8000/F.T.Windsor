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
            var ftwHandlerSelector = new Ftw(Kernel);
            Kernel.AddHandlerSelector(ftwHandlerSelector);
            Kernel.AddHandlersFilter(ftwHandlerSelector);
            Kernel.ComponentRegistered += (key, handler) => ComponentRegistered(handler, ftwHandlerSelector);
        }

        void ComponentRegistered(IHandler handler, Ftw ftw)
        {
            var componentModel = handler.ComponentModel;
            RegisterSelectors(ftw, componentModel);
            RegisterFilters(ftw, componentModel);
        }

        void RegisterFilters(Ftw ftw, ComponentModel componentModel)
        {
            var filters = componentModel.Services
                .Where(s => s.IsGenericType && s.GetGenericTypeDefinition() == typeof(IFilterHandlersFor<>))
                .ToList();

            foreach (var filterType in filters)
            {
                ftw.RegisterFilter(filterType.GetGenericArguments()[0]);
            }
        }

        void RegisterSelectors(Ftw ftw, ComponentModel componentModel)
        {
            var selectors = componentModel.Services
                .Where(s => s.IsGenericType && s.GetGenericTypeDefinition() == typeof (ISelectHandlerFor<>))
                .ToList();

            foreach (var selectorType in selectors)
            {
                ftw.RegisterSelector(selectorType.GetGenericArguments()[0]);
            }
        }
    }
}