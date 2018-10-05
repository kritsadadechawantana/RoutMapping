using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UrlMapper.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var simStrParamBuilder = new SimpleStringParameterBuilder();
            var samStr = simStrParamBuilder.Parse("https://mana.com/app/{app/-id}/service&/{ser${0}vice-id}/htr/{ndjqwnkjqw}");
            var result = samStr.IsMatched("https://mana.com/app/fnk%$^>H/service&/gftryu/htr/{}P{{}{/");
            var RoutValues = new Dictionary<string, string>{};
            samStr.ExtractVariables("https://mana.com/app/fnk%$^>H/service&/gftryu/htr/{}P{   {}{", ref RoutValues);
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed.Milliseconds);
            foreach (var item in RoutValues)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }
        }
    }
}
