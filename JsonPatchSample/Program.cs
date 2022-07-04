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
                // ���� NewtonsoftJsonPatchInputFormatter ʵ��
                // ��������Ϊ��һʵ����� MvcOptions.InputFormatter ������
                //
                // NewtonsoftJsonPatchInputFormatter ���� JSON Patch ����
                // System.Text.Json �������� JSON �������Ӧ
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