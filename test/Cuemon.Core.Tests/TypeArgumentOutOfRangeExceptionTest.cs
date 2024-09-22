﻿using System.Runtime.InteropServices;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Cuemon.Xml.Serialization.Formatters;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class TypeArgumentOutOfRangeExceptionTest : Test
    {
        public TypeArgumentOutOfRangeExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TypeArgumentOutOfRangeException_ShouldBeSerializable_Json()
        {
            var randomParamName = Generate.RandomString(10);
            var actualValue = 42;
            var randomMessage = Generate.RandomString(50);
            var sut1 = new TypeArgumentOutOfRangeException(randomParamName, 42, randomMessage);
            var sut2 = new NewtonsoftJsonFormatter();
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<TypeArgumentOutOfRangeException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.ParamName, original.ParamName);
            Assert.Equal(sut1.ActualValue!.ToString(), original.ActualValue!.ToString());
            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(sut1.ToString(), original.ToString());

#if NET48_OR_GREATER
            Assert.Equal($$"""
                         {
                           "type": "Cuemon.TypeArgumentOutOfRangeException",
                           "message": "{{randomMessage}}\r\nParameter name: {{randomParamName}}\r\nActual value was {{actualValue}}.",
                           "actualValue": {{actualValue}},
                           "paramName": "{{randomParamName}}"
                         }
                         """.ReplaceLineEndings(), sut4);
#else
            var newline = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @"\r\n" : @"\n";
            Assert.Equal($$"""
                           {
                             "type": "Cuemon.TypeArgumentOutOfRangeException",
                             "message": "{{randomMessage}} (Parameter '{{randomParamName}}'){{newline}}Actual value was {{actualValue}}.",
                             "actualValue": {{actualValue}},
                             "paramName": "{{randomParamName}}"
                           }
                           """.ReplaceLineEndings(), sut4);
#endif
        }

        [Fact]
        public void TypeArgumentOutOfRangeException_ShouldBeSerializable_Xml()
        {
            var randomParamName = Generate.RandomString(10);
            var actualValue = 42;
            var randomMessage = Generate.RandomString(50);
            var sut1 = new TypeArgumentOutOfRangeException(randomParamName, actualValue, randomMessage);
            var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true);
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<TypeArgumentOutOfRangeException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.ParamName, original.ParamName);
            Assert.Equal(sut1.ActualValue!.ToString(), original.ActualValue!.ToString());

            TestOutput.WriteLine("---");
            TestOutput.WriteLine(sut1.ToString());
            TestOutput.WriteLine("---");
            TestOutput.WriteLine(original.ToString());
            TestOutput.WriteLine("---");

            Assert.Equal(sut1.Message, original.Message, ignoreLineEndingDifferences: true);
            Assert.Equal(sut1.ToString(), original.ToString());

#if NET48_OR_GREATER
            Assert.Equal($$"""
                         <?xml version="1.0" encoding="utf-8"?>
                         <TypeArgumentOutOfRangeException namespace="Cuemon">
                         	<Message>{{randomMessage}}
                         Parameter name: {{randomParamName}}
                         Actual value was {{actualValue}}.</Message>
                         	<ActualValue>{{actualValue}}</ActualValue>
                         	<ParamName>{{randomParamName}}</ParamName>
                         </TypeArgumentOutOfRangeException>
                         """.ReplaceLineEndings(), sut4);
#else
            Assert.Equal($$"""
                           <?xml version="1.0" encoding="utf-8"?>
                           <TypeArgumentOutOfRangeException namespace="Cuemon">
                           	<Message>{{randomMessage}} (Parameter '{{randomParamName}}')
                           Actual value was {{actualValue}}.</Message>
                           	<ActualValue>{{actualValue}}</ActualValue>
                           	<ParamName>{{randomParamName}}</ParamName>
                           </TypeArgumentOutOfRangeException>
                           """.ReplaceLineEndings(), sut4);
#endif
        }
    }
}
