using Core.Logic.Logs;
using Core.Logic.Tracing.TraceIdLogic.TraceIdAccessors;
using Sagas.Posts.GetPost;
using Serilog;

namespace ApiGateaway
{
    public class Program
    {
        [Obsolete("Obsolete")]
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // adding logger
            builder.Services.AddLoggerServices();
            builder.Services.AddTraceId();
            Log.Logger = new LoggerConfiguration().GetConfiguration().CreateLogger();
            builder.Host.UseSerilog();
            builder.Services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));
            
            builder.Services.AddGetPostSagaServices(builder.Configuration);
            var connection = builder.Configuration.GetConnectionString("PostgreSqlConnection");
            builder.Services.ConfigureGetPostListSagaDb(connection);
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
