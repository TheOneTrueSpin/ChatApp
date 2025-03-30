using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ChatApp.Shared.Utils;

public static class EmbeddedFiles
{
    public static string ReadFile(string name)
    {
        using (Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
        {
            if (stream is null)
            {
                throw new Exception($"Could not find embedded file names: {name}");
            }

            using (StreamReader reader = new StreamReader(stream))
            {
                string content = reader.ReadToEnd();

                return content;
            }
        }
    }
}
