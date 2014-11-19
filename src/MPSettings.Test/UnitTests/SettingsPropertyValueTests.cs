using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Xunit;
using MPS = MPSettings.Provider;
using FluentAssertions;

namespace MPSettings.Test.UnitTests
{
    public class SettingsPropertyValueTests
    {
        [Fact]
        public void ConvertPrimitiveDatatypeToStringTest()
        {
            {
                Boolean t1 = true;
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("True");
            }
            {
                Byte t1 = 5;
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                SByte t1 = 5;
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                Int16 t1 = 5;
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                UInt16 t1 = 5;
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                Int32 t1 = 5;
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                UInt32 t1 = 5;
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                Int64 t1 = 5;
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                UInt64 t1 = 5;
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
           {
                Char t1 = '5';
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                Double t1 = 1.79;
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("1.79");
            }
            {
                Single t1 = 1.79f;
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("1.79");
            }
            {
                Decimal t1 = 1.79M;
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("1.79");
            }
            {
                string t1 = null;
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(string), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().BeNull();
            }
            {
                string t1 = "";
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("");
            }
            {
                string t1 = "foo";
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("foo");
            }
        }

        [Fact]
        public void ConvertStringToPrimitiveDatatypeTest()
        {
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(Boolean), null));
                SPV.SerializedValue = "True";
                SPV.PropertyValue.Should().BeOfType<Boolean>().Which.Should().BeTrue();
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(Byte), null));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<Byte>().Which.Should().Be(5);
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(SByte), null));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<SByte>().Which.Should().Be(5);
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(Int16), null));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<Int16>().Which.Should().Be(5);
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(UInt16), null));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<UInt16>().Which.Should().Be(5);
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(Int32), null));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<Int32>().Which.Should().Be(5);
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(UInt32), null));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<UInt32>().Which.Should().Be(5);
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(Int64), null));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<Int64>().Which.Should().Be(5);
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(UInt64), null));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<UInt64>().Which.Should().Be(5);
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(Char), null));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<Char>().Which.Should().Be('5');
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(Double), null));
                SPV.SerializedValue = "1.79";
                SPV.PropertyValue.Should().BeOfType<Double>().Which.Should().Be(1.79);
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(Single), null));
                SPV.SerializedValue = "1.79";
                SPV.PropertyValue.Should().BeOfType<Single>().Which.Should().Be(1.79f);
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(Decimal), null));
                SPV.SerializedValue = "1.79";
                SPV.PropertyValue.Should().BeOfType<Decimal>().Which.Should().Be(1.79M);
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(string), null));
                SPV.SerializedValue = null;
                //SPV.PropertyValue.Should().BeOfType<string>().Which.Should().BeNull();
                SPV.PropertyValue.Should().BeNull();
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(string), null));
                SPV.SerializedValue = "";
                SPV.PropertyValue.Should().BeOfType<string>().Which.Should().Be("");
            }
            {
                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", typeof(string), null));
                SPV.SerializedValue = "foo";
                SPV.PropertyValue.Should().BeOfType<string>().Which.Should().Be("foo");
            }

        }

        public class Test
        {
            public int foo;
        }


        [Fact]
        public void ConvertNonPrimitiveDatatypeToStringTest()
        {
            {
                Test t1 = new Test();
                t1.foo = 44;


                var SPV = new MPS.SettingsPropertyValue(new MPS.SettingsProperty("whatever", t1.GetType(), null));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("ff");
            }
            
        }

    }
}
