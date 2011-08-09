using System;
using System.Linq;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;

namespace F.T.Windsor.Tests
{
    [TestFixture]
    public class TestProperDisposal
    {
        public static bool Disposed;

        [SetUp]
        public void SetUp()
        {
            Disposed = false;
        }

        [Test]
        public void HandlerSelectorsAreProperlyDisposed()
        {
            var container = new WindsorContainer().AddFacility<FtwFacility>();

            container.Register(Component.For<ISelectHandlerFor<string>>().ImplementedBy<DisposableHandlerSelector>().LifeStyle.Transient,
                               Component.For<string>().Instance("hello world!"));

            var someString = container.Resolve<string>();
            container.Release(someString);

            Assert.IsTrue(Disposed);
        }

        public class DisposableHandlerSelector : ISelectHandlerFor<string>, IDisposable
        {
            public IHandler Select(IHandler[] handlers)
            {
                return handlers.First();
            }

            public void Dispose()
            {
                Disposed = true;
            }
        }

        [Test]
        public void HandlerFiltersAreProperlyDisposed()
        {
            var container = new WindsorContainer().AddFacility<FtwFacility>();

            container.Register(Component.For<IFilterHandlersFor<string>>().ImplementedBy<DisposableHandlerFilter>().LifeStyle.Transient,
                               Component.For<string>().Instance("hello world!"));

            var strings = container.ResolveAll<string>();

            foreach (var str in strings)
            {
                container.Release(str);
            }

            Assert.IsTrue(Disposed);
        }

        public class DisposableHandlerFilter : IFilterHandlersFor<string>, IDisposable
        {
            public IHandler[] Select(IHandler[] handlers)
            {
                return handlers;
            }

            public void Dispose()
            {
                Disposed = true;
            }
        }
    }
}