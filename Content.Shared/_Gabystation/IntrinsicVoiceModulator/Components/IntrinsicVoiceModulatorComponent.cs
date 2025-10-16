using Content.Shared.Alert;
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
    public ProtoId<SpeechVerbPrototype>? SpeechVerbProtoId;

    [DataField]
    public ProtoId<JobIconPrototype>? JobIconProtoId;

    [DataField]
    public string? JobName;

    [DataField]
    public EntProtoId ActionProtoId = "ActionChangeIntrinsicVoiceModulator";

    [DataField]
    public EntityUid? ActionEntity;

    [DataField]
    public ProtoId<AlertPrototype> ToggleAlertProtoId = "IntrinsicVoiceModulator";

    [DataField]
    public bool Enabled;
}
