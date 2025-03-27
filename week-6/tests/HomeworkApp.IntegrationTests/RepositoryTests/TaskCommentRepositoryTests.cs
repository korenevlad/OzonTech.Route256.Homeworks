using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HomeworkApp.Dal.Entities;
using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.IntegrationTests.Fakers;
using HomeworkApp.IntegrationTests.Fixtures;
using Xunit;

namespace HomeworkApp.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class TaskCommentRepositoryTests
{
    private readonly ITaskCommentRepository _repository;

    public TaskCommentRepositoryTests(TestFixture fixture)
    {
        _repository = fixture.TaskCommentRepository;
    }

    [Fact]
    public async Task Add_TaskComment_Success()
    {
        //Arrange
        var taskComment = TaskCommentEntityV1Faker.Generate();
        
        //Act
        var id = await _repository.Add(taskComment.First(), default);
        
        //Asserts
        id.Should().NotBe(0);
        id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task Get_WithoutDeleted_Success()
    {
        //Arrange
        var countTaskCommentsGroupByTaskId = new int[] { 1, 2, 3, 4, 5 };
        var random = new Random();
        var randomTaskIds = Enumerable.Range(0, 5)
            .Select(_ => random.Next(1, 101)) 
            .ToArray();
        var allTaskComments = new List<TaskCommentEntityV1>();
        for (var i = 0; i < countTaskCommentsGroupByTaskId.Length; i++)
        {
            allTaskComments.AddRange(TaskCommentEntityV1Faker.Generate(countTaskCommentsGroupByTaskId[i])
                .Select(comment => comment.WithTaskId(randomTaskIds[i]).WithRandomExistedDeletedAt()));
        }
        foreach (var taskComment in allTaskComments)
        {
            await _repository.Add(taskComment, default);
        }
        var taskCommentGetModel = new TaskCommentGetModel()
        {
            TaskId = randomTaskIds[random.Next(randomTaskIds.Length)],
            IncludeDeleted = false
        };
        var filteredTaskComments = allTaskComments
            .Where(tc => tc.TaskId == taskCommentGetModel.TaskId && 
                         (taskCommentGetModel.IncludeDeleted || tc.DeletedAt == null))
            .ToList();
        
        //Act
       var taskCommentsWithoutDeleted = await _repository.Get(taskCommentGetModel, default);

        //Asserts
        taskCommentsWithoutDeleted.Should().HaveCount(filteredTaskComments.Count);
        taskCommentsWithoutDeleted.Should().BeEquivalentTo(filteredTaskComments, options => 
            options.Excluding(x => x.Id));
    }
    
    [Fact]
    public async Task Get_IncludeDeleted_Success()
    {
        //Arrange
        var countTaskCommentsGroupByTaskId = new int[] { 1, 2, 3, 4, 5 };
        var random = new Random();
        var randomTaskIds = Enumerable.Range(0, 5)
            .Select(_ => random.Next(1, 101)) 
            .ToArray();
        var allTaskComments = new List<TaskCommentEntityV1>();
        for (var i = 0; i < countTaskCommentsGroupByTaskId.Length; i++)
        {
            allTaskComments.AddRange(TaskCommentEntityV1Faker.Generate(countTaskCommentsGroupByTaskId[i])
                .Select(comment => comment.WithTaskId(randomTaskIds[i]).WithRandomExistedDeletedAt()));
        }
        foreach (var taskComment in allTaskComments)
        {
            await _repository.Add(taskComment, default);
        }
        var taskCommentGetModel = new TaskCommentGetModel()
        {
            TaskId = randomTaskIds[random.Next(randomTaskIds.Length)],
            IncludeDeleted = true
        };
        var filteredTaskComments = allTaskComments
            .Where(tc => tc.TaskId == taskCommentGetModel.TaskId)
            .ToList();
        
        //Act
        var taskCommentsWithoutDeleted = await _repository.Get(taskCommentGetModel, default);

        //Asserts
        taskCommentsWithoutDeleted.Should().BeEquivalentTo(filteredTaskComments, options => 
            options.Excluding(x => x.Id));
    }
    

    [Fact]
    public async Task Update_TaskComment_Success()
    {
        //Arrange
        var taskComment = TaskCommentEntityV1Faker.Generate().First();
        var id = await _repository.Add(taskComment, default);
        var generatedTaskCommentForUpdate = TaskCommentEntityV1Faker.Generate().First().WithId(id).WithModifiedAt();
        
        //Act
        await _repository.Update(generatedTaskCommentForUpdate, default);
        
        var taskCommentGetModel = new TaskCommentGetModel()
        {
            TaskId = generatedTaskCommentForUpdate.TaskId,
            IncludeDeleted = true
        };
       var taskComments =  await _repository.Get(taskCommentGetModel, default);
       var updatedTaskComment = taskComments.Where(x => x.Id == id).First();
        
        //Asserts
        updatedTaskComment.Should().BeEquivalentTo(generatedTaskCommentForUpdate);
    }

    [Fact]
    public async Task SetDeleted_taskCommentId_Sucess()
    {
        //Arrange
        var taskComment = TaskCommentEntityV1Faker.Generate().First();
        var id = await _repository.Add(taskComment, default);
        
        //Act
        _repository.SetDeleted(id, default);
        var taskCommentGetModel = new TaskCommentGetModel()
        {
            TaskId = taskComment.TaskId,
            IncludeDeleted = true
        };
        var taskComments =  await _repository.Get(taskCommentGetModel, default);
        var taskCommentWithSetDeleted = taskComments.Where(x => x.Id == id).First();
        
        //Asserts
        taskCommentWithSetDeleted.Id.Should().Be(id);
        taskCommentWithSetDeleted.DeletedAt.Should().NotBeNull();
    }
}