using System.Threading;
using System.Threading.Tasks;
using HomeworkApp.Dal.Entities;
using HomeworkApp.Dal.Models;
using TaskStatus = HomeworkApp.Dal.Models.TaskStatus;

namespace HomeworkApp.Dal.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<long[]> Add(TaskEntityV1[] tasks, CancellationToken token);

    Task<TaskEntityV1[]> Get(TaskGetModel query, CancellationToken token);

    Task Assign(AssignTaskModel model, CancellationToken token);
    
    Task<SubTaskModel[]> GetSubTasksInStatus(long parentTaskId, TaskStatus[] statuses, CancellationToken token);
}
