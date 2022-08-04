using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JsonConverterTest;

public class InterfaceConverter<TInterface, TImplement> : JsonConverter<TInterface> where TImplement : TInterface
{
    public override TInterface? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (TInterface?)JsonSerializer.Deserialize(ref reader, typeof(TImplement), options);
    }

    public override void Write(Utf8JsonWriter writer, TInterface value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, typeof(TImplement), options);
    }
}