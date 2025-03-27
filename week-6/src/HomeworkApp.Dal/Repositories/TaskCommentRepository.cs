using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using HomeworkApp.Dal.Entities;
using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.Dal.Settings;
using Microsoft.Extensions.Options;

namespace HomeworkApp.Dal.Repositories;

public class TaskCommentRepository : PgRepository, ITaskCommentRepository
{
    public TaskCommentRepository( IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
    {
    }

    public async Task<long> Add(TaskCommentEntityV1 model, CancellationToken token)
    {
        const string sqlQuery = @"
insert into task_comments (task_id, author_user_id, message, at, modified_at, deleted_at)
values (@TaskId, @AuthorUserId, @Message, @At, @ModifiedAt, @DeletedAt)
returning id;
";
        await using var connection = await GetConnection();
        var id = await connection.QuerySingleAsync<long>(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    TaskId = model.TaskId,
                    AuthorUserId = model.AuthorUserId,
                    Message = model.Message,
                    At = model.At,
                    ModifiedAt = model.ModifiedAt,
                    DeletedAt = model.DeletedAt
                },
                cancellationToken: token));
        return id;
    }

    public async Task Update(TaskCommentEntityV1 model, CancellationToken token)
    {
        const string sqlQuery = @"
update task_comments
set task_id = @TaskId,
    author_user_id = @AuthorUserId,
    message = @Message,
    modified_at = @ModifiedAt
where id = @Id
";
        await using var connection = await GetConnection();
        var id = await connection.ExecuteAsync(
            new CommandDefinition(
                sqlQuery,
                new
                {
                    Id = model.Id,
                    TaskId = model.TaskId,
                    AuthorUserId =  model.AuthorUserId,
                    Message = model.Message,
                    ModifiedAt = model.ModifiedAt
                },
                cancellationToken: token));
    }

    public Task SetDeleted(long taskCommentId, CancellationToken token)
    {
        throw new System.NotImplementedException();
    }

    public async Task<TaskCommentEntityV1[]> Get(TaskCommentGetModel model, CancellationToken token)
    {
        var sqlQuery = @"
select *
from task_comments tc
where tc.task_id = @TaskId
";
        var @params = new DynamicParameters();
        @params.Add($"TaskId", model.TaskId);
        
        if (!model.IncludeDeleted)
        {
            var condition = $"and tc.deleted_at is null";
            sqlQuery = sqlQuery + condition;
        }

        var cmd = new CommandDefinition(
            sqlQuery + $"\norder by tc.at desc",
            @params,
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: token);
        
        await using var connection = await GetConnection();
        return (await connection.QueryAsync<TaskCommentEntityV1>(cmd)).ToArray();
    }
}