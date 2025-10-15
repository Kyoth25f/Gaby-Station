using Content.Shared.Speech;
using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;

namespace Content.Shared._Gabystation.IntrinsicVoiceModulator.Components;

[RegisterComponent]
public sealed partial class IntrinsicVoiceModulatorComponent : Component
{
    [DataField]
    public string VoiceName = "";

    [DataField]
    public ProtoId<SpeechVerbPrototype>? VoiceMaskSpeechVerb;

    [DataField]
    public ProtoId<JobIconPrototype>? JobIconId;

    [DataField]
    public EntProtoId Action = "ActionChangeIntrinsicVoiceModulator";

    [DataField]
    public EntityUid? ActionEntity;
}
