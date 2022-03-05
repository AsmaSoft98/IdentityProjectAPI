using identityProjectAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureCors(); //sym

builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureServices(builder.Configuration); //sym
builder.Services.AddIdentityAuthentication(builder.Configuration); //sym
builder.Services.AddApplicationDatabaseContext(builder.Configuration); //sym
builder.Services.RegisterServices(); //sym

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("CorsPolicy"); //sym

app.UseHttpsRedirection();

app.UseAuthentication(); //sym
app.UseAuthorization();

app.MapControllers();

app.Run();
