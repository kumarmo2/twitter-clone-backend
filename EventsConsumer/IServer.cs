namespace EventsConsumer
{
    public interface IServer
    {
        IServer InitializeServer();
        void StartListeningEvents();
    }
}