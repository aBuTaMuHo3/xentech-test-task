using System;
using System.Collections.Generic;

namespace SynaptikonFramework.Interfaces.Language
{
    public interface ILanguage
    {
        string GetString(string id);

        string GetString(string id, Dictionary<string, string> variables);
    }
}
