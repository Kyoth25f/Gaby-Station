using Content.Shared._Gabystation.IntrinsicVoiceModulator;
using Content.Shared._Gabystation.IntrinsicVoiceModulator.Components;
using Content.Shared._Gabystation.MalfAi.Events;
using Content.Shared.Administration.Logs;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Content.Shared.Popups;
using Robust.Server.GameObjects;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;

namespace Content.Server._Gabystation.IntrinsicVoiceModulator;

public sealed partial class IntrinsicVoiceModulatorSystem : SharedIntrinsicVoiceModulatorSystem
{
    [Dependency] private readonly IConfigurationManager _cfgManager = default!;
    [Dependency] private readonly IPrototypeManager _protoManager = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLog = default!;

    private int _maxNameLenght;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<IntrinsicVoiceModulatorComponent, OpenIntrinsicVoiceModulatorMenuEvent>(OnOpenVoiceModulatorMenu);

        Subs.BuiEvents<IntrinsicVoiceModulatorComponent>(IntrinsicVoiceModularUiKey.Key, subs =>
        {
            subs.Event<IntrinsicVoiceModulatorNameChangedMessage>(OnNameChangedMessage);
            subs.Event<IntrinsicVoiceModulatorJobIconChangedMessage>(OnJobIconChanged);
            subs.Event<IntrinsicVoicemodulatorVerbChangedMessage>(OnVerbChangeMessage);
        });

        Subs.CVar(_cfgManager, CCVars.MaxNameLength, value => _maxNameLenght = value, true);
    }

    private void OnOpenVoiceModulatorMenu(Entity<IntrinsicVoiceModulatorComponent> ent, ref OpenIntrinsicVoiceModulatorMenuEvent ev)
    {
        if (!UiSystem.HasUi(ev.Performer, IntrinsicVoiceModularUiKey.Key))
            return;


        UiSystem.OpenUi(ev.Performer, IntrinsicVoiceModularUiKey.Key, ev.Performer);
    }

    private void OnNameChangedMessage(Entity<IntrinsicVoiceModulatorComponent> ent, ref IntrinsicVoiceModulatorNameChangedMessage args)
    {
        if (args.Name.Length > _maxNameLenght
            || args.Name.Length == 0)
        {
            _popup.PopupEntity(Loc.GetString("intrinsic-voice-modulator-popup-failure"), ent, args.Actor, PopupType.SmallCaution);
            return;
        }

        ent.Comp.VoiceName = args.Name;

        _adminLog.Add(LogType.Action, LogImpact.Medium, $"{ToPrettyString(args.Actor):player} set them voice: {ent.Comp.VoiceName}");

        _popup.PopupEntity(Loc.GetString("intrinsic-voice-modulator-popup-success"), ent, args.Actor);
        UpdateUi(ent);
    }

    private void OnVerbChangeMessage(Entity<IntrinsicVoiceModulatorComponent> ent, ref IntrinsicVoicemodulatorVerbChangedMessage args)
    {
        if (args.SpeechProtoId is not { } speechProtoId
            || _protoManager.HasIndex(speechProtoId))
            return;

        ent.Comp.VoiceMaskSpeechVerb = speechProtoId;

        _popup.PopupEntity(Loc.GetString("intrinsic-voice-modulator-popup-success"), ent, args.Actor);
        UpdateUi(ent);
    }

    private void OnJobIconChanged(Entity<IntrinsicVoiceModulatorComponent> ent, ref IntrinsicVoiceModulatorJobIconChangedMessage args)
    {
        if (!_protoManager.TryIndex(args.JobIconProtoId, out var proto)
            || !proto.AllowSelection)
            return;

        ent.Comp.JobIconId = proto.ID;

        _popup.PopupEntity(Loc.GetString("intrinsic-voice-modulator-popup-success"), ent, args.Actor);
        UpdateUi(ent);
    }

    private void UpdateUi(Entity<IntrinsicVoiceModulatorComponent> ent)
    {
        var (uid, comp) = ent;

        if (!UiSystem.IsUiOpen(uid, IntrinsicVoiceModularUiKey.Key))
            return;

        var buiState = new IntrinsicVoiceModulatorBoundUserInterfaceState(comp.VoiceName, comp.VoiceMaskSpeechVerb, comp.JobIconId);

        UiSystem.SetUiState(uid, IntrinsicVoiceModularUiKey.Key, buiState);
    }
}