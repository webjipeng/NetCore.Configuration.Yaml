using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using YamlDotNet.RepresentationModel;

namespace NetCore.Configuration.Yaml
{
    public class YamlFileParser
    {
        private readonly IDictionary<string, string> _data =
            new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Stack<string> _context = new Stack<string>();
        private string _currentPath;
        
        public IDictionary<string,string> Parse(Stream stream)
        {
            _data.Clear();
            _context.Clear();
            
            var yaml = new YamlStream();
            yaml.Load(new StreamReader(stream,Encoding.UTF8,true));
            if(yaml.Documents.Any())
            {
                var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
                LoadYamlMappingNode(mapping);
            }
            return _data;
        }
        
        private void LoadYamlMappingNode(YamlMappingNode node)
        {
            foreach (var yamlNodePair in node.Children)
            {
                LoadYamlNodePair(yamlNodePair);
            }
        }
        
        private void LoadYamlNodePair(KeyValuePair<YamlNode, YamlNode> yamlNodePair)
        {
            var context = ((YamlScalarNode)yamlNodePair.Key).Value;
            LoadYamlNode(context, yamlNodePair.Value);
        }

        private void LoadYamlNode(string context, YamlNode node)
        {
            if (node is YamlScalarNode scalarNode)
            {
                LoadYamlScalarNode(context, scalarNode);
            }
            if (node is YamlMappingNode mappingNode)
            {
                LoadYamlMappingNode(context, mappingNode);
            }
            if (node is YamlSequenceNode sequenceNode)
            {
                LoadYamlSequenceNode(context, sequenceNode);
            }
        }

        private void LoadYamlScalarNode(string context, YamlScalarNode yamlValue)
        {
            EnterContext(context);
            var currentKey = _currentPath;
            if (_data.ContainsKey(currentKey))
            {
                throw new FormatException($"{currentKey} key is duplicated");
            }
            _data[currentKey] = yamlValue.Value;
            ExitContext();
        }



        private void LoadYamlMappingNode(string context, YamlMappingNode yamlValue)
        {
            EnterContext(context);
            LoadYamlMappingNode(yamlValue);
            ExitContext();
        }

        private void LoadYamlSequenceNode(string context, YamlSequenceNode yamlValue)
        {
            EnterContext(context);
            LoadYamlSequenceNode(yamlValue);
            ExitContext();
        }

        private void LoadYamlSequenceNode(YamlSequenceNode node)
        {
            for (var i = 0; i < node.Children.Count; i++)
            {
                LoadYamlNode(i.ToString(), node.Children[i]);
            }
        }

        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }
    }
}