using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Diagnostics;

namespace ServiceBusExample
{
    public class MessagesHostedService : IHostedService
    {
        private readonly QueueClient _queueClient;

        public MessagesHostedService(IOptions<AppConfig> configuration)
        {
            _queueClient = new QueueClient(
                connectionString: configuration.Value.ServiceBusConnectionString,
                entityPath: configuration.Value.QueueName
            );
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _queueClient.RegisterMessageHandler(
                MessageHandler,
                new MessageHandlerOptions(OnError)
                {
                    AutoComplete = true,
                    MaxConcurrentCalls = 2
                }
            );

            return Task.CompletedTask;
        }

        private Task OnError(ExceptionReceivedEventArgs error)
        {
            Debug.WriteLine("Erro");
            return Task.CompletedTask;
        }

        private async Task MessageHandler(Message msg, CancellationToken token)
        {
            var text = Encoding.Default.GetString(msg.Body);
            Debug.WriteLine("Mensagem recebida: " + text);

            if (text.Equals("erro"))
            {
                throw new Exception("Simulação de erro");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _queueClient.CloseAsync();
        }
    }
}