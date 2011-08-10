namespace F.T.Windsor.Tests.Services
{
    public interface ISmsGateway
    {
        void SendSms(string phoneNumber, string message);
    }
}