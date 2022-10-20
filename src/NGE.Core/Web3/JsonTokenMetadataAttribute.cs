using System.Text.Json.Serialization;

namespace NGE.Core.Web3;

public sealed class JsonTokenMetadataAttribute
{
    [JsonPropertyName("trait_type")]
    public string? TraitType { get; set; }

    [JsonPropertyName("value")]
    public object? Value { get; set; }
}