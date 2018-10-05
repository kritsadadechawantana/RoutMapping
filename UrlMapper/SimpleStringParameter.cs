using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace UrlMapper
{
    public class SimpleStringParameter : ISimpleStringParameter
    {
        private readonly string pattern;
        public SimpleStringParameter(string pattern)
        {
            this.pattern = pattern;
        }
        public void ExtractVariables(string target, ref Dictionary<string, string> dicToStoreResults)
        {
            var variables = new Dictionary<string, string>{};
            if(IsMatched(target))
            {
                var patternUrl = this.pattern.Split(new string[] { "/{" }, StringSplitOptions.None);
                var patternBaseUrl = patternUrl[0] ?? string.Empty;
                var patternRoutUrl = this.pattern.Replace(patternBaseUrl, ""); 

                var patternRoutParams = getPatternRoutParams(patternRoutUrl);

                if(!target.StartsWith(patternBaseUrl)) return;
                target = target.Replace($"{patternBaseUrl}/", string.Empty);   
                var result = extractVariables(target, patternRoutParams);
                dicToStoreResults = result;
            }
        }
        public bool IsMatched(string textToCompare)
        {
            var patternUrl = this.pattern.Split(new string[] { "/{" }, StringSplitOptions.None);
            var patternBaseUrl = patternUrl[0] ?? string.Empty;
            var patternRoutUrl = this.pattern.Replace(patternBaseUrl, ""); 

            var patternRoutParams = getPatternRoutParams(patternRoutUrl);

            if(!textToCompare.StartsWith(patternBaseUrl)) return false;
            textToCompare = textToCompare.Replace($"{patternBaseUrl}/", string.Empty);   
            var isMatch = MatchPattern(textToCompare, patternRoutParams);

            return isMatch;
        }
        private bool MatchPattern(string textToCompare, string[] routParams)
        {
            foreach (var routParam in routParams)
            {
                string param = string.Empty;
                if(routParam.StartsWith("{") && routParam.EndsWith("}"))
                {
                    var beginIndex = 0;
                    var endIndex = textToCompare.IndexOf("/");
                    endIndex = endIndex != -1 ? endIndex : textToCompare.Length - 1;
                    param = textToCompare.Substring(beginIndex, endIndex - beginIndex);
                }
                else if(textToCompare.StartsWith(routParam))
                {
                    param = routParam;
                }
                else return false;
                textToCompare = textToCompare.Replace($"{param}/", "");
            }

            return true;
        }
        private Dictionary<string, string> extractVariables(string textToCompare, string[] routParams)
        {
            var variables = new Dictionary<string, string>{};
            foreach (var routParam in routParams)
            {
                string param = string.Empty;
                if(routParam.StartsWith("{") && routParam.EndsWith("}"))
                {
                    var beginIndex = 0;
                    var endIndex = textToCompare.IndexOf("/");
                    endIndex = endIndex != -1 ? endIndex : textToCompare.Length;
                    param = textToCompare.Substring(beginIndex, endIndex - beginIndex);
                    variables.Add(routParam, param);
                }
                else if(textToCompare.StartsWith(routParam))
                {
                    param = routParam;
                }
                else return variables;
                textToCompare = textToCompare.Replace($"{param}/", "");
            }

            return variables;
        }
        private string[] getPatternRoutParams(string patternRoutUrl)
        {  
            var routParams = new List<string>();

            for(int index = 0; !string.IsNullOrEmpty(patternRoutUrl); index++)
            {
                var routParam = getRoutParam(patternRoutUrl);
                routParams.Add(routParam);
                patternRoutUrl = patternRoutUrl.Replace($"/{routParam}", "");
            }

            return routParams.ToArray();
        }
        private string getRoutParam(String url)
        {
            string routParam;
            if(url.StartsWith("/{"))
            {
                var beginIndex = url.IndexOf("/{");
                var endIndex = url.IndexOf("}/") != -1 ? url.IndexOf("}/") : url.IndexOf("}");
                endIndex = endIndex != -1 ? endIndex : url.Length - 1;
                routParam = url.Substring(beginIndex +1, endIndex - beginIndex);
            }else{
                var beginIndex = 1;
                var endIndex = url.IndexOf("/{");
                endIndex = endIndex != -1 ? endIndex : url.Length;
                routParam = url.Substring(beginIndex, endIndex - beginIndex);
            }
            return routParam;
        }
    }
}