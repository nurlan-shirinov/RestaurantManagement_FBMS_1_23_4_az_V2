using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository.Common;

namespace Application.Services.BackgroundServices;

public class DeleteUserBackgroundService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory=scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var userToDelete = unitOfWork.UserRepository.GetAll().Where(u => u.CreatedDate == null && u.IsDeleted == false).ToList();

                if (userToDelete.Count != 0)
                {
                    foreach (var user in userToDelete)
                    {
                        user.IsDeleted = true;
                        user.DeletedDate = DateTime.Now;
                        user.DeletedBy = 1;
                    }
                    await unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            await Task.Delay(TimeSpan.FromMinutes(0.5), stoppingToken);

        }
    }
}