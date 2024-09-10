// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Guis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MLEM.Input;
using MLEM.Maths;
using MLEM.Ui;
using MLEM.Ui.Elements;

/// <summary>Represents the options menu.</summary>
public sealed class OptionsMenu : Panel
{
    private readonly GameMain gameMain;

    /// <summary>Initializes a new instance of the <see cref="OptionsMenu"/> class.</summary>
    /// <param name="gameMain">The GameMain instance.</param>
    public OptionsMenu(GameMain gameMain)
        : base(Anchor.Center, Vector2.Zero)
    {
        this.gameMain = gameMain;

        this.Priority = 1;
        this.SetHeightBasedOnChildren = true;
        this.SetWidthBasedOnChildren = true;
        this.ChildPadding = new Padding(Grid.Size(1));

        this.AddChild(new Paragraph(Anchor.AutoLeft, Grid.Size(32), "Master volume"));

        this.AddChild(new Slider(Anchor.AutoLeft, Grid.Size(32, 4), (int)Grid.Size(4), 1)
        {
            AutoNavGroup = nameof(OptionsMenu),
            StepPerScroll = 0.01f,
            CurrentValue = this.gameMain.Settings.MasterVolume,
            OnValueChanged = (_, value) => this.SetMasterVolume(value),
        });

        this.AddChild(new VerticalSpace(Grid.Size(1)));

        this.AddChild(new Paragraph(Anchor.AutoLeft, Grid.Size(32), "Sound volume"));

        this.AddChild(new Slider(Anchor.AutoLeft, Grid.Size(32, 4), (int)Grid.Size(4), 1)
        {
            AutoNavGroup = nameof(OptionsMenu),
            StepPerScroll = 0.01f,
            CurrentValue = this.gameMain.Settings.SoundVolume,
            OnValueChanged = (_, value) => this.SetSoundVolume(value),
        });

        this.AddChild(new VerticalSpace(Grid.Size(1)));

        this.AddChild(new Paragraph(Anchor.AutoLeft, Grid.Size(32), "Music volume"));

        this.AddChild(new Slider(Anchor.AutoLeft, Grid.Size(32, 4), (int)Grid.Size(4), 1)
        {
            AutoNavGroup = nameof(OptionsMenu),
            StepPerScroll = 0.01f,
            CurrentValue = this.gameMain.Settings.MusicVolume,
            OnValueChanged = (_, value) => this.SetMusicVolume(value),
        });

        this.AddChild(new VerticalSpace(Grid.Size(1)));

        var initialItem = new Button(Anchor.AutoLeft, Grid.Size(32, 4), "Back")
        {
            AutoNavGroup = nameof(OptionsMenu),
            OnPressed = _ => this.Back(),
        };

        this.AddChild(initialItem);

        this.OnAddedToUi += _ => this.Root.SelectElement(initialItem, null, UiControls.NavigationType.Gamepad);
    }

    /// <inheritdoc/>
    public override void Update(GameTime time)
    {
        base.Update(time);

        var consumeA = this.Input.TryConsumePressed(new GenericInput(Keys.Escape));
        var consumeB = this.Input.TryConsumePressed(new GenericInput(Buttons.Back));

        if (consumeA || consumeB)
        {
            this.Back();
        }
    }

    private void SetMasterVolume(float value)
    {
        this.gameMain.Settings.MasterVolume = value;
    }

    private void SetSoundVolume(float value)
    {
        this.gameMain.Settings.SoundVolume = value;
    }

    private void SetMusicVolume(float value)
    {
        this.gameMain.Settings.MusicVolume = value;
    }

    private void Back()
    {
        this.System.Add(nameof(MainMenu), new MainMenu(this.gameMain));
        this.System.Remove(nameof(OptionsMenu));
    }
}
