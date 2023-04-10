namespace app.web.view.ClientBot
{
    public interface IReceiverService
    {
         Task ReceiveAsync(CancellationToken stoppingToken);
    }
}