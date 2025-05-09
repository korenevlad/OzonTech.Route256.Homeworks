using System.Collections.Generic;
using System.Linq;
using AutoBogus;
using Bogus;
using HomeworkApp.Dal.Entities;
using HomeworkApp.IntegrationTests.Creators;

namespace HomeworkApp.IntegrationTests.Fakers;

public static class TaskEntityV1Faker
{
    private static readonly object Lock = new();

    private static readonly Faker<TaskEntityV1> Faker = new AutoFaker<TaskEntityV1>()
        .RuleFor(x => x.Id, _ => Create.RandomId())
        .RuleFor(x => x.ParentTaskId, _ => default)
        .RuleFor(x => x.Status, f => f.Random.Int(1, 5))
        .RuleFor(x => x.CreatedAt, f => f.Date.RecentOffset().UtcDateTime)
        .RuleFor(x => x.CompletedAt, f => f.Date.RecentOffset().UtcDateTime)
        .RuleForType(typeof(long), f => f.Random.Long(0L));

    public static TaskEntityV1[] Generate(int count = 1, int? status = null)
    {
        lock (Lock)
        {
            return status.HasValue
                ? Faker.RuleFor(x => x.Status, status.Value).Generate(count).ToArray()
                : Faker.Generate(count).ToArray();
        }
    }
    
    public static List<TaskEntityV1> GenerateTaskHierarchy(int depth, List<TaskEntityV1> tasks, long? parentTaskId = null)
    {
        if (depth <= 0)
            return tasks;
        var task = TaskEntityV1Faker.Generate().First()
            .WithParentTaskId(parentTaskId ?? default)
            .WithId(Create.RandomId());
        tasks.Add(task);
        var childTasks = GenerateTaskHierarchy(depth - 1,tasks, task.Id);
        return tasks;
    }
    
    public static TaskEntityV1 WithCreatedByUserId(
        this TaskEntityV1 src, 
        long userId)
        => src with { CreatedByUserId = userId };
    
    public static TaskEntityV1 WithId(
        this TaskEntityV1 src, 
        long id)
        => src with { Id = id };
    
    public static TaskEntityV1 WithAssignedToUserId(
        this TaskEntityV1 src, 
        long assignedToUserId)
        => src with { AssignedToUserId = assignedToUserId };

    public static TaskEntityV1 WithParentTaskId(
        this TaskEntityV1 src,
        long parentTaskId)
        => src with { ParentTaskId = parentTaskId };
}