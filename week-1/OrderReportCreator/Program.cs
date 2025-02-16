using Microsoft.Extensions.DependencyInjection;
using OrderReportCreator.Application;
using OrderReportCreator.Application.Repositories;
using OrderReportCreator.Application.Senders;
using OrderReportCreator.Domain.Models;
using OrderReportCreator.Presentation;
using OrderReportCreator.Services;

var serviceProvider = new ServiceCollection()
    .AddScoped<IOrderRepository, OrderRepositoryCsv>()
    .AddScoped<IReportSender, ConsoleReportSender>()
    .AddScoped<IReportSender, FileReportSender>()
    .AddScoped<IOrderReportService, OrderReportService>()
    .AddScoped<IUI, UI>()
    .AddScoped<IOrderReportManager, OrderReportManager>()
    .BuildServiceProvider();
var orderReportManager = serviceProvider.GetRequiredService<IOrderReportManager>();
orderReportManager.GenerateOrderReport();