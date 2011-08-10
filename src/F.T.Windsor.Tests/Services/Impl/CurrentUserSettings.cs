using System.Collections.Generic;

namespace F.T.Windsor.Tests.Services.Impl
{
    class CurrentUserSettings : ICurrentUserSettings
    {
        public CurrentUserSettings()
        {
            TasksToLeaveOut = new HashSet<string>();
        }

        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool HasSmartphone { get; set; }

        public HashSet<string> TasksToLeaveOut { get; set; }
    }
}