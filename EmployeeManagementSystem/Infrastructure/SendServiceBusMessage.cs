
using Azure.Messaging.ServiceBus;
using EmployeeManagementSystem.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure
{
    public class SendServiceBusMessage
    {
        private readonly ILogger _logger;
        //xonnection string to your service bus namespace
        public IConfiguration _configuration;
        //the client that owns the connection and can be used to create senders and receivers
        public ServiceBusClient _client;
        //the sender used to publish message to the topic
        public ServiceBusSender _clientSender;

        public SendServiceBusMessage(IConfiguration _configuration,
            ILogger<SendServiceBusMessage> logger)
        {
            _logger = logger;
            var _serviceBusConnectionString = _configuration["ServiceBusConnectionString"];
            string _queueName = _configuration["ServiceBusQueueName"];
            _client = new ServiceBusClient(_serviceBusConnectionString);
            _clientSender = _client.CreateSender(_queueName);
        }
        public async Task sendServiceBusMessage(ServiceBusMessageData Message)
        {
            var messagePayload = JsonSerializer.Serialize(Message);
            ServiceBusMessage ServiceBusMessageData = new ServiceBusMessage(messagePayload);
            try
            {
                await _clientSender.SendMessageAsync(ServiceBusMessageData);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

       
    }
}
