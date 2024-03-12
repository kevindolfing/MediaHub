using MediaHub.DAL.FS.Services;

string rootPath = Environment.GetEnvironmentVariable("MEDIAHUB_ROOT_PATH") ?? Directory.GetCurrentDirectory() + "/media";


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
AddServices(builder.Services);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
return;

void AddServices(IServiceCollection services)
{
    services.AddTransient<IMediaService, MediaService>(provider => new MediaService(rootPath));
}