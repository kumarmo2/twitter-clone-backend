namespace UserEventsConsumer
{
    public interface IServer
    {
        IServer InitializeServer();
        void StartListeningEvents();
    }
}