using System;

namespace F.T.Windsor.Tests.Services.Impl
{
    public abstract class AbstractTask : IProcessingTask
    {
        public void Process(TaskData data)
        {
            data.RegisterTask(GetType().Name);
        }
    }

    public class Task02 : AbstractTask
    {
    }

    public class Task01 : AbstractTask
    {
    }

    public class Task05 : AbstractTask
    {
    }

    public class Task04 : AbstractTask, IDisposable
    {
        readonly IFeedback feedback;

        public Task04(IFeedback feedback)
        {
            this.feedback = feedback;
        }

        public void Dispose()
        {
            feedback.Write("Task04 disposed!");
        }
    }

    public class Task08 : AbstractTask
    {
    }

    public class Task07 : AbstractTask
    {
        public Task07()
        {
            throw new InvalidOperationException("Task07 should have been left out!");
        }
    }

    public class Task06 : AbstractTask
    {
    }

    public class Task03 : AbstractTask
    {
        public Task03()
        {
            throw new InvalidOperationException("Task03 should have been left out!");
        }
    }
}