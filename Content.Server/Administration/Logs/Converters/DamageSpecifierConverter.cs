using Content.Shared.Damage;
using System.Text.Json;

namespace Content.Server.Administration.Logs.Converters;

[AdminLogConverter]
public sealed class DamageSpecifierConverter : AdminLogConverter<DamageSpecifier>
{
    public override void Write(Utf8JsonWriter writer, DamageSpecifier value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        foreach (var (damage, amount) in value.DamageDict)
        {
            writer.WriteNumber(damage.Id, amount.Double());
        }
        writer.WriteEndObject();
    }
}
