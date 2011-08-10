using System.Collections.Generic;

namespace F.T.Windsor.Tests.Services
{
    public interface ITaskProcessor
    {
        void DoWork();
    }

    class TaskProcessor : ITaskProcessor
    {
        readonly IEnumerable<IProcessingTask> tasks;
        readonly IUserNotifier notifier;

        public TaskProcessor(IEnumerable<IProcessingTask> tasks, IUserNotifier notifier)
        {
            this.tasks = tasks;
            this.notifier = notifier;
        }

        public void DoWork()
        {
            var taskData = new TaskData();

            foreach(var task in tasks)
            {
                task.Process(taskData);
            }

            notifier.NotifyUser(string.Format("The following tasks have been processed: {0}", string.Join(",", taskData.TouchedBy)));
        }
    }
}