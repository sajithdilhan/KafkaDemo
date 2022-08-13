using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Kafka.Public;
using Kafka.Public.Loggers;

namespace KafkaDemo
{
    public class KafkaConsumerHost : IHostedService
    {

        private readonly ILogger<KafkaConsumerHost> _logger;
        private readonly ClusterClient _cluster;

        public KafkaConsumerHost(ILogger<KafkaConsumerHost> logger )
        {
            _logger = logger;
            _cluster = new ClusterClient(new Configuration()
            {
                Seeds = "localhost:9092"
            }, new ConsoleLogger());
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Consumer called");
            _cluster.ConsumeFromLatest("demo");
            _cluster.MessageReceived += record =>
            {
                _logger.LogInformation($"Received:{Encoding.UTF8.GetString(record.Value as byte[])}");
            };
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cluster?.Dispose();
            return  Task.CompletedTask;
        }
    }
}
