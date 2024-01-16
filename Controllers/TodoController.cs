using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo_AppBE.Models;

namespace Todo_AppBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _todoContext;

        public TodoController(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            if(_todoContext.Todos == null)
            {
                return NotFound();
            }
            return await _todoContext.Todos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            if (_todoContext.Todos == null)
            {
                return NotFound();
            }

            var todo = await _todoContext.Todos.FindAsync(id);
            if(todo == null) 
            { 
                return NotFound();
            }
            return todo;
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> AddTodo(Todo todo)
        {
            _todoContext.Todos.Add(todo);
            await _todoContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodo), new {id = todo.ID}, todo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTodo(int id, Todo todo)
        {
            if(id != todo.ID)
            {
                return BadRequest();
            }

            _todoContext.Entry(todo).State = EntityState.Modified;
            try
            {
                await _todoContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok();  //status code = 200
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            if(_todoContext.Todos == null)
            {
                return NotFound();
            }

            var todo = await _todoContext.Todos.FindAsync(id);
            if(todo == null)
            {
                return NotFound();
            }

            _todoContext.Todos.Remove(todo);
            await _todoContext.SaveChangesAsync();  
            return Ok();
        }
    }
}
