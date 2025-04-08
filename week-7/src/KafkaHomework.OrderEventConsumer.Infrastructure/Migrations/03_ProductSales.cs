using System;
using FluentMigrator;
using KafkaHomework.OrderEventConsumer.Infrastructure.Common;

namespace Ozon.Route256.Postgres.Persistence.Migrations;
[Migration(3, "Product sales migration")]
public sealed class ProductSales : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => 
        """
        create table product_sales
        (
             seller_id      bigint not null
           , currency       varchar(3) not null
           , total_sales    decimal(18,2) not null default 0.00
           , total_quantity int not null default 0
           , last_updated   timestamp with time zone
           , primary key (seller_id, currency)
        );
        """;
    
    protected override string GetDownSql(IServiceProvider services) =>
        """
        drop table if exists product_sales;
        """;
}
