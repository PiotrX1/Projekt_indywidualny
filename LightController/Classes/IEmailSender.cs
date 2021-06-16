namespace LightController.Classes
{
    public interface IEmailSender
    {
        void SendMessage(string recipient, string title, string body);
    }
}