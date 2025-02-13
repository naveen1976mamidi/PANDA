using PANDA.Services;
using PatientAPI.Services;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPatientService, PatientService>();
builder.Services.AddSingleton<IAppointmentService, AppointmentService>();

builder.Services.AddSingleton<PatientService>(); // Singleton service
builder.Services.AddScoped<AppointmentService>(); // Ensure AppointmentService is registered
builder.Services.AddHostedService<ShutdownService>(); // Hosted service


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
IServiceCollection serviceCollection = new ServiceCollection();

	
app.UseExceptionHandler(errorApp =>
{
	errorApp.Run(async context =>
	{
		context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		context.Response.ContentType = "application/json";

		var error = new { message = "An unexpected error occurred." };
		await context.Response.WriteAsJsonAsync(error);
	});
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
