using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// System.Text.Json
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

    if (builder.Environment.IsDevelopment())
        options.JsonSerializerOptions.WriteIndented = true;

    // Reduce bytes
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;

    // Increase bytes & readability ;)
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


// Newtonsoft.Json
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

    if (builder.Environment.IsDevelopment())
        options.SerializerSettings.Formatting = Formatting.Indented;

    // Reduce bytes
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;

    // Increase bytes & readability ;)
    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
});


// System.Text.Json for Minimal APis (From .NET Core 7):
builder.Services.ConfigureHttpJsonOptions(options =>
{
    // Defaults to new JsonSerializerOptions(JsonSerializerDefaults.Web);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
