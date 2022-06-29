# Create a web API with ASP.NET Core and MongoDB

```bash
# 创建数据库
use BookStore
# 创建数据集合(数据表)
db.createCollection('Books')
#
db.Books.insertMany([{ "Name": "Design Patterns", "Price": 54.93, "Category": "Computers", "Author": "Ralph Johnson" }, { "Name": "Clean Code", "Price": 43.15, "Category": "Computers","Author": "Robert C. Martin" }])
# 查询资料
db.Books.find().pretty()
```
