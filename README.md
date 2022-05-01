# NetCore.Configuration.Yaml
Make .netcore support yaml files

## How To Use

1. add new file `appsettings.yml`

   ```yaml
   user:
     name: mike
     age: 22
     birthday: 2000-11-11
     hobby:
       - basketball
       - football
       - baseball
   ```

2. add new class `User.cs`

   ```c#
   public class User
   {
       public string Name { get; set; }
       public int Age { get; set; }
       public DateTime Birthday { get; set; }
       public List<string> Hobby{get;set;}
   
       public override string ToString()
       {
           return $"user: {Name}, {Age}, {Birthday}, {string.Join(',',Hobby)}";
       }
   }
   ```

3. use NetCore.Configuration.Yaml

   ```c#
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
   ```

   ouput

   ```
   name: mike
   user1 user: mike, 22, 2000-11-11 0:00:00, basketball,football,baseball
   user2 user: mike, 22, 2000-11-11 0:00:00, basketball,football,baseball
   ```

   

