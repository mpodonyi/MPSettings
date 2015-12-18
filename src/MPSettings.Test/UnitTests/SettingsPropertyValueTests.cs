﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Xunit;
using MPS = MPSettings.Provider;
using FluentAssertions;
using System.Globalization;
using System.Xml.Linq;
using SettingsProperty = MPSettings.Core.SettingsProperty;
using SettingsPropertyValue = MPSettings.Core.SettingsPropertyValue;

namespace MPSettings.Test.UnitTests
{
    public class SettingsPropertyValueTests
    {
        [Fact]
        public void Convert_PrimitiveDatatypeToString_Test()
        {
            {
                Boolean t1 = true;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("True");
            }
            {
                Byte t1 = 5;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                SByte t1 = 5;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                Int16 t1 = 5;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                UInt16 t1 = 5;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                Int32 t1 = 5;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                UInt32 t1 = 5;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                Int64 t1 = 5;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                UInt64 t1 = 5;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                Char t1 = '5';
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                Double t1 = 1.79;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("1.79");
            }
            {
                Single t1 = 1.79f;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("1.79");
            }
            {
                Decimal t1 = 1.79M;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("1.79");
            }
            {
                string t1 = null;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(string)));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().BeNull();
            }
            {
                string t1 = "";
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("");
            }
            {
                string t1 = "foo";
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", t1.GetType()));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("foo");
            }
        }

        [Fact]
        public void Convert_StringToPrimitiveDatatype_Test()
        {
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Boolean)));
                SPV.SerializedValue = "True";
                SPV.PropertyValue.Should().BeOfType<Boolean>().Which.Should().BeTrue();
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Byte)));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<Byte>().Which.Should().Be(5);
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(SByte)));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<SByte>().Which.Should().Be(5);
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Int16)));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<Int16>().Which.Should().Be(5);
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(UInt16)));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<UInt16>().Which.Should().Be(5);
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Int32)));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<Int32>().Which.Should().Be(5);
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(UInt32)));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<UInt32>().Which.Should().Be(5);
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Int64)));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<Int64>().Which.Should().Be(5);
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(UInt64)));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<UInt64>().Which.Should().Be(5);
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Char)));
                SPV.SerializedValue = "5";
                SPV.PropertyValue.Should().BeOfType<Char>().Which.Should().Be('5');
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Double)));
                SPV.SerializedValue = "1.79";
                SPV.PropertyValue.Should().BeOfType<Double>().Which.Should().Be(1.79);
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Single)));
                SPV.SerializedValue = "1.79";
                SPV.PropertyValue.Should().BeOfType<Single>().Which.Should().Be(1.79f);
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Decimal)));
                SPV.SerializedValue = "1.79";
                SPV.PropertyValue.Should().BeOfType<Decimal>().Which.Should().Be(1.79M);
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(string)));
                SPV.SerializedValue = null;
                //SPV.PropertyValue.Should().BeOfType<string>().Which.Should().BeNull();
                SPV.PropertyValue.Should().BeOfType<string>().Which.Should().Be("");
                //SPV.PropertyValue.Should().BeNull();
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(string)));
                SPV.SerializedValue = "";
                SPV.PropertyValue.Should().BeOfType<string>().Which.Should().Be("");
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(string)));
                SPV.SerializedValue = "foo";
                SPV.PropertyValue.Should().BeOfType<string>().Which.Should().Be("foo");
            }

        }

        public class Test
        {
            public int foo;
            public long Bar { get; set; }
        }


        private static readonly string CompareValueString = @"<?xml version=""1.0"" encoding=""utf-16""?>
                                                    <Test xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                                                    <foo>44</foo>
                                                    <Bar>3</Bar>
                                                </Test>";
        private static readonly XDocument CompareValue = XDocument.Parse(CompareValueString);

        [Fact]
        public void Convert_NonPrimitiveDatatypeToString_Test()
        {
            {
                Test t1 = new Test();
                t1.foo = 44;
                t1.Bar = 3;

                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Test)));
                SPV.PropertyValue = t1;
                XDocument.Parse(SPV.SerializedValue).Should().BeEquivalentTo(CompareValue);
            }
            {
                Test t1 = new Test();
                t1.foo = 44;
                t1.Bar = 3;

                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Test)));
                SPV.PropertyValue = null;
                SPV.SerializedValue.Should().BeNull();
            }
        }

        [Fact]
        public void Convert_StringToNonPrimitiveDatatype_Test()
        {
            {
                Test t1 = new Test();
                t1.foo = 44;
                t1.Bar = 3;

                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Test)));
                SPV.SerializedValue = CompareValueString;
                SPV.PropertyValue.Should().BeOfType<Test>().Which.ShouldBeEquivalentTo(t1);
            }
            {
                Test t1 = new Test();
                t1.foo = 44;
                t1.Bar = 3;

                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Test)));
                SPV.SerializedValue = null;
                SPV.PropertyValue.Should().BeNull();
            }
        }


        [Fact]
        public void Convert_DateTimeToString_Test()
        {
            {
                DateTime t1 = new DateTime(2014, 11, 24, 11, 40, 2, 3, DateTimeKind.Unspecified);
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(DateTime)));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("2014-11-24T11:40:02.0030000");
            }
            {
                DateTime? t1 = new DateTime(2014, 11, 24, 11, 40, 2, 3, DateTimeKind.Unspecified);
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(DateTime?)));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("2014-11-24T11:40:02.0030000");
            }
        }

        [Fact]
        public void Convert_StringToDateTime_Test()
        {
            {
                string t1 = "2014-11-24T11:40:02.0030000";
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(DateTime)));
                SPV.SerializedValue = t1;
                SPV.PropertyValue.Should().BeOfType<DateTime>().Which.Should().Be(new DateTime(2014, 11, 24, 11, 40, 2, 3, DateTimeKind.Unspecified));
            }
            {
                string t1 = "2014-11-24T11:40:02.0030000";
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(DateTime?)));
                SPV.SerializedValue = t1;
                SPV.PropertyValue.Should().BeOfType<DateTime>().Which.Should().Be(new DateTime(2014, 11, 24, 11, 40, 2, 3, DateTimeKind.Unspecified));
            }
            {
                string t1 = null;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(DateTime?)));
                SPV.SerializedValue = t1;
                SPV.PropertyValue.Should().BeNull();
            }
            {
                string t1 = "";
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(DateTime?)));
                SPV.SerializedValue = t1;
                SPV.PropertyValue.Should().BeNull();
            }
        }

        [Fact]
        public void Convert_DateTimeOffsetToString_Test()
        {
            {
                DateTimeOffset t1 = new DateTimeOffset(2014, 11, 24, 11, 40, 2, 3, TimeSpan.Zero);
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(DateTimeOffset)));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("2014-11-24T11:40:02.0030000+00:00");
            }
            {
                DateTimeOffset? t1 = new DateTimeOffset(2014, 11, 24, 11, 40, 2, 3, TimeSpan.Zero);
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(DateTimeOffset?)));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("2014-11-24T11:40:02.0030000+00:00");
            }
        }

        [Fact]
        public void Convert_StringToDateTimeOffset_Test()
        {
            {
                string t1 = "2014-11-24T11:40:02.0030000";
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(DateTimeOffset)));
                SPV.SerializedValue = t1;
                SPV.PropertyValue.Should().BeOfType<DateTimeOffset>().Which.Should().Be(new DateTimeOffset(2014, 11, 24, 11, 40, 2, 3, TimeSpan.Zero));
            }
            {
                string t1 = "2014-11-24T11:40:02.0030000";
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(DateTimeOffset?)));
                SPV.SerializedValue = t1;
                SPV.PropertyValue.Should().BeOfType<DateTimeOffset>().Which.Should().Be(new DateTimeOffset(2014, 11, 24, 11, 40, 2, 3, TimeSpan.Zero));
            }
            {
                string t1 = null;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(DateTimeOffset?)));
                SPV.SerializedValue = t1;
                SPV.PropertyValue.Should().BeNull();
            }
            {
                string t1 = "";
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(DateTimeOffset?)));
                SPV.SerializedValue = t1;
                SPV.PropertyValue.Should().BeNull();
            }
        }



        [Fact]
        public void Convert_NullablePrimitiveDatatypeToString_Test()
        {
            {
                Int16? t1 = 5;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Int16?)));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().Be("5");
            }
            {
                Int16? t1 = null;
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Int16?)));
                SPV.PropertyValue = t1;
                SPV.SerializedValue.Should().BeNull();
            }

        }

        [Fact]
        public void Convert_StringToNullablePrimitiveDatatype_Test()
        {
            //the type returned will be the underlying type; there is no way to generate a nullable type with an underlying type "dynamically"
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Int16?)));
                SPV.SerializedValue = "5";
                ((Int16?)SPV.PropertyValue).Should().Be(5);
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Int16?)));
                SPV.SerializedValue = null;
                ((Int16?)SPV.PropertyValue).Should().NotHaveValue();
            }
            {
                var SPV = new SettingsPropertyValue(new SettingsProperty("whatever", typeof(Int16?)));
                SPV.SerializedValue = "";
                ((Int16?)SPV.PropertyValue).Should().NotHaveValue();
            }

        }

    }
}
