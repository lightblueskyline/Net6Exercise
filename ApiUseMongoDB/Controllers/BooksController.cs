using ApiUseMongoDB.Models;
using ApiUseMongoDB.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiUseMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _bookService;

        public BooksController(BooksService booksService) =>
            _bookService = booksService;

        /// <summary>
        /// 取得所有资料
        /// </summary>
        [HttpGet]
        public async Task<List<Book>> Get() =>
            await _bookService.GetAsync();

        /// <summary>
        /// 依据 ID 取得指定资料
        /// </summary>
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _bookService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            return book;
        }

        /// <summary>
        /// 创建资料
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post(Book newBook)
        {
            await _bookService.CreateAsync(newBook);

            // 返回 201 响应状态码
            return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
        }

        /// <summary>
        /// 更新资料
        /// </summary>
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Book updateBook)
        {
            var book = await _bookService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            updateBook.Id = book.Id;

            await _bookService.UpdateAsync(id, updateBook);

            return NoContent();
        }

        /// <summary>
        /// 删除资料
        /// </summary>
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _bookService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _bookService.RemoveAsync(id);

            return NoContent();
        }
    }
}
