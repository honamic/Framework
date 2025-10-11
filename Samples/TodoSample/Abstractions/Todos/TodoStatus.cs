using System.ComponentModel.DataAnnotations;

namespace TodoSample.Todos;

public enum TodoStatus
{
    [Display(Name = "در انتظار")]
    Pending = 1,

    [Display(Name = "درحال انجام")]
    InProgress = 2,

    [Display(Name = "تکمیل شده")]
    Completed = 3,

    [Display(Name = "لغو شده")]
    Cancelled = 4
}