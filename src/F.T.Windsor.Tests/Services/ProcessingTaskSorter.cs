using System.Linq;
using Castle.MicroKernel;

namespace F.T.Windsor.Tests.Services
{
    public class ProcessingTaskSorter : IFilterHandlersFor<IProcessingTask>
    {
        readonly ICurrentUserSettings currentUserSettings;

        public ProcessingTaskSorter(ICurrentUserSettings currentUserSettings)
        {
            this.currentUserSettings = currentUserSettings;
        }

        public IHandler[] Select(IHandler[] handlers)
        {
            return handlers
                .Where(CanParseNameAsInt)
                .Where(ShouldBeIncluded)
                .OrderBy(ParseInt).ToArray();
        }

        bool ShouldBeIncluded(IHandler handler)
        {
            return !currentUserSettings.TasksToLeaveOut.Contains(handler.ComponentModel.Implementation.Name);
        }

        bool CanParseNameAsInt(IHandler h)
        {
            var className = h.ComponentModel.Implementation.Name;
            int temp;
            return int.TryParse(GetIntPart(className), out temp); 
        }

        string GetIntPart(string className)
        {
            return className.Length > 4
                       ? className.Substring("task".Length)
                       : className;
        }

        int ParseInt(IHandler h)
        {
            return int.Parse(GetIntPart(h.ComponentModel.Implementation.Name));
        }
    }
}