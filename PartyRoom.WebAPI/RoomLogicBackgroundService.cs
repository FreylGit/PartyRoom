using PartyRoom.Domain.Interfaces.Services;

namespace PartyRoom.WebAPI
{
    public class RoomLogicBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public RoomLogicBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var roomService = scope.ServiceProvider.GetRequiredService<IRoomService>();
                while (!stoppingToken.IsCancellationRequested)
                {
                    await roomService.CheckStartRoomsAsync(stoppingToken);
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }
    }
}
