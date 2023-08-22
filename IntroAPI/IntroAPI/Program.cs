

#region Serilog

using Serilog.Formatting.Compact;
using Serilog;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var strCon = config.GetValue<string>("MySqlLogConnectionString");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.MySQL(strCon, tableName: "FIPPLOG")
    .CreateLogger();

#endregion

try
{

    Log.Information("Iniciando aplicação");

    var builder = WebApplication.CreateBuilder(args);
    // Add services to the container.

    builder.Services.AddControllers();

    builder.Host.UseSerilog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception e) {
    Log.Error(e, "Não levantou...");
};
