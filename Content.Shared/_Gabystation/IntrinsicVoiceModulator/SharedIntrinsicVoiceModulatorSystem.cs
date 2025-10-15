using Content.Shared.Speech;
using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Gabystation.IntrinsicVoiceModulator;

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
    public ProtoId<SpeechVerbPrototype>? CurrentVerb { get; }
    public ProtoId<JobIconPrototype>? JobIcon { get; }

    public IntrinsicVoiceModulatorBoundUserInterfaceState(string currentName, ProtoId<SpeechVerbPrototype>? currentVerb, ProtoId<JobIconPrototype>? jobIcon)
    {
        CurrentName = currentName;
        CurrentVerb = currentVerb;
        JobIcon = jobIcon;
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

public sealed class IntrinsicVoicemodulatorVerbChangedMessage : BoundUserInterfaceMessage
{
    public ProtoId<SpeechVerbPrototype>? SpeechProtoId { get; }

    public IntrinsicVoicemodulatorVerbChangedMessage(ProtoId<SpeechVerbPrototype>? speechProtoId)
    {
        SpeechProtoId = speechProtoId;
    }
}
