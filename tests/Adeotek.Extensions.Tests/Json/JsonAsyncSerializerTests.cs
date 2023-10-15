using Adeotek.Extensions.Json;

namespace Adeotek.Extensions.Tests.Json;

public class JsonAsyncSerializerTests
{
    private const string ExpectedResult = "{\"foo\":\"Hello\",\"bar\":\"World\",\"notDefault\":true}";
    
    [Fact]
    public async Task SerializeAsync_WithObjectType_ReturnString()
    {
        var result = await JsonAsyncSerializer.SerializeAsync((object)new JsonTestClass());
        
        Assert.Equal(ExpectedResult, result);
    }
   
    [Fact]
    public async Task SerializeAsync_WithGenericType_ReturnString()
    {
        var result = await JsonAsyncSerializer.SerializeAsync(new JsonTestClass());
        
        Assert.Equal(ExpectedResult, result);
    }
    
    [Fact]
    public void Serialize_WithGenericType_ReturnString()
    {
        var result = JsonAsyncSerializer.Serialize(new JsonTestClass());
        
        Assert.Equal(ExpectedResult, result);
    }
    
    private class JsonTestClass
    {
        public string Foo { get; set; } = "Hello";
        
        public string Bar { get; set; } = "World";
        
        public bool NotDefault { get; set; } = true;
        
        public bool IgnoreAsDefault { get; set; }

        public string? IgnoredAsNull { get; set; }
    }
}