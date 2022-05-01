namespace NetCoreConfigurationYaml.Simple.Model;

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