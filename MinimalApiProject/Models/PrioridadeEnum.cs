using System.Text.Json.Serialization;

namespace MinimalApiProject.Models
{ 
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PrioridadeEnum
{
    Baixa,
    Media,
    Alta
}

}