using System.Reflection;

namespace Honamic.Todo.Facade;

public class ProjectInfo
{
    public static Assembly Assembly => typeof(ProjectInfo).Assembly;
}