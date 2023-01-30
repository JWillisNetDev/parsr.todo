using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using parsr.todo.db.Models;

namespace parsr.todo.Dtos.Profiles;

public class TodoProfile : Profile
{
    public TodoProfile()
    {
        #region TodoTask
        CreateMap<TodoTaskCreate, TodoTask>();
        CreateMap<TodoTask, TodoTaskGet>()
            .ReverseMap();
        CreateMap<TodoTaskPut, TodoTask>()
            .ForAllMembers(opt => opt.Condition((_, _, srcMember) => srcMember is not null));
        #endregion
        #region TodoTaskList
        CreateMap<TodoTaskListCreate, TodoTaskList>();
        CreateMap<TodoTaskList, TodoTaskListGet>()
            .ReverseMap();
        CreateMap<TodoTaskListPut, TodoTaskList>()
            .ForAllMembers(opt => opt.Condition((_, _, srcMember) => srcMember is not null));
        #endregion
    }
}
