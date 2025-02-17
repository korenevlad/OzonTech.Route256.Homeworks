using Microsoft.Extensions.DependencyInjection;
using OrderReportCreator.Application;
using OrderReportCreator.Application.Repositories;
using OrderReportCreator.Application.Senders;
using OrderReportCreator.Domain.Models.OrderAggregate;
using OrderReportCreator.Presentation;
using OrderReportCreator.Services;

var serviceProvider = new ServiceCollection()
    .AddScoped<IOrderRepository, OrderRepositoryCsv>()
    .AddScoped<IOrderReportService, OrderReportService>()
    .AddScoped<IReportSenderFactory, ReportSenderFactory>()
    .AddScoped<IReportSender, ConsoleReportSender>()
    .AddScoped<IReportSender, FileReportSender>()
    .AddScoped<IUI, UI>()
    .AddScoped<IOrderReportManager, OrderReportManager>()
    .BuildServiceProvider();
var orderReportManager = serviceProvider.GetRequiredService<IOrderReportManager>();
orderReportManager.GenerateOrderReport();