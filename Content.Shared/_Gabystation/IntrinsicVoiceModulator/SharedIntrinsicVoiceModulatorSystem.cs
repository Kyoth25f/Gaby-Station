using Content.Shared._Gabystation.IntrinsicVoiceModulator.Components;
using Content.Shared.Actions;
using Content.Shared.Speech;
using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Gabystation.IntrinsicVoiceModulator;

// Isso não está rodando no cliente propositalmente. Talvez mandar tudo pro servidor, então?
public abstract class SharedIntrinsicVoiceModulatorSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] protected readonly SharedUserInterfaceSystem UiSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<IntrinsicVoiceModulatorComponent, ComponentShutdown>(OnComponentShutdown);
        SubscribeLocalEvent<IntrinsicVoiceModulatorComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(Entity<IntrinsicVoiceModulatorComponent> ent, ref MapInitEvent args)
    {
        var action = _actions.AddAction(ent.Owner, ent.Comp.Action);
        ent.Comp.ActionEntity = action;

        // Isso ser hardcoded é terrível, mas acho que não tem outra forma de conseguir o nome da classe. Ela vive no cliente, não daria pra pegar no servidor.
        var data = new InterfaceData("IntrinsicVoiceModulatorBoundUserInterface");
        UiSystem.SetUi(ent.Owner, IntrinsicVoiceModularUiKey.Key, data);
    }

    private void OnComponentShutdown(Entity<IntrinsicVoiceModulatorComponent> ent, ref ComponentShutdown args)
    {
        _actions.RemoveAction(ent.Owner, ent.Comp.ActionEntity);
    }
}

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

[NetSerializable, Serializable]
public sealed class IntrinsicVoiceModulatorNameChangedMessage : BoundUserInterfaceMessage
{
    public string Name { get; }

    public IntrinsicVoiceModulatorNameChangedMessage(string name)
    {
        Name = name;
    }
}

[NetSerializable, Serializable]
public sealed class IntrinsicVoiceModulatorJobIconChangedMessage : BoundUserInterfaceMessage
{
    public ProtoId<JobIconPrototype> JobIconProtoId { get; }

    public IntrinsicVoiceModulatorJobIconChangedMessage(ProtoId<JobIconPrototype> jobIconProtoId)
    {
        JobIconProtoId = jobIconProtoId;
    }
}

[NetSerializable, Serializable]
public sealed class IntrinsicVoicemodulatorVerbChangedMessage : BoundUserInterfaceMessage
{
    public ProtoId<SpeechVerbPrototype>? SpeechProtoId { get; }

    public IntrinsicVoicemodulatorVerbChangedMessage(ProtoId<SpeechVerbPrototype>? speechProtoId)
    {
        SpeechProtoId = speechProtoId;
    }
}
