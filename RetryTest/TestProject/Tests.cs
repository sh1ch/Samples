using NUnit.Framework;
using RetryTest;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject;
public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test_Action()
    {
        try
        {
            await Retry.InvokeAsync(TestAction);
        }
        catch (AggregateException e)
        {
            Assert.AreEqual(e.Flatten().InnerExceptions.Where(p => p is Exception).Count(), 4);
        }
    }

    [Test]
    public void Test_Func()
    {
        Assert.ThrowsAsync<AggregateException>(() => Retry.InvokeAsync(TestFunc));
    }

    public void TestAction()
    {
        throw new Exception();
    }

    public bool TestFunc()
    {
        throw new Exception();
    }
}