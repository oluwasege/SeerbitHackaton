using Microsoft.AspNetCore.Identity;
using SeerbitHackaton.API;
using SeerbitHackaton.Core.DataAccess.EfCore.Context;
using SeerbitHackaton.Core.Entities;
using SeerbitHackaton.Core.Utils;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger("Seerbit Payroll Service");

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddSettingsAndAuthentication(builder.Configuration);
builder.Services.AddServices(builder.Environment,builder.Configuration);

var app = builder.Build();

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
