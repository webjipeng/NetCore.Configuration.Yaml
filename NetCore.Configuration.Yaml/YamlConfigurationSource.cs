using Microsoft.Extensions.Configuration;

namespace NetCore.Configuration.Yaml
{
    public class YamlConfigurationSource:FileConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new YamlConfigurationProvider(this);
        }
    }
}