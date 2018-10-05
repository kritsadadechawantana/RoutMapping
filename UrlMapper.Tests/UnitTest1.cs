using System;
using Xunit;

namespace UrlMapper.Tests
{
    public class UnitTest1
    {
        [Fact(Skip = "Just a sample test.")]
        public void Test1()
        {
            var simStrParamBuilder = new SimpleStringParameterBuilder();
            var samStr = simStrParamBuilder.Parse("https://mana.com/app/{app/-id}/services/{service-id}");
            var result = samStr.IsMatched("https://mana.com/app/fnk%$^>H/services/gftryu");
            Assert.Equal(true, result);
        }
    }
}
