using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using YamlSettingsProvider.Test.Properties;
using FluentAssertions;

namespace YamlSettingsProvider.Test.Tests
{
    
    public class BasicSettingsTests
    {
        [Fact]
        public void String_Returned_Successful()
        {
            Settings.Default.TestStringProp.Should().Be("bar");

            
        }

    }
}
