namespace AppBot.Abstract
{
    public interface IReceiverService
    {
         Task ReceiveAsync(CancellationToken stoppingToken);
    }
}