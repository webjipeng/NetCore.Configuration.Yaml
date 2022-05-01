using NetCoreConfigurationYaml.Simple.Model;

var builder = WebApplication.CreateBuilder(args);
// add yaml file configuration
builder.Configuration.AddYamlFile("appsettings.yml",true);
var app = builder.Build();

// get single value
var name = app.Configuration.GetValue<string>("user:name");
Console.WriteLine("name: "+name);
// get a model
var user1 = app.Configuration.GetSection("user").Get<User>();
Console.WriteLine("user1 "+user1);
// get a model
var user2 = new User();
app.Configuration.GetSection("user").Bind(user2);
Console.WriteLine("user2 "+user2);


app.MapGet("/", () => "Hello World!");
app.Run();