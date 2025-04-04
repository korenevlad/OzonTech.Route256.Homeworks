using System;
using FluentMigrator;
using KafkaHomework.OrderEventConsumer.Infrastructure.Common;

namespace Ozon.Route256.Postgres.Persistence.Migrations;
[Migration(2, "Initial item statistics")]
public class InitialItemStatistics : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => @"
create table product_items (
                    item_id bigint primary key, 
                    reserved_count int not null default 0,
                    sold_count int not null default 0,
                    cancelled_count int not null default 0,
                    last_updated timestamp with time zone not null
                );
";
}