using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq; // Adicione este using para o .FirstOrDefault()
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers
{
    [ApiController]
    // BOA PRÁTICA: Defina uma rota base para seu controller.
    // Isso organiza sua API. Agora todos os endpoints começarão com "/v1/todos".
    [Route("v1/todos")]
    public class HomeController : ControllerBase
    {
        // 1. Obter todos os Todos
        // Rota: GET /v1/todos
        [HttpGet]
        public ActionResult<List<TodoModel>> Get([FromServices] AppDbContext context)
        {
            var todos = context.Todos.ToList();
            return Ok(todos);
        }

        // 2. Obter um Todo por ID
        // Rota: GET /v1/todos/{id}
        [HttpGet("{id:int}")] // Anexa "{id:int}" à rota base "/v1/todos"
        public ActionResult<TodoModel> GetById(
            [FromRoute] int id,
            [FromServices] AppDbContext context)
        {
            var todo = context.Todos.FirstOrDefault(x => x.Id == id); // Usar FirstOrDefault é mais seguro

            if (todo == null)
                return NotFound(); // Retorna 404 Not Found se não encontrar

            return Ok(todo);
        }

        // 3. Criar um novo Todo
        // Rota: POST /v1/todos
        [HttpPost]
        public ActionResult<TodoModel> Post(
            [FromBody] TodoModel todo, // [FromBody] é explícito e mais claro
            [FromServices] AppDbContext context)
        {
            context.Todos.Add(todo);
            context.SaveChanges();

            // BOA PRÁTICA: Retorna um status 201 Created com a localização do novo recurso
            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }

        // 4. Atualizar um Todo
        // Rota: PUT /v1/todos/{id}
        [HttpPut("{id:int}")] // Anexa "{id:int}" à rota base "/v1/todos"
        public ActionResult<TodoModel> Put(
            [FromRoute] int id,
            [FromBody] TodoModel input,
            [FromServices] AppDbContext context)
        {
            var model = context.Todos.FirstOrDefault(x => x.Id == id);

            if (model == null)
                return NotFound();

            // Atualiza apenas os campos permitidos
            model.Title = input.Title;
            model.Done = input.Done;

            context.Todos.Update(model);
            context.SaveChanges();

            return Ok(model);
        }

        // 5. Deletar um Todo (Action que estava faltando)
        // Rota: DELETE /v1/todos/{id}
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

            // BOA PRÁTICA: Retorna status 204 No Content para deleção bem-sucedida
            return NoContent();
        }
    }
}