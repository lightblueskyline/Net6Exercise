using Microsoft.AspNetCore.Mvc.Formatters;

namespace JsonPatchSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddControllers()
            //    .AddNewtonsoftJson();
            builder.Services.AddControllers(options =>
            {
                // 创建 NewtonsoftJsonPatchInputFormatter 实例
                // 并将其作为第一实体插入 MvcOptions.InputFormatter 集合中
                //
                // NewtonsoftJsonPatchInputFormatter 处理 JSON Patch 请求
                // System.Text.Json 处理其他 JSON 请求和响应
                options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = String.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}