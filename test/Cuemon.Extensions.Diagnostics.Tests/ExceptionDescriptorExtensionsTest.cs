using System;
using System.Collections;
using System.Collections.Generic;
using Cuemon.Collections.Generic;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Diagnostics
{
    public class ExceptionDescriptorExtensionsTest : Test
    {
        public ExceptionDescriptorExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ToInsightsString_ShouldReturnDetailedExceptionString_WithAllCaptures()
        {
            InsufficientMemoryException ime = null;
            try
            {
                throw new InsufficientMemoryException()
                {
                    Data = { { "Name", "Cuemon" } }
                };
            }
            catch (InsufficientMemoryException e)
            {
                ime = ExceptionInsights.Embed(e, snapshots: SystemSnapshots.CaptureAll);
            }

            var sut1 = ExceptionDescriptor.Extract(ime);
            var sut2 = sut1.ToInsightsString(o =>
            {
                o.SensitivityDetails = FaultSensitivityDetails.All;
            });

            TestOutput.WriteLine(sut2);

            Assert.Contains(@"Error
	Code: UnhandledException
	Message: An unhandled exception occurred.".ReplaceLineEndings(), sut2);
            Assert.Contains(@$"Failure
	System.InsufficientMemoryException
		Source: Cuemon.Extensions.Diagnostics.Tests
		Message: Insufficient memory to continue the execution of the program.
		Stack:
			at Cuemon.Extensions.Diagnostics.ExceptionDescriptorExtensionsTest.{nameof(ToInsightsString_ShouldReturnDetailedExceptionString_WithAllCaptures)}()".ReplaceLineEndings(), sut2);
            Assert.Contains(@"Data:
			Name=Cuemon".ReplaceLineEndings(), sut2);
            Assert.Contains(@$"Evidence
	Thrower:
		Cuemon.Extensions.Diagnostics.ExceptionDescriptorExtensionsTest.{nameof(ToInsightsString_ShouldReturnDetailedExceptionString_WithAllCaptures)}()".ReplaceLineEndings(), sut2);
            Assert.Contains(@"	Thread:", sut2);
            Assert.Contains(@"	Process:", sut2);
            Assert.Contains(@"	Environment:", sut2);
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void ToInsightsString_ShouldReturnDetailedExceptionString_WithCaptures_MakeUseOfIncludeOptions(ExceptionDescriptorOptions options, SystemSnapshots snapshots)
        {
            InsufficientMemoryException ime = null;
            try
            {
                throw new InsufficientMemoryException()
                {
                    Data = { {"Name", "Cuemon"}, }
                };
            }
            catch (InsufficientMemoryException e)
            {
                ime = ExceptionInsights.Embed(e, Arguments.ToArray(options, snapshots), snapshots);
            }

            var sut1 = ExceptionDescriptor.Extract(ime);
            var sut2 = sut1.ToInsightsString(Patterns.ConfigureRevert(options));

            TestOutput.WriteLine(sut2);

            Assert.Contains(@"Error
	Code: UnhandledException
	Message: An unhandled exception occurred.".ReplaceLineEndings(), sut2);

            Condition.FlipFlop(options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence), () =>
            {
                Assert.Contains(@$"Evidence
	Thrower:
		Cuemon.Extensions.Diagnostics.ExceptionDescriptorExtensionsTest.{nameof(ToInsightsString_ShouldReturnDetailedExceptionString_WithCaptures_MakeUseOfIncludeOptions)}".ReplaceLineEndings(), sut2);
            }, () =>
            {
                Assert.DoesNotContain(@$"Evidence
	Thrower:
		Cuemon.Extensions.Diagnostics.ExceptionDescriptorExtensionsTest.{nameof(ToInsightsString_ShouldReturnDetailedExceptionString_WithCaptures_MakeUseOfIncludeOptions)}".ReplaceLineEndings(), sut2);
            });

            Condition.FlipFlop(options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Failure), () =>
            {
                Assert.Contains(@$"Failure
	System.InsufficientMemoryException
		Source: Cuemon.Extensions.Diagnostics.Tests
		Message: Insufficient memory to continue the execution of the program.".ReplaceLineEndings(), sut2);
            }, () =>
            {
                Assert.DoesNotContain(@$"Failure
	System.InsufficientMemoryException
		Source: Cuemon.Extensions.Diagnostics.Tests
		Message: Insufficient memory to continue the execution of the program.".ReplaceLineEndings(), sut2);
            });

            Condition.FlipFlop(options.SensitivityDetails.HasFlag(FaultSensitivityDetails.FailureWithStackTrace), () =>
            {
                Assert.Contains(@$"Stack:
			at Cuemon.Extensions.Diagnostics.ExceptionDescriptorExtensionsTest.{nameof(ToInsightsString_ShouldReturnDetailedExceptionString_WithCaptures_MakeUseOfIncludeOptions)}".ReplaceLineEndings(), sut2);
            }, () =>
            {
                Assert.DoesNotContain(@$"Stack:
			at Cuemon.Extensions.Diagnostics.ExceptionDescriptorExtensionsTest.{nameof(ToInsightsString_ShouldReturnDetailedExceptionString_WithCaptures_MakeUseOfIncludeOptions)}".ReplaceLineEndings(), sut2);
            });

            
            Condition.FlipFlop(options.SensitivityDetails.HasFlag(FaultSensitivityDetails.FailureWithData), () =>
            {
                Assert.Contains(@"Data:
			Name=Cuemon".ReplaceLineEndings(), sut2);
            }, () =>
            {
                Assert.DoesNotContain(@"Data:
			Name=Cuemon".ReplaceLineEndings(), sut2);
            });

            Condition.FlipFlop(snapshots.HasFlag(SystemSnapshots.CaptureThreadInfo), () => Assert.Contains(@"	Thread:", sut2), () => Assert.DoesNotContain(@"	Thread:".ReplaceLineEndings(), sut2));
            Condition.FlipFlop(snapshots.HasFlag(SystemSnapshots.CaptureProcessInfo), () => Assert.Contains(@"	Process:", sut2), () => Assert.DoesNotContain(@"	Process:".ReplaceLineEndings(), sut2));
            Condition.FlipFlop(snapshots.HasFlag(SystemSnapshots.CaptureEnvironmentInfo), () => Assert.Contains(@"	Environment:", sut2), () => Assert.DoesNotContain(@"	Environment:".ReplaceLineEndings(), sut2));
        }

        public static IEnumerable<object[]> GetData()
        {
            var parameters = new List<object[]>()
            {
                new object[]
                {
                    new ExceptionDescriptorOptions()
                    {
                        SensitivityDetails = FaultSensitivityDetails.FailureWithStackTrace | FaultSensitivityDetails.Evidence
                    }, 
                    SystemSnapshots.CaptureAll },
                new object[]
                {
                    new ExceptionDescriptorOptions(),
                    SystemSnapshots.None
                },
                new object[]
                {
                    new ExceptionDescriptorOptions()
                    {
                        SensitivityDetails = FaultSensitivityDetails.All
                    },
                    SystemSnapshots.CaptureThreadInfo
                },
                new object[]
                {
                    new ExceptionDescriptorOptions()
                    {
                        SensitivityDetails = FaultSensitivityDetails.FailureWithStackTrace
                    },
                    SystemSnapshots.None
                },
                new object[]
                {
                    new ExceptionDescriptorOptions()
                    {
                        SensitivityDetails = FaultSensitivityDetails.Failure | FaultSensitivityDetails.Evidence
                    },
                    SystemSnapshots.CaptureProcessInfo
                },
                new object[]
                {
                    new ExceptionDescriptorOptions()
                    {
                        SensitivityDetails = FaultSensitivityDetails.Failure
                    },
                    SystemSnapshots.None
                },
                new object[]
                {
                    new ExceptionDescriptorOptions()
                    {
                        SensitivityDetails = FaultSensitivityDetails.Evidence
                    },
                    SystemSnapshots.CaptureAll
                },
                new object[]
                {
                    new ExceptionDescriptorOptions()
                    {
                        SensitivityDetails = FaultSensitivityDetails.FailureWithStackTraceAndData
                    },
                    SystemSnapshots.None
                },
                new object[]
                {
                    new ExceptionDescriptorOptions()
                    {
                        SensitivityDetails = FaultSensitivityDetails.FailureWithData
                    },
                    SystemSnapshots.None
                }
            };

            return parameters;
        }
    }
}