using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Tomlyn;
using Tomlyn.Model;
using Tomlyn.Syntax;

namespace Nouns.CLI.Configuration
{
    public sealed class TomlConfigurationProvider : FileConfigurationProvider
    {
        private readonly TomlConfigurationSource source;

        public TomlConfigurationProvider(TomlConfigurationSource source) : base(source)
        {
            this.source = source;
        }

        public override void Load(Stream stream)
        {
            if(!stream.CanRead)
                throw new ArgumentException("Stream is not readable", nameof(stream));

            if (stream.CanSeek)
            {
                var utf8Bytes = new byte[stream.Length - stream.Position];
                var read = stream.Read(utf8Bytes, 0, utf8Bytes.Length);
                Debug.Assert(read == utf8Bytes.Length);

                PopulateData(Toml.Parse(utf8Bytes, source.Path, source.ParserOptions));
            }
            else
            {
                using var sr = new StreamReader(stream, Encoding.UTF8);
                PopulateData(Toml.Parse(sr.ReadToEnd(), source.Path, source.ParserOptions));
            }
        }

        private void PopulateData(DocumentSyntax document)
        {
            Data.Clear();

            IDictionary<string, object> model = document.ToModel();

            foreach (var (k, v) in model)
            {
                if (v is TomlTable table)
                {
                    foreach (var (s, o) in table)
                    {
                        Data.Add(k + ":" + s, o.ToString());
                    }
                }

                if (v is TomlTableArray tableArray)
                {
                    foreach (var tableRow in tableArray)
                    {
                        foreach (var (s, o) in tableRow)
                        {
                            Data.Add(k + ":" + s, o.ToString());
                        }
                    }
                }
            }
        }
    }
}