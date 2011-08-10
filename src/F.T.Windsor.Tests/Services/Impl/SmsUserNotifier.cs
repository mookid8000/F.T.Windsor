namespace F.T.Windsor.Tests.Services.Impl
{
    public class SmsUserNotifier : IUserNotifier
    {
        readonly ICurrentUserSettings currentUserSettings;
        readonly IFeedback feedback;

        public SmsUserNotifier(ICurrentUserSettings currentUserSettings, IFeedback feedback)
        {
            this.currentUserSettings = currentUserSettings;
            this.feedback = feedback;
        }

        public void NotifyUser(string message)
        {
            feedback.Write(string.Format("SMS to {0}: {1}", currentUserSettings.PhoneNumber, message));
        }
    }
}