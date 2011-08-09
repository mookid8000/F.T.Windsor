using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;

namespace F.T.Windsor.Tests
{
    [TestFixture]
    public class TestFilterHandlersFor
    {
        [Test]
        public void CanResolveStuffWithHandlerSelector()
        {
            var container = new WindsorContainer();

            container.AddFacility<FtwFacility>()
                .Register(Component.For<IFilterHandlersFor<ISomeService>>().ImplementedBy<SomeServiceFilter>(),
                          Component.For<ISomeService>().ImplementedBy<FirstImpl>(),
                          Component.For<ISomeService>().ImplementedBy<SecondImpl>());

            var services = container.ResolveAll<ISomeService>();
            Assert.That(services[0].GetType(), Is.EqualTo(typeof(SecondImpl)));
            Assert.That(services[1].GetType(), Is.EqualTo(typeof(FirstImpl)));
        }

    }

    public class SomeServiceFilter : IFilterHandlersFor<ISomeService>
    {
        readonly Dictionary<Type, int> typeOrders = new Dictionary<Type, int>
                                                        {
                                                            {typeof (FirstImpl), 2},
                                                            {typeof (SecondImpl), 1}
                                                        };

        public IHandler[] Select(IHandler[] handlers)
        {
            return handlers
                .OrderBy(h => typeOrders[h.ComponentModel.Implementation])
                .ToArray();
        }
    }
}