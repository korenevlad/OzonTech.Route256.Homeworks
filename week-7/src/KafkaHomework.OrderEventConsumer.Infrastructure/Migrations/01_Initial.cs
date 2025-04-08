using System;
using FluentMigrator;

using KafkaHomework.OrderEventConsumer.Infrastructure.Common;

namespace Ozon.Route256.Postgres.Persistence.Migrations;

[Migration(1, "Initial migration")]
public sealed class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) => @"
        create table order_items
(
    order_id   bigint,
    item_id    bigint,
    status     text,
    money      numeric,
    currency   text,
    quantity bigint,
    created_at timestamp with time zone,
    constraint order_items_pk
        unique (order_id, item_id, status)
);

create table inventory
(
    item_id  bigint,
    status   text,
    money      numeric,
    quantity bigint,
    currency text,
    created_at timestamp with time zone,
    constraint inventory_pk
        unique (item_id, status, currency)
);

create table seller_inventory
(
    currency  text,
    quantity  bigint,
    money       numeric,
    seller_id text,
    good_id   text,
    constraint seller_inventory_pk
        unique (seller_id, good_id, currency)
);
";

    protected override string GetDownSql(IServiceProvider services) =>
        """
        drop table if exists order_items;
        drop table if exists inventory;
        drop table if exists seller_inventory;
        """;
}
