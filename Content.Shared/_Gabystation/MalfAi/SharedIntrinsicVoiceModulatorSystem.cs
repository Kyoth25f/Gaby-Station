using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Gabystation.MalfAi;

public abstract class SharedIntrinsicVoiceModulatorSystem : EntitySystem;

[Serializable, NetSerializable]
public enum IntrinsicVoiceModularUiKey : byte
{
    Key,
}

[Serializable, NetSerializable]
public sealed class IntrinsicVoiceModulatorBoundUserInterfaceState : BoundUserInterfaceState
{
    public string CurrentName { get; }

    public IntrinsicVoiceModulatorBoundUserInterfaceState(string currentName)
    {
        CurrentName = currentName;
    }
}

public sealed class IntrinsicVoiceModulatorNameChangedMessage : BoundUserInterfaceMessage
{
    public string Name { get; }

    public IntrinsicVoiceModulatorNameChangedMessage(string name)
    {
        Name = name;
    }
}

public sealed class IntrinsicVoiceModulatorJobIconChangedMessage : BoundUserInterfaceMessage
{
    public ProtoId<JobIconPrototype> JobIconProtoId { get; }

    public IntrinsicVoiceModulatorJobIconChangedMessage(ProtoId<JobIconPrototype> jobIconProtoId)
    {
        JobIconProtoId = jobIconProtoId;
    }
}
