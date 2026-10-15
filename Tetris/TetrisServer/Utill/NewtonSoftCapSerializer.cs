using System;
using System.Text;
using System.Threading.Tasks;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TetrisServer.Utill;

public class NewtonSoftCapSerializer : ISerializer
{
    public Message Deserialize(string json)
    {
        return JsonConvert.DeserializeObject<Message>(json);
    }

    public object Deserialize(object value, Type valueType)
    {
        // CAP kan soms al een JToken (zoals JObject) doorgeven vanuit interne caching.
        // Als het al een JToken is, mappen we deze naar het doeltype, anders parsen we het als string.
        if (value is JToken jToken)
        {
            return jToken.ToObject(valueType);
        }

        if (value is string json)
        {
            return JsonConvert.DeserializeObject(json, valueType);
        }

        return value;
    }

    public ValueTask<Message> DeserializeAsync(TransportMessage transportMessage, Type valueType)
    {
        if (transportMessage.Body.IsEmpty || transportMessage.Body.Length == 0)
        {
            return new ValueTask<Message>(new Message(transportMessage.Headers, null));
        }

        // Haal de JSON string uit de bytes van het transportbericht
        string json = Encoding.UTF8.GetString(transportMessage.Body.ToArray());

        // Deserialiseer de object-payload met het juiste type
        object value = JsonConvert.DeserializeObject(json, valueType);

        // Retourneer het volledige CAP Message object
        var message = new Message(transportMessage.Headers, value);
        return new ValueTask<Message>(message);
    }

    public bool IsJsonType(object jsonObject)
    {
        return jsonObject is JToken;
    }

    public string Serialize(Message message)
    {
        return JsonConvert.SerializeObject(message);
    }

    public ValueTask<TransportMessage> SerializeAsync(Message message)
    {
        if (message.Value == null)
        {
            return new ValueTask<TransportMessage>(new TransportMessage(message.Headers, null));
        }

        // In CAP wordt alleen de *Value* (de payload) van het Message object over de message broker verzonden
        var json = JsonConvert.SerializeObject(message.Value);
        var bytes = Encoding.UTF8.GetBytes(json);

        var transportMessage = new TransportMessage(message.Headers, bytes);
        return new ValueTask<TransportMessage>(transportMessage);
    }
}
