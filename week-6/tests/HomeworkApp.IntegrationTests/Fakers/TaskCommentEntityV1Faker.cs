using System;
using AutoBogus;
using Bogus;
using HomeworkApp.Dal.Entities;
using HomeworkApp.IntegrationTests.Creators;

namespace HomeworkApp.IntegrationTests.Fakers;
public static class TaskCommentEntityV1Faker
{
    private static readonly object Lock = new();

    private static readonly Faker<TaskCommentEntityV1> Faker = new AutoFaker<TaskCommentEntityV1>()
        .RuleFor(x => x.Id, _ => Create.RandomId())
        .RuleFor(x => x.TaskId, f => f.Random.Int(1, 100)) 
        .RuleFor(x => x.AuthorUserId, f => f.Random.Int(1, 50))
        .RuleFor(x => x.Message, f => f.Lorem.Sentence())
        .RuleFor(x => x.At, f => f.Date.RecentOffset().UtcDateTime)
        .RuleFor(x => x.ModifiedAt,_ => null)
        .RuleFor(x => x.DeletedAt, _ => null);

    public static TaskCommentEntityV1[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Faker.Generate(count).ToArray();
        }
    }
    
    public static TaskCommentEntityV1 WithId(
        this TaskCommentEntityV1 src, 
        long id)
        => src with { Id = id };
    
    public static TaskCommentEntityV1 WithModifiedAt(
        this TaskCommentEntityV1 src)
        => src with { ModifiedAt = DateTimeOffset.UtcNow };
    
    public static TaskCommentEntityV1 WithRandomExistedDeletedAt(
        this TaskCommentEntityV1 src)
    {
        var random = new Random();
        DateTimeOffset? randomDeletedAt = random.Next(2) == 0 
            ? (DateTimeOffset?)null 
            : DateTimeOffset.UtcNow;
        return src with { DeletedAt = randomDeletedAt };
    }
    
    public static TaskCommentEntityV1 WithTaskId(
        this TaskCommentEntityV1 src, 
        long taskId)
        => src with { TaskId = taskId };
    


}