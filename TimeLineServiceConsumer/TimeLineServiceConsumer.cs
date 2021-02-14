using System;
using System.Threading;
using CommonLibs.ConsumerServer.Abstractions;
using TimeLineServiceConsumer.Controllers;
using Utils.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using Dtos.Tweets;

namespace TimeLineServiceConsumer
{
    public class TimeLineServiceConsumerServer : IConsumerServer
    {
        private readonly TimeLineServiceController _controller;
        private readonly IRabbitMqClient _rabbitMqClient;
        private static ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        public TimeLineServiceConsumerServer(TimeLineServiceController tweetEventsController, IRabbitMqClient rabbitMqClient)
        {
            _controller = tweetEventsController;
            _rabbitMqClient = rabbitMqClient;
        }
        public IConsumerServer InitializeServer()
        {
            ThreadPool.SetMaxThreads(Environment.ProcessorCount, Environment.ProcessorCount);
            using (var channel = _rabbitMqClient.GetChannel())
            {
                // Create exchange if not created.
                channel.ExchangeDeclare(Utils.Common.Constants.TweetsExchangeName, "fanout", autoDelete: false, durable: true, arguments: null);
            }
            return this;
        }

        public void StartListeningEvents()
        {
            using (var channel = _rabbitMqClient.GetChannel())
            {

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    try
                    {
                        var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var tweetEvent = JsonConvert.DeserializeObject<TweetEvent>(json);
                        await _controller.ProcessEvent(tweetEvent);
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine($"exception: {ex.ToString()}");
                    }
                };

                channel.QueueDeclare(Utils.Common.Constants.TimeLineServiceConsumerQueue, durable: true, exclusive: false, autoDelete: false);

                Console.WriteLine(">>>>>>>>>>>>> Tweets Events Consumer started <<<<<<<<<<<<<<<<<<<<<<");

                channel.BasicConsume(Utils.Common.Constants.TimeLineServiceConsumerQueue, true, consumer: consumer);

                manualResetEvent.WaitOne();
            }
        }
    }
}