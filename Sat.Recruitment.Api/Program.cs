using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using Sat.Recruitment.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions(builder.Configuration);
builder.Services.AddDbContexts(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddSwagger($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default30seconds", new CacheProfile { Duration = 30 });
}).AddNewtonsoftJson();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

////Soporte para CORS
//app.UseCors(builder =>
//{
//    builder.AllowAnyOrigin();
//    builder.AllowAnyMethod();
//    builder.AllowAnyMethod();
//});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();