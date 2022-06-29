using ApiUseMongoDB.Models;
using ApiUseMongoDB.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<BookStoreDatabaseSettings>(builder.Configuration.GetSection("BookStoreDatabase")); // 依赖注入 BookStoreDatabaseSettings
builder.Services.AddSingleton<BooksService>(); // 通过依赖注入注册，以支持构造函数注入
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null); // 配置 JSON 序列化选项
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// 添加额外信息
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Book Store API",
        Description = " 通过 MongoDB 资料库",
        TermsOfService = new Uri("https://example.com/terms"), // 服务条款
        Contact = new OpenApiContact
        {
            Name = "联络方式示例",
            Url = new Uri("https://example.com/contact"),
        },
        License = new OpenApiLicense
        {
            Name = "许可证示例",
            Url = new Uri("https://example.com/license"),
        },
    });

    // 添加 XML 注释，针对方法名称
    string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
}); // 添加 Swagger 中间件

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = String.Empty; // 从根路径提供服务
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
