using Domain;

namespace UnitTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Class1 obj = new();

        int result = obj.GetNum;

        Assert.Equal(1, result);
    }
}