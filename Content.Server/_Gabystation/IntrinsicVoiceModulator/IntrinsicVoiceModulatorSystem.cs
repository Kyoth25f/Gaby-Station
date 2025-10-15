using System.Diagnostics.CodeAnalysis;
using Content.Shared._Gabystation.IntrinsicVoiceModulator;
using Content.Shared._Gabystation.IntrinsicVoiceModulator.Components;
using Content.Shared._Gabystation.MalfAi.Events;
using Content.Shared.Administration.Logs;
using Content.Shared.CCVar;
using Content.Shared.Chat;
using Content.Shared.Database;
using Content.Shared.Inventory;
using Content.Shared.Popups;
using Content.Shared.Roles;
using Content.Shared.StatusIcon;
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

        SubscribeLocalEvent<IntrinsicVoiceModulatorComponent, TransformSpeakerNameEvent>(OnTransformSpeakerName);
        SubscribeLocalEvent<IntrinsicVoiceModulatorComponent, TransformSpeakerJobIconEvent>(OnTransformJobIcon);
        SubscribeLocalEvent<IntrinsicVoiceModulatorComponent, OpenIntrinsicVoiceModulatorMenuEvent>(OnOpenVoiceModulatorMenu);

        Subs.BuiEvents<IntrinsicVoiceModulatorComponent>(IntrinsicVoiceModularUiKey.Key, subs =>
        {
            subs.Event<IntrinsicVoiceModulatorNameChangedMessage>(OnNameChangedMessage);
            subs.Event<IntrinsicVoiceModulatorJobIconChangedMessage>(OnJobIconChanged);
            subs.Event<IntrinsicVoicemodulatorVerbChangedMessage>(OnVerbChangeMessage);
        });

        Subs.CVar(_cfgManager, CCVars.MaxNameLength, value => _maxNameLenght = value, true);
    }

    private void OnTransformSpeakerName(Entity<IntrinsicVoiceModulatorComponent> ent, ref TransformSpeakerNameEvent args)
    {
        if (!string.IsNullOrWhiteSpace(ent.Comp.VoiceName))
            args.VoiceName = ent.Comp.VoiceName;

        if (ent.Comp.VoiceMaskSpeechVerb is { } speechVerb)
            args.SpeechVerb = speechVerb;
    }

    private void OnTransformJobIcon(Entity<IntrinsicVoiceModulatorComponent> ent, ref TransformSpeakerJobIconEvent args)
    {
        if (ent.Comp.JobIconId is { } jobIcon)
            args.JobIcon = jobIcon;

        if (!string.IsNullOrWhiteSpace(ent.Comp.JobName))
            args.JobName = ent.Comp.JobName;
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
            || !_protoManager.HasIndex(speechProtoId))
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

        if (TryFindJobProtoFromIcon(proto, out var job))
            ent.Comp.JobName = job.LocalizedName;

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

    private bool TryFindJobProtoFromIcon(JobIconPrototype jobIcon, [NotNullWhen(true)] out JobPrototype? job)
    {
        foreach (var jobPrototype in _protoManager.EnumeratePrototypes<JobPrototype>())
        {
            if (jobPrototype.Icon == jobIcon.ID)
            {
                job = jobPrototype;
                return true;
            }
        }

        job = null;
        return false;
    }
}