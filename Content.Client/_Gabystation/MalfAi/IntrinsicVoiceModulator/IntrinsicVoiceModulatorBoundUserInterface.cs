// SPDX-FileCopyrightText: 2025 Dreykor <160512778+Dreykor@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 GabyChangelog <agentepanela2@gmail.com>
// SPDX-FileCopyrightText: 2025 Kyoth25f <kyoth25f@gmail.com>
// SPDX-FileCopyrightText: 2025 Tyranex <bobthezombie4@gmail.com>
// SPDX-FileCopyrightText: 2025 funkystationbot <funky@funkystation.org>
//
// SPDX-License-Identifier: MIT

using Content.Client.MalfAI.Theme;
using Content.Shared._Gabystation.MalfAi;
using Content.Shared.MalfAI;
using Content.Shared.StatusIcon;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;

namespace Content.Client._Gabystation.MalfAI.IntrinsicVoiceModulator;

public sealed class IntrinsicVoiceModulatorBoundUserInterface : BoundUserInterface
{
    private IntrinsicVoiceModulatorWindow? _window;

    public IntrinsicVoiceModulatorBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey) { }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<IntrinsicVoiceModulatorWindow>();

        _window.OnNameChanged += OnNameChanged;
        _window.OnJobIconChanged += OnJobIconChanged;
    }

    private void OnNameChanged(string newName)
    {
        SendMessage(new IntrinsicVoiceModulatorNameChangedMessage(newName));
    }

    public void OnJobIconChanged(ProtoId<JobIconPrototype> newJobIconId)
    {
        SendMessage(new IntrinsicVoiceModulatorJobIconChangedMessage(newJobIconId));
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (_window is null
            || state is not IntrinsicVoiceModulatorBoundUserInterfaceState cast)
            return;

        _window.SetCurrentName(cast.CurrentName);
    }
}

