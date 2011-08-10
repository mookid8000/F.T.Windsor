using System;

namespace F.T.Windsor.Tests.Services.Impl
{
    public class DummySmsGateway : ISmsGateway
    {
        public void SendSms(string phoneNumber, string message)
        {
            Console.WriteLine("SMS to {0}: {1}", phoneNumber, message);
        }
    }
}