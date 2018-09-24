using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace pots.HostedServices
{
    internal class ConsumeScopedServiceHostedService : IHostedService, IDisposable
    {

        private Timer _timer;
        private readonly IServiceProvider _services;
        
        public ConsumeScopedServiceHostedService(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromSeconds(30));//.FromMinutes(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _services.CreateScope())
            {
                var scopedProcessingService = 
                    scope.ServiceProvider
                        .GetRequiredService<HostedNotificationService>();

                await scopedProcessingService.DoWork();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}