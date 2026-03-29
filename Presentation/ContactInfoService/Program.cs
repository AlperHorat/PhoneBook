using ContactInfoService.Messaging;
using ContactInfoService.Middlewares;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Infrastructure.Data;
using PhoneBook.Infrastructure.Repositories;
using PhoneBook.Services.ContactInfos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContactInfoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IContactInfoRepository, ContactInfoRepository>();
builder.Services.AddScoped<IContactInfoService, ContactInfoManager>();
builder.Services.AddHostedService<ContactDeletedConsumer>();

builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ContactInfoDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();