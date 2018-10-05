using System.Collections.Generic;

namespace UrlMapper
{
    public interface ISimpleStringParameter
    {
        bool IsMatched(string textToCompare);
        void ExtractVariables(string target, ref Dictionary<string, string> dicToStoreResults);
    }
}