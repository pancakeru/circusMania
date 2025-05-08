using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DictionaryConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Dictionary<string, object>);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        Dictionary<string, object> dict = new Dictionary<string, object>();
        foreach (JProperty prop in jo.Properties())
        {
            dict.Add(prop.Name, prop.Value.ToObject<object>());
        }
        return dict;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Dictionary<string, object> dict = (Dictionary<string, object>)value;
        writer.WriteStartObject();
        foreach (KeyValuePair<string, object> kvp in dict)
        {
            writer.WritePropertyName(kvp.Key);
            serializer.Serialize(writer, kvp.Value);
        }
        writer.WriteEndObject();
    }
}
