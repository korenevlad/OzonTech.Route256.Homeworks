using HomeworkApp.Dal.Entities;

namespace HomeworkApp.Bll.Models.Mappers;
public static class TaskCommentMapper
{
    public static TaskMessage ToTaskMessage(this TaskCommentEntityV1 entity)
    {
        return new TaskMessage
        {
            TaskId = entity.TaskId,
            Comment = entity.Message,
            IsDeleted = entity.DeletedAt.HasValue,
            At = entity.At
        };
    }
}
