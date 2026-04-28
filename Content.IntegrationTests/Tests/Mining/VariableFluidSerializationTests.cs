using Content.Shared.Atmos;
using Content.Shared.MiningFluid.Components;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.UnitTesting.Shared.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using YamlDotNet.RepresentationModel;

namespace Content.IntegrationTests.Tests.Mining;

public sealed class VariableFluidSerializationTests : SerializationTest
{
    protected override Assembly[] Assemblies =>
    [
        typeof(VariableFluidDefinition).Assembly
    ];

    [Test]
    public async Task SerializeTest()
    {
        var variableMixture = new Dictionary<Gas, VariableFluidDefinition>
        {
            { Gas.Oxygen, new() { Moles = 100f, Probability = 0.5f } }
        };
        const string expectedResult =
"""
Oxygen:
  moles: 100
  prob: 0.5
...

""";
        var serializedNode = Serialization.WriteValue(variableMixture);
        Assert.That(serializedNode.ToString(), Is.EqualTo(expectedResult));

        var yamlStream = new YamlStream();
        yamlStream.Load(new StringReader(expectedResult));
        var mapping = yamlStream.Documents[0].RootNode.ToDataNodeCast<MappingDataNode>();
        var deserializedMix = Serialization.Read<Dictionary<Gas, VariableFluidDefinition>>(mapping);
        Assert.That(deserializedMix.ContainsKey(Gas.Oxygen), Is.True);
        Assert.That(deserializedMix[Gas.Oxygen].Moles, Is.EqualTo(100f));
        Assert.That(deserializedMix[Gas.Oxygen].Probability, Is.EqualTo(0.5f));
    }
}
