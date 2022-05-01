using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using YamlDotNet.Core;

namespace NetCore.Configuration.Yaml
{
    public class YamlConfigurationProvider:FileConfigurationProvider
    {
        public YamlConfigurationProvider(FileConfigurationSource source) : base(source)
        {
        }

        public override void Load(Stream stream)
        {
            var parser = new YamlFileParser();
            try
            {
                Data = parser.Parse(stream);
            }
            catch (YamlException e)
            {
                throw new FormatException("yaml file format error", e);
            }
        }
    }
}


