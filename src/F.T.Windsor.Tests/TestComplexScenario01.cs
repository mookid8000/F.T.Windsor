using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using F.T.Windsor.Tests.Services;
using F.T.Windsor.Tests.Services.Impl;
using NUnit.Framework;

namespace F.T.Windsor.Tests
{
    [TestFixture]
    public class TestComplexScenario01
    {
        WindsorContainer container;

        [SetUp]
        public void SetUp()
        {
            container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
            container.Kernel.ComponentModelBuilder.AddContributor(new SetDefaultLifestyleToTransient());
            container.AddFacility<FtwFacility>();
        }

        [TearDown]
        public void TearDown()
        {
            container.Dispose();
        }

        [Test]
        public void DoesNotChokeOnComplexShit()
        {
            var feedbackCollector = new FeedbackCollector();

            container.Register(AllTypes.FromThisAssembly().BasedOn<IProcessingTask>().WithService.Base())
                .Register(AllTypes.FromThisAssembly().BasedOn<IUserNotifier>().WithService.Base())
                .Register(Component.For<ITaskProcessor>().ImplementedBy<TaskProcessor>(),
                          Component.For<ICurrentUserSettings>().Instance(new CurrentUserSettings
                                                                             {
                                                                                 EmailAddress = "mookid8000@gmail.com",
                                                                                 HasSmartphone = true,
                                                                                 PhoneNumber = "+45 29 36 70 77",
                                                                                 TasksToLeaveOut =
                                                                                     {
                                                                                         "Task03",
                                                                                         "Task07"
                                                                                     }
                                                                             }),
                          Component.For<ISelectHandlerFor<IUserNotifier>>().ImplementedBy<UserNotifierSelector>(),
                          Component.For<IFilterHandlersFor<IProcessingTask>>().ImplementedBy<ProcessingTaskSorter>(),
                          Component.For<IFeedback>().Instance(feedbackCollector));

            var processor = container.Resolve<ITaskProcessor>();

            processor.DoWork();

            container.Release(processor);

            Assert.That(feedbackCollector.Messages, 
                Contains.Item("Emailing to mookid8000@gmail.com: The following tasks have been processed: Task01,Task02,Task04,Task05,Task06,Task08"));

            Assert.That(feedbackCollector.Messages,
                Contains.Item("Task04 disposed!"));
        }

        class SetDefaultLifestyleToTransient : IContributeComponentModelConstruction
        {
            public void ProcessModel(IKernel kernel, ComponentModel model)
            {
                if (model.LifestyleType == LifestyleType.Undefined)
                {
                    model.LifestyleType = LifestyleType.Transient;
                }
            }
        }
    }
}