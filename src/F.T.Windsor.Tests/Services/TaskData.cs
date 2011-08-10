using System.Collections.Generic;

namespace F.T.Windsor.Tests.Services
{
    public class TaskData
    {
        readonly List<string>  touchedBy = new List<string>();

        public IEnumerable<string> TouchedBy { get { return touchedBy; }}

        public void RegisterTask(string name)
        {
            touchedBy.Add(name);
        }
    }
}