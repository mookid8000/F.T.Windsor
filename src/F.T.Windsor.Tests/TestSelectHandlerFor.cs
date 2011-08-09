using System.Linq;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;

namespace F.T.Windsor.Tests
{
    [TestFixture]
    public class TestSelectHandlerFor
    {
        [Test]
        public void CanResolveStuffWithHandlerSelector()
        {
            var container = new WindsorContainer();

            container.AddFacility<FtwFacility>()
                .Register(Component.For<ISelectHandlerFor<ISomeService>>().ImplementedBy<SomeServiceSelector>(),
                          Component.For<ISomeService>().ImplementedBy<FirstImpl>(),
                          Component.For<ISomeService>().ImplementedBy<SecondImpl>());

            var someService = container.Resolve<ISomeService>();
            Assert.That(someService.GetType(), Is.EqualTo(typeof(SecondImpl)));
        }
    }

    interface ISomeService { }
    class FirstImpl : ISomeService { }
    class SecondImpl : ISomeService { }

    class SomeServiceSelector : ISelectHandlerFor<ISomeService>
    {
        public IHandler Select(IHandler[] handlers)
        {
            return handlers.Single(h => h.ComponentModel.Implementation == typeof(SecondImpl));
        }
    }
}
