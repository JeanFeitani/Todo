using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers
{
    [ApiController]

    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public List<TodoModel> Get([FromServices] AppDbContext context)
        {
            return [.. context.Todos];
        }
        [HttpGet("/{id:int}")]
        public ActionResult<TodoModel> GetById([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var todo = context.Todos.Find(id);

            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        [HttpPost("/")]
        public TodoModel Post(TodoModel todo, [FromServices] AppDbContext context)
        {
            context.Todos.Add(todo);
            context.SaveChanges();

            return todo;
        }
        [HttpPut("/{id:int}")]
        public ActionResult<TodoModel> Put(
     [FromRoute] int id,
     [FromBody] TodoModel input,
     [FromServices] AppDbContext context)
        {
            var model = context.Todos.Find(id);


            if (model == null)
                return NotFound();

            model.Done = input.Done;

            context.Todos.Update(model);
            context.SaveChanges();

            return Ok(model);
        }

    }
}