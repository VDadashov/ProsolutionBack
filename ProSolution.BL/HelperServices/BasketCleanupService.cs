using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProSolution.Core.Repositories;
using ProSolution.DAL.Contexts;

public class BasketCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public BasketCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Gözləmə müddəti: 3 gün
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);

            using (var scope = _serviceProvider.CreateScope())
            {
                var basketRepository = scope.ServiceProvider.GetRequiredService<IBasketRepository>();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // 3 gündən əvvəl yaradılan və təsdiqlənməmiş basketlər
                var threshold = DateTime.Now.AddDays(-3);

                var expiredBaskets = await basketRepository
                    .GetAllWhere(b => b.CreatedAt < threshold && !b.IsVerified)
                    .ToListAsync(stoppingToken);

                if (expiredBaskets.Any())
                {
                    foreach (var basket in expiredBaskets)
                    {
                        basketRepository.Delete(basket);
                    }

                    await context.SaveChangesAsync(stoppingToken);
                }
            }
        }
    }
}
