using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.Security;

namespace Cuemon.Text
{
    public class StringObfuscator : Obfuscator
    {
        public StringObfuscator() : this(Encoding.UTF8)
        {
        }

        public StringObfuscator(Encoding encoding) : base(encoding)
        {
        }

        public override Stream CreateMapping()
        {
            return Disposable.SafeInvoke(() => new MemoryStream(), ms =>
            {
                using (var writer = new StreamWriter(ms, Encoding, 512, true))
                {
                    foreach (var kvp in Mappings)
                    {
                        writer.WriteLine($"{kvp.Value.Obfuscated};^{kvp.Value.Original}^");
                    }
                }
                ms.Flush();
                ms.Position = 0;
                return ms;
            });
        }

        public override Stream Obfuscate(Stream value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            var oldPosition = value.Position;
            return Disposable.SafeInvoke(() => new MemoryStream(), ms =>
            {
                using (var writer = new StreamWriter(ms, Encoding, 512, true))
                {
                    using (var reader = new StreamReader(value, Encoding, false, 512, true))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var mapping = ManageMappings(line);
                            writer.WriteLine(Exclusions.Contains(mapping.Original) ? mapping.Original : mapping.Obfuscated);
                        }
                    }
                }
                ms.Flush();
                ms.Position = 0;
                if (value.CanSeek) { value.Position = oldPosition; }
                return ms;
            });
        }

        private ObfuscatorMapping ManageMappings(string value)
        {
            var existing = true;
            var valueHash = ComputeHash(value);
            if (!Mappings.ContainsKey(valueHash))
            {
                lock (Mappings)
                {
                    existing = false;
                    Mappings.Add(valueHash, new ObfuscatorMapping(GenerateObfuscatedValue(), value));
                }
            }
            if (existing) { Mappings[valueHash].IncrementCount(); }
            return Mappings[valueHash];
        }

        public override Stream Revert(Stream obfuscated, Stream mapping)
        {
            Validator.ThrowIfNull(obfuscated, nameof(obfuscated));
            Validator.ThrowIfNull(mapping, nameof(mapping));

            var mappings = new Dictionary<string, string>();
            using (var mappedReader = Disposable.SafeInvoke(() => new StreamReader(mapping), sr => sr))
            {
                string line;
                while ((line = mappedReader.ReadLine()) != null)
                {
                    var kvp = StringUtility.SplitDsv(line, ";", "^");
                    mappings.Add(kvp[0], kvp[1]);
                }
            }

            return Disposable.SafeInvoke(() => new MemoryStream(), ms =>
            {
                using (var writer = new StreamWriter(ms, Encoding, 512, true))
                {
                    using (var reader = new StreamReader(obfuscated, Encoding, false, 512, true))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (mappings.TryGetValue(line, out var value))
                            {
                                writer.WriteLine(Exclusions.Contains(line) ? line : value);
                            }
                        }
                    }
                }
                ms.Flush();
                ms.Position = 0;
                return ms;
            });
        }
    }
}