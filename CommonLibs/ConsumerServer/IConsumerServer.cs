namespace CommonLibs.ConsumerServer.Abstractions
{
    public interface IConsumerServer
    {
        IConsumerServer InitializeServer();
        void StartListeningEvents();
    }
}