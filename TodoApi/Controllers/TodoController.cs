using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TodoApi.Models;

namespace TodoApi.Controllers
{

  [Route("api/[controller]")]
  [ApiController]
  public class TodoController : ControllerBase
  {
    private readonly TodoContext _context;

    public TodoController(TodoContext context)
    {
      _context = context;

      if (_context.TodoItems.Count() == 0)
      {
        _context.TodoItems.Add(new TodoItem { Name = "a new todo item" });
        _context.SaveChanges();

      }
    }



    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
    {
      return await _context.TodoItems.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
    {
      _context.TodoItems.Add(item);
      await _context.SaveChangesAsync();
      return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTodoItem(long id, TodoItem item)
    {
      if (id != item.Id)
      {
        return BadRequest();
      }

      var todoItem = await _context.TodoItems.FindAsync(id);
      if (todoItem == null)
      {
        return NotFound();
      }

      todoItem.IsComplete = item.IsComplete;
      _context.Update(todoItem);
      await _context.SaveChangesAsync();
      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
      var todoItem = await _context.TodoItems.FindAsync(id);
      if (todoItem == null)
      {
        return NotFound();
      }

      _context.TodoItems.Remove(todoItem);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
    {
      if (id != item.Id)
      {
        return BadRequest();
      }

      _context.Entry(item).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
    {
      var todoItem = await _context.TodoItems.FindAsync(id);
      if (todoItem == null)
      {
        return NotFound();
      }

      return todoItem;

    }

  }

}