using Microsoft.Extensions.DependencyInjection;
using PatientAPI.Services;

namespace PANDA.Services
{

	public class ShutdownService : IHostedService
	{
		private readonly ILogger<ShutdownService> _logger;
		private readonly IHostApplicationLifetime _hostApplicationLifetime;
		private readonly IServiceScopeFactory _serviceScopeFactory;
		public ShutdownService(ILogger<ShutdownService> logger, IHostApplicationLifetime hostApplicationLifetime)
		{
			_logger = logger;
			_hostApplicationLifetime = hostApplicationLifetime;			
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			// Registering to the application stopping event
			_hostApplicationLifetime.ApplicationStopping.Register(OnShutdown);

			_logger.LogInformation("Application is starting.");
			return Task.CompletedTask;
		}

		private void OnShutdown()
		{
			_logger.LogInformation("Application is shutting down... Performing cleanup.");

			
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			// Create a new DI scope
			using var scope = _serviceScopeFactory.CreateScope();
			if (scope != null)
			{
				var appointmentService = scope.ServiceProvider.GetRequiredService<AppointmentService>();
				var patientService = scope.ServiceProvider.GetRequiredService<PatientService>();
				appointmentService.HandleBeforeShutdown();
				patientService.HandleBeforeShutdown();
			}
			_logger.LogInformation("ShutdownService is stopping.");
			return Task.CompletedTask;
		}
	}
}
