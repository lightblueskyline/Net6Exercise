using ApiUseMongoDB.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiUseMongoDB.Services
{
    public class BooksService
    {
        private readonly IMongoCollection<Book> _booksCollection;

        // 依赖注入
        // 构造函数注入 BookStoreDatabaseSettings
        public BooksService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            // MongoClient 应当在依赖注入中通过单例模式注册
            MongoClient mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);

            _booksCollection = mongoDatabase.GetCollection<Book>(bookStoreDatabaseSettings.Value.BooksCollectionName);
        }


        /// <summary>
        /// 取得所有资料
        /// </summary>
        public async Task<List<Book>> GetAsync() =>
            await _booksCollection.Find(_ => true).ToListAsync();

        /// <summary>
        /// 依据 ID 取得指定资料
        /// </summary>
        public async Task<Book?> GetAsync(string id) =>
            await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        /// <summary>
        /// 创建资料
        /// </summary>
        public async Task CreateAsync(Book newBook) =>
             await _booksCollection.InsertOneAsync(newBook);

        /// <summary>
        /// 更新资料
        /// </summary>
        public async Task UpdateAsync(string id, Book updateBook) =>
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updateBook);

        /// <summary>
        /// 删除资料
        /// </summary>
        public async Task RemoveAsync(string id) =>
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
