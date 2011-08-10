using System.Collections.Generic;

namespace F.T.Windsor.Tests.Services.Impl
{
    class FeedbackCollector : IFeedback
    {
        readonly List<string>messages = new List<string>();

        public void Write(string message)
        {
            messages.Add(message);
        }

        public IEnumerable<string> Messages
        {
            get { return messages; }
        }
    }
}