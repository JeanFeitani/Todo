using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers
{
    [ApiController]
    [Route("v1/todos")]
    public class HomeController : ControllerBase
    {

        [HttpGet]
        public ActionResult<List<TodoModel>> Get([FromServices] AppDbContext context)
        {
            var todos = context.Todos.ToList();
            return Ok(todos);
        }



        [HttpGet("{id:int}")]
        public ActionResult<TodoModel> GetById(
            [FromRoute] int id,
            [FromServices] AppDbContext context)
        {
            var todo = context.Todos.FirstOrDefault(x => x.Id == id);

            if (todo == null)
                return NotFound();

            return Ok(todo);
        }



        [HttpPost]
        public ActionResult<TodoModel> Post(
            [FromBody] TodoModel todo,
            [FromServices] AppDbContext context)
        {
            context.Todos.Add(todo);
            context.SaveChanges();


            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }



        [HttpPut("{id:int}")]
        public ActionResult<TodoModel> Put(
            [FromRoute] int id,
            [FromBody] TodoModel input,
            [FromServices] AppDbContext context)
        {
            var model = context.Todos.FirstOrDefault(x => x.Id == id);

            if (model == null)
                return NotFound();

            model.Title = input.Title;
            model.Done = input.Done;

            context.Todos.Update(model);
            context.SaveChanges();

            return Ok(model);
        }



        [HttpDelete("{id:int}")]
        public ActionResult Delete(
            [FromRoute] int id,
            [FromServices] AppDbContext context)
        {
            var model = context.Todos.FirstOrDefault(x => x.Id == id);
            if (model == null)
                return NotFound();

            context.Todos.Remove(model);
            context.SaveChanges();

            return NoContent();
        }
    }
}