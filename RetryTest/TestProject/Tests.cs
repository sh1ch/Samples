using NUnit.Framework;
using RetryTest;
using System;

namespace TestProject;
public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test_Action()
    {
        Assert.ThrowsAsync<Exception>(() => Retry.InvokeAsync(TestAction));
    }

    [Test]
    public void Test_Func()
    {
        Assert.ThrowsAsync<Exception>(() => Retry.InvokeAsync(TestFunc));
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