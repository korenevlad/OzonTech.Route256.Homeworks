using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HomeworkApp.Dal.Entities;
using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.IntegrationTests.Creators;
using HomeworkApp.IntegrationTests.Fakers;
using HomeworkApp.IntegrationTests.Fixtures;
using Xunit;
using TaskStatus = HomeworkApp.Dal.Models.TaskStatus;

namespace HomeworkApp.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class TaskRepositoryTests
{
    private readonly ITaskRepository _repository;

    public TaskRepositoryTests(TestFixture fixture)
    {
        _repository = fixture.TaskRepository;
    }

    [Fact]
    public async Task Add_Task_Success()
    {
        // Arrange
        const int count = 5;

        var tasks = TaskEntityV1Faker.Generate(count);

        // Act
        var results = await _repository.Add(tasks, default);

        // Asserts
        results.Should().HaveCount(count);
        results.Should().OnlyContain(x => x > 0);
    }

    [Fact]
    public async Task Get_SingleTask_Success()
    {
        // Arrange
        var tasks = TaskEntityV1Faker.Generate();
        var taskIds = await _repository.Add(tasks, default);
        var expectedTaskId = taskIds.First();
        var expectedTask = tasks.First()
            .WithId(expectedTaskId);

        // Act
        var results = await _repository.Get(new TaskGetModel()
        {
            TaskIds = new[] { expectedTaskId }
        }, default);

        // Asserts
        results.Should().HaveCount(1);
        var task = results.Single();

        task.Should().BeEquivalentTo(expectedTask);
    }

    [Fact]
    public async Task AssignTask_Success()
    {
        // Arrange
        var assigneeUserId = Create.RandomId();

        var tasks = TaskEntityV1Faker.Generate();
        var taskIds = await _repository.Add(tasks, default);
        var expectedTaskId = taskIds.First();
        var expectedTask = tasks.First()
            .WithId(expectedTaskId)
            .WithAssignedToUserId(assigneeUserId);
        var assign = AssignTaskModelFaker.Generate()
            .First()
            .WithTaskId(expectedTaskId)
            .WithAssignToUserId(assigneeUserId);

        // Act
        await _repository.Assign(assign, default);

        // Asserts
        var results = await _repository.Get(new TaskGetModel()
        {
            TaskIds = new[] { expectedTaskId }
        }, default);

        results.Should().HaveCount(1);
        var task = results.Single();

        expectedTask = expectedTask with {Status = assign.Status};
        task.Should().BeEquivalentTo(expectedTask);
    }

    [Fact]
    public async Task GetSubTasksInStatus_Success()
    {
        // Arrange
        const int depth = 5;
        var targetStatuses = new TaskStatus[] { TaskStatus.InProgress, TaskStatus.Done };
        var countTasks = depth;
        long parentTaskId = default;
        var generatedTasksWithoutFirstTask = new List<TaskEntityV1>();
        long firstTaskId = default;
        while (countTasks > 0)
        {
            TaskEntityV1 generatedTask;
            switch (countTasks)
            {
                case depth:
                    generatedTask = TaskEntityV1Faker.Generate().First();
                    var createdTaskId = await _repository.Add(new TaskEntityV1[]{ generatedTask }, default);
                    firstTaskId = createdTaskId[0];
                    parentTaskId = createdTaskId[0];
                    break;
                case var _:
                    generatedTask = TaskEntityV1Faker.Generate().First().WithParentTaskId(parentTaskId);
                    generatedTasksWithoutFirstTask.Add(generatedTask);
                    createdTaskId = await _repository.Add(new TaskEntityV1[]{ generatedTask }, default);
                    parentTaskId = createdTaskId[0];
                    break;
            }
            countTasks--;
        }
        var tasksWithTargetStatuses = generatedTasksWithoutFirstTask
            .Where(t => targetStatuses.Contains((TaskStatus)t.Status)).ToArray();
        
        // Act
        var realTasksInStatuses = await _repository.GetSubTasksInStatus(firstTaskId, targetStatuses, default);
        
        // Asserts
        Assert.Equal(tasksWithTargetStatuses.Length, realTasksInStatuses.Length);
        Assert.All(realTasksInStatuses, task => Assert.Contains((TaskStatus)task.Status, targetStatuses));
    }
}
