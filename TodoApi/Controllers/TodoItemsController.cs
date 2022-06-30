using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            // if (_context.TodoItems == null)
            // {
            //     return NotFound();
            // }
            // return await _context.TodoItems.ToListAsync();

            return await _context.TodoItems.Select(x => ItemToDTO(x)).ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            // if (_context.TodoItems == null)
            // {
            //     return NotFound();
            // }
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            // return todoItem;
            return ItemToDTO(todoItem);
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            // _context.Entry(todoItem).State = EntityState.Modified;

            // try
            // {
            //     await _context.SaveChangesAsync();
            // }
            // catch (DbUpdateConcurrencyException)
            // {
            //     if (!TodoItemExists(id))
            //     {
            //         return NotFound();
            //     }
            //     else
            //     {
            //         throw;
            //     }
            // }

            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemDTO todoItemDTO)
        {
            // if (_context.TodoItems == null)
            // {
            //     return Problem("Entity set 'TodoContext.TodoItems'  is null.");
            // }
            // _context.TodoItems.Add(todoItem);
            // await _context.SaveChangesAsync();

            // // return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            // return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);

            var todoItem = new TodoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name,
            };

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, ItemToDTO(todoItem));
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };

        #region Utility
        private List<Product> GenerateListProduct() =>
            new List<Product>
            {
                new Product
                {
                    ID=111,
                    Name="AAA",
                    Price=111M,
                    IsOnSale=true,
                },
                new Product
                {
                    ID=112,
                    Name="BBB",
                    Price=112M,
                    IsOnSale=false,
                },
                new Product
                {
                    ID=113,
                    Name="CCC",
                    Price=113M,
                    IsOnSale=true,
                },
            };
        #endregion

        #region Specific type
        [HttpGet("GetProducts")]
        public List<Product> GetProducts() =>
            GenerateListProduct();
        #endregion

        #region IEnumerable<T> or IAsyncEnumerable<T>
        [HttpGet("syncsale")]
        public IEnumerable<Product> GetOnSaleProducts()
        {
            List<Product> listProduct = GenerateListProduct();

            foreach (var item in listProduct)
            {
                if (item.IsOnSale)
                {
                    yield return item;
                }
            }
        }

        //[HttpGet("asyncsale")]
        //public async IAsyncEnumerable<Product> GetOnSaleProductsAsync()
        //{
        //    List<Product> listProduct = GenerateListProduct();

        //    await foreach (var item in listProduct)
        //    {
        //        if (item.IsOnSale)
        //        {
        //            yield return item;
        //        }
        //    }
        //}
        #endregion
    }
}
