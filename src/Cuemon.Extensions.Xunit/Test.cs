using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit
{
    public abstract class Test : Disposable
    {
        protected Test(ITestOutputHelper output = null)
        {
            TestOutput = output;
        }


        protected ITestOutputHelper TestOutput { get; }

        protected bool HasTestOutputEnabled => TestOutput != null;

        protected override void OnDisposeManagedResources()
        {
        }
    }
}