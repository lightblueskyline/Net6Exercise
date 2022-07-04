using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
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

        #region Action return types

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

        #region IActionResult type

        #region Synchronous action
        [HttpGet("GetProductByID/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // 未找到相关数据
        public IActionResult GetProductByID(int id)
        {
            var listProdcut = GenerateListProduct();

            if (!listProdcut.Any(x => x.ID == id))
            {
                return NotFound(); // 等同于 return new NotFoundResult();
            }

            var product = listProdcut.Select(x => x.ID == id).FirstOrDefault();

            return Ok(product); // 等同于 return new OkObjectResult(product);
        }
        #endregion

        #region Asynchronous action
        //[HttpPost]
        //[Consumes(MediaTypeNames.Application.Json)]
        //[ProducesResponseType(StatusCodes.Status201Created)] // 由 CreatedAtAction 响应
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> CreateProductAsync(Product product)
        //{
        //    if (product.Description.Contains("XYZ Widget"))
        //    {
        //        return BadRequest();
        //    }

        //    await _repository.AddProductAsync(product);

        //    return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        //}
        #endregion

        #endregion

        #region ActionResult<T> type
        /*
         * ActionResult<Product> 两种返回值
         * 404 status code NotFound()
         * 200 status code 同 Product 一起返回
         */
        [HttpGet("GetProductByID1/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> GetProductByID1(int id)
        {
            var listProduct = GenerateListProduct();

            if (!listProduct.Any(x => x.ID == id))
            {
                return NotFound();
            }

            var product = listProduct.Where(x => x.ID == id).Select(x => x);

            return product.First();
        }
        #endregion

        #endregion

        #region JsonPatch in ASP.NET Core web API
        /*
         * 需要安装包 Microsoft.AspNetCore.Mvc.NewtonsoftJson
         * AddNewtonsoftJson() 会替换 System.Text.Json
         */
        #endregion

        #region Format response data in ASP.NET Core Web API
        [HttpGet("Version")]
        public ContentResult GetVersion() => Content("V1.0.0.0");
        #endregion
    }
}
