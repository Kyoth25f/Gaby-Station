using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Gabystation.VoiceMask;

[Serializable, NetSerializable]
public sealed class VoiceMaskChangeJobIconMessage : BoundUserInterfaceMessage
{
    public ProtoId<JobIconPrototype> JobIconProtoId { get; }

    public VoiceMaskChangeJobIconMessage(ProtoId<JobIconPrototype> jobIconProtoId)
    {
        JobIconProtoId = jobIconProtoId;
    }
}