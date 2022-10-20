using System;
using System.ComponentModel.DataAnnotations;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Diagnostics
{
    public class FaultDescriptorOptionsTest : Test
    {
        public FaultDescriptorOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ValidateOptions_ShouldThrowArgumentNullExceptionForHttpFaultResolvers()
        {
            var sut1 = new FaultDescriptorOptions
            {
                HttpFaultResolvers = null
            };

            var sut2 = Assert.Throws<ArgumentNullException>(() => sut1.ValidateOptions());

            Assert.Equal("Value cannot be null. (Parameter 'HttpFaultResolvers')", sut2.Message);
        }

        [Fact]
        public void ValidateOptions_ShouldThrowArgumentNullExceptionForNonMvcResponseHandlers()
        {
            var sut1 = new FaultDescriptorOptions
            {
                NonMvcResponseHandlers = null
            };

            var sut2 = Assert.Throws<ArgumentNullException>(() => sut1.ValidateOptions());

            Assert.Equal("Value cannot be null. (Parameter 'NonMvcResponseHandlers')", sut2.Message);
        }

        [Fact]
        public void ValidateOptions_ShouldHaveDefaultValues()
        {
            var sut = new FaultDescriptorOptions();

            Assert.Null(sut.RootHelpLink);
            Assert.False(sut.HasRootHelpLink);
            Assert.False(sut.UseBaseException);
            Assert.Collection(sut.HttpFaultResolvers,
                resolver => Assert.True(resolver.TryResolveFault(new BadRequestException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new ConflictException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new ForbiddenException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new GoneException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new NotFoundException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new MethodNotAllowedException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new NotAcceptableException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new PayloadTooLargeException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new PreconditionFailedException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new PreconditionRequiredException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new TooManyRequestsException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new UnauthorizedException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new UnsupportedMediaTypeException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new ApiKeyException(400, "UnitTest"), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new ThrottlingException("UnitTest", 1, TimeSpan.FromMinutes(1), DateTime.Today), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new UserAgentException(400, "UnitTest"), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new ValidationException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new FormatException(), out _)),
                resolver => Assert.True(resolver.TryResolveFault(new ArgumentException(), out _)));
            Assert.NotNull(sut.ExceptionDescriptorResolver);
            Assert.Null(sut.ExceptionCallback);
            Assert.NotNull(sut.NonMvcResponseHandlers);
            Assert.Null(sut.RequestBodyParser);
            Assert.False(sut.IncludeRequest);
            Assert.False(sut.IncludeFailure);
            Assert.False(sut.IncludeStackTrace);
            Assert.False(sut.IncludeEvidence);
        }
    }
}
