using ApiUseMongoDB.Models;
using ApiUseMongoDB.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<BookStoreDatabaseSettings>(builder.Configuration.GetSection("BookStoreDatabase")); // ����ע�� BookStoreDatabaseSettings
builder.Services.AddSingleton<BooksService>(); // ͨ������ע��ע�ᣬ��֧�ֹ��캯��ע��
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null); // ���� JSON ���л�ѡ��
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// ��Ӷ�����Ϣ
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Book Store API",
        Description = " ͨ�� MongoDB ���Ͽ�",
        TermsOfService = new Uri("https://example.com/terms"), // ��������
        Contact = new OpenApiContact
        {
            Name = "���緽ʽʾ��",
            Url = new Uri("https://example.com/contact"),
        },
        License = new OpenApiLicense
        {
            Name = "���֤ʾ��",
            Url = new Uri("https://example.com/license"),
        },
    });

    // ��� XML ע�ͣ���Է�������
    string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
}); // ��� Swagger �м��

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = String.Empty; // �Ӹ�·���ṩ����
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
