using System.Text.Json.Serialization;
using Orleans.Configuration;

var builder = WebApplication.CreateBuilder(args);

var invariant = "Npgsql";
var connectionString =
    "Host=shortingdb;Port=5432;Database=shorting-db;Username=postgres;Password=Compaq2009";

builder.Host.UseOrleans((ctx, siloBuilder) =>
{
    if (builder.Environment.IsDevelopment())
    {
        siloBuilder.UseLocalhostClustering();
        //siloBuilder.AddMemoryGrainStorageAsDefault();
        siloBuilder.AddMemoryGrainStorage("shopping-cart");
        //siloBuilder.UseInMemoryReminderService();
        siloBuilder.AddStartupTask<SeedData>();
    }
    else
    {
        siloBuilder.ConfigureEndpoints(11111, 30000);
        siloBuilder.UseAdoNetClustering(options =>
        {
            options.Invariant = invariant;
            options.ConnectionString = connectionString;
        });
        siloBuilder.UseAdoNetReminderService(options =>
        {
            options.Invariant = invariant;
            options.ConnectionString = connectionString;
        });
        siloBuilder.AddAdoNetGrainStorage("shopping-cart", options =>
        {
            options.Invariant = invariant;
            options.ConnectionString = connectionString;
            //options.UseJsonFormat = true;
        });

        siloBuilder.Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "shoppingCluster";
            options.ServiceId = "shoppingService";
        });
    }
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        ops =>
        {
            ops
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddSwaggerDocument(config =>
{
    config.PostProcess = document =>
    {
        document.Info.Version = "v1";
        document.Info.Title = "Shopping APi";
        document.Info.Description = "Web API for Shopping";
        //document.Info.TermsOfService = "None";
        document.Info.Contact = new NSwag.OpenApiContact
        {
            Name = "systemad",
            Email = string.Empty,
            Url = "https://github.com/Systemad/"
        };
        
        /*
        document.Info.License = new NSwag.OpenApiLicense
        {
            Name = "Use under LICX",
            Url = "https://example.com/license"
        };
        */
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("CorsPolicy");
app.UseOpenApi();
app.UseSwaggerUi3();
app.UseHttpsRedirection();
app.UseRouting();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

app.Run();