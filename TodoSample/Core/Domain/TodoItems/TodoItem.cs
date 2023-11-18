using Honamic.Framework.Domain;
using Honamic.Todo.Domain.Messages;
using System.ComponentModel.DataAnnotations;

namespace Honamic.Todo.Domain.TodoItems;

public class TodoItem : AggregateRoot<long>
{
    // public long UserId { get; set; }

    protected TodoItem()
    {
        
    }

    public TodoItem(long id, string title, string content, List<string> tags)
    {
        Id = id;
        Title = title;
        Content = content;
        Tags = tags;

        RaiseEvent(new TodoItemCreatedEvent(Id));
    }

    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Title { get; set; }

    [MaxLength(200)]
    [MinLength(15)]
    [DataType(DataType.MultilineText)]
    public string Content { get; set; }

    public bool Done { get; set; }

    public List<string> Tags { get; set; }
}