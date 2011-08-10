using System.Collections.Generic;

namespace F.T.Windsor.Tests.Services
{
    public interface ICurrentUserSettings
    {
        string PhoneNumber { get; set; }
        string EmailAddress { get; set; }
        bool HasSmartphone { get; set; }
        HashSet<string> TasksToLeaveOut { get; set; }
    }
}