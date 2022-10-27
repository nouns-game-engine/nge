using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Tomlyn;
using Tomlyn.Model;
using Tomlyn.Syntax;

namespace NGE.Core.Configuration
{
    public sealed class TomlConfigurationProvider : FileConfigurationProvider
    {
        private readonly TomlConfigurationSource source;
        private byte[] contentHash = Array.Empty<byte>();

        private DocumentSyntax? document;

        public TomlConfigurationProvider(TomlConfigurationSource source) : base(source)
        {
            this.source = source;

            if (!File.Exists(source.Path))
                return;

            var utf8Bytes = File.ReadAllBytes(source.Path);
            contentHash = MD5.HashData(utf8Bytes);
            document = Toml.Parse(utf8Bytes, source.Path, source.ParserOptions);
            PopulateData();
        }

        public void Save()
        {
            SaveAs(source.Path);
        }

        public void SaveAs(string path)
        {
            if (document == null)
            {
                Trace.TraceWarning($"No configuration document was found while attempting to save to {path}");
                return;
            }
            using var fs = File.OpenWrite(path);
            using var sw = new StreamWriter(fs);
            document.WriteTo(sw);
        }

        public override void Set(string key, string value)
        {
            base.Set(key, value);

            Debug.Assert(document != null);
            var model = document.ToModel();

            TomlTable? keyContainer = null;
            while (key.Contains(':'))
            {     
                var keyPath = key[..key.IndexOf(':', StringComparison.Ordinal)].ToLowerInvariant();

                if(!model.TryGetValue(keyPath, out var tomlTable))
                    model.Add(keyPath, tomlTable = new TomlTable());
                
                keyContainer = tomlTable as TomlTable;
                key = key[(1 + key.IndexOf(':', StringComparison.Ordinal))..];
            }

            if (keyContainer == null)
                return;

            keyContainer[key] = value;
            
            var toml = Toml.FromModel(model);
            
            var newContentHash = MD5.HashData(Encoding.UTF8.GetBytes(toml));
            if (contentHash.SequenceEqual(newContentHash)) return;

            contentHash = newContentHash;
            File.WriteAllText(source.Path, toml);
        }

        public override void Load(Stream stream)
        {
            var currentHash = GetCurrentHash();
            if (contentHash.SequenceEqual(currentHash))
                return; // no changes detected

            contentHash = currentHash;

            Trace.WriteLine("Reloading configuration");

            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable", nameof(stream));

            if (stream.CanSeek)
            {
                var utf8Bytes = new byte[stream.Length - stream.Position];
                var read = stream.Read(utf8Bytes, 0, utf8Bytes.Length);
                Debug.Assert(read == utf8Bytes.Length);

                document = Toml.Parse(utf8Bytes, source.Path, source.ParserOptions);
                PopulateData();
            }
            else
            {
                using var sr = new StreamReader(stream, Encoding.UTF8);
                document = Toml.Parse(sr.ReadToEnd(), source.Path, source.ParserOptions);
                PopulateData();
            }
        }

        private byte[] GetCurrentHash() => File.Exists(source.Path) ? MD5.HashData(File.ReadAllBytes(source.Path)) : Array.Empty<byte>();

        private void PopulateData()
        {
            Data.Clear();

            Debug.Assert(document != null);
            IDictionary<string, object> model = document.ToModel();

            foreach (var (k, v) in model)
            {
                switch (v)
                {
                    case TomlTable table:
                    {
                        foreach (var (s, o) in table)
                        {
                            Data.Add($"{k}:{s}", o.ToString());
                        }

                        break;
                    }
                    case TomlTableArray tableArray:
                    {
                        foreach (var tableRow in tableArray)
                        {
                            foreach (var (s, o) in tableRow)
                            {
                                Data.Add($"{k}:{s}", o.ToString());
                            }
                        }

                        break;
                    }
                }
            }
        }
    }
}