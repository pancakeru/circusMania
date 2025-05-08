using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DictionaryConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        // Check if the type is a generic Dictionary<,>
        return objectType.IsGenericType &&
               objectType.GetGenericTypeDefinition() == typeof(Dictionary<,>);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        // Create a JObject from the JSON
        JObject jo = JObject.Load(reader);
        IDictionary dict = (IDictionary)Activator.CreateInstance(objectType);

        foreach (JProperty prop in jo.Properties())
        {
            // Get the key and value types of the dictionary
            Type keyType = objectType.GetGenericArguments()[0];
            Type valueType = objectType.GetGenericArguments()[1];

            // Deserialize the key and value
            object key = prop.Name; // Keys in JSON are always strings, so convert if necessary
            object value = prop.Value.ToObject(valueType, serializer);

            // Add the entry to the dictionary
            dict.Add(key, value);
        }

        return dict;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        IDictionary dict = (IDictionary)value;
        writer.WriteStartObject();

        foreach (DictionaryEntry entry in dict)
        {
            // Convert the key to a string (JSON keys are always strings)
            string key = Convert.ToString(entry.Key);
            writer.WritePropertyName(key);
            serializer.Serialize(writer, entry.Value);
        }

        writer.WriteEndObject();
    }
}
