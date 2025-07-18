﻿using Serilog;
using BookStore.API.Controllers;
using BookStore.API.Data;
using Microsoft.EntityFrameworkCore;
using BookStore.API.Configurration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog((ctx, lc) => {
    lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration);
});
builder.Services.AddDbContext<BookStoreDbContext>(opt=> opt.UseSqlServer (builder.Configuration.GetConnectionString("Default")));
builder.Services.AddAutoMapper(typeof(MappingConfig));
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
