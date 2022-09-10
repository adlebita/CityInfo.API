namespace CityInfo.API.Services;

public class CloudMailService : IMailService
{
    public void Send(string subject, string message)
    {
        Console.WriteLine("---Mail from Cloud Server---");
        Console.Write($"Subject: {subject}");
        Console.Write($"Message: {message}");
    }
}