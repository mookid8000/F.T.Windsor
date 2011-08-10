using System.Linq;
using Castle.MicroKernel;
using F.T.Windsor.Tests.Services.Impl;

namespace F.T.Windsor.Tests.Services
{
    public class UserNotifierSelector : ISelectHandlerFor<IUserNotifier>
    {
        readonly ICurrentUserSettings currentUserSettings;

        public UserNotifierSelector(ICurrentUserSettings currentUserSettings)
        {
            this.currentUserSettings = currentUserSettings;
        }

        public IHandler Select(IHandler[] handlers)
        {
            if (currentUserSettings.HasSmartphone)
            {
                return handlers.First(h => h.ComponentModel.Implementation == typeof (EmailUserNotifier));
            }

            return handlers.First(h => h.ComponentModel.Implementation == typeof (SmsUserNotifier));
        }
    }
}