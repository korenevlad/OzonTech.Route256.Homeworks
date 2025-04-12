using FluentMigrator;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Migrations;

[Migration(20250327111000, TransactionBehavior.None)]
public class AddModifiedAndDeletedAtToTaskComments : Migration {
    public override void Up()
    {
        const string sql = @"
            alter table task_comments 
            add column modified_at timestamp with time zone null,
            add column deleted_at timestamp with time zone null;
        ";
        
        Execute.Sql(sql);
    }

    public override void Down()
    {
        const string sql = @"
            alter table task_comments 
            drop column IF exists modified_at,
            drop column IF exists deleted_at;
        ";

        Execute.Sql(sql);
    }
}