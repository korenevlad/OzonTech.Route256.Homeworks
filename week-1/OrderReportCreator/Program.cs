using Microsoft.Extensions.DependencyInjection;
using OrderReportCreator.Application.Commands;
using OrderReportCreator.Application.Repositories;
using OrderReportCreator.Domain.Models;
using OrderReportCreator.Presentation;

var serviceProvider = new ServiceCollection()
    .AddScoped<IClientRepository, ClientRepository>()
    .AddScoped<IOrderReportService, OrderReportService>()
    .AddScoped<IUI, UI>()
    
    .BuildServiceProvider();
var orderReportService = serviceProvider.GetRequiredService<IOrderReportService>();
orderReportService.CreateOrderReports();