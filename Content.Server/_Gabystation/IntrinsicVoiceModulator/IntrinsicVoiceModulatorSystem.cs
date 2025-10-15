using Content.Shared._Gabystation.MalfAi;
using Content.Shared._Gabystation.MalfAi.Components;
using Content.Shared._Gabystation.MalfAi.Events;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;

namespace Content.Server._Gabystation.IntrinsicVoiceModulator;

public sealed partial class IntrinsicVoiceModulatorSystem : SharedIntrinsicVoiceModulatorSystem
{
    [Dependency] private readonly IConfigurationManager _cfgManager = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _uiSystem = default!;
    [Dependency] private readonly IPrototypeManager _protoManager = default!;

    private int _maxNameLenght;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<IntrinsicVoiceModulatorComponent, OpenIntrinsicVoiceModulatorMenuEvent>(OnOpenVoiceModulatorMenu);

        Subs.BuiEvents<IntrinsicVoiceModulatorComponent>(IntrinsicVoiceModularUiKey.Key, subs =>
        {
            subs.Event<IntrinsicVoiceModulatorNameChangedMessage>(OnNameChangedMessage);
            subs.Event<IntrinsicVoiceModulatorJobIconChangedMessage>(OnJobIconChanged);
        });

        Subs.CVar(_cfgManager, CCVars.MaxNameLength, value => _maxNameLenght = value, true);
    }

    private void OnOpenVoiceModulatorMenu(Entity<IntrinsicVoiceModulatorComponent> ent, ref OpenIntrinsicVoiceModulatorMenuEvent ev)
    {
        if (_uiSystem.HasUi(ev.Performer, IntrinsicVoiceModularUiKey.Key))
            return;

        _uiSystem.OpenUi(ev.Performer, IntrinsicVoiceModularUiKey.Key, ev.Performer);
    }

    private void OnNameChangedMessage(Entity<IntrinsicVoiceModulatorComponent> ent, ref IntrinsicVoiceModulatorNameChangedMessage args)
    {
        // Isso nunca deveria acontecer com um cliente "não-hackaeado". Criar um popup falando que deu errado?
        if (args.Name.Length > _maxNameLenght)
            return;

        ent.Comp.VoiceName = args.Name;
    }

    private void OnJobIconChanged(Entity<IntrinsicVoiceModulatorComponent> ent, ref IntrinsicVoiceModulatorJobIconChangedMessage args)
    {
        if (!_protoManager.TryIndex(args.JobIconProtoId, out var proto)
            || !proto.AllowSelection)
            return;

        ent.Comp.JobIconId = proto.ID;
    }
}