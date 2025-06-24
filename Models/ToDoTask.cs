using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models;
public class ToDoTask{
public int Id {get; set;}

[Required]
[StringLength (20,MinimumLength =1)]
public string TaskName {get; set;} = string.Empty;

    public bool IsDone { get; set; } = false;
}