namespace F.T.Windsor.Tests.Services.Impl
{
    public class EmailUserNotifier : IUserNotifier
    {
        readonly ICurrentUserSettings currentUserSettings;
        readonly IFeedback feedback;

        public EmailUserNotifier(ICurrentUserSettings currentUserSettings, IFeedback feedback)
        {
            this.currentUserSettings = currentUserSettings;
            this.feedback = feedback;
        }

        public void NotifyUser(string message)
        {
            feedback.Write(string.Format("Emailing to {0}: {1}", currentUserSettings.EmailAddress, message));
        }
    }
}