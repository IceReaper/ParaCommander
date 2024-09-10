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
using ParaCommander.Enums;

/// <summary>Represents the main menu.</summary>
public sealed class MainMenu : Panel
{
    private readonly GameMain gameMain;

    private readonly Button start;
    private readonly Button stop;
    private readonly Button exit;
    private readonly Button back;

    /// <summary>Initializes a new instance of the <see cref="MainMenu"/> class.</summary>
    /// <param name="gameMain">The GameMain instance.</param>
    public MainMenu(GameMain gameMain)
        : base(Anchor.Center, Vector2.Zero)
    {
        this.gameMain = gameMain;

        this.Priority = 1;
        this.SetHeightBasedOnChildren = true;
        this.SetWidthBasedOnChildren = true;

        this.ChildPadding = new Padding(Grid.Size(1));

        this.AddChild(this.start = new Button(Anchor.AutoLeft, Grid.Size(32, 4), "Start")
        {
            AutoNavGroup = nameof(MainMenu),
            OnPressed = _ => this.StartGame(),
        });

        this.AddChild(this.stop = new Button(Anchor.AutoLeft, Grid.Size(32, 4), "Stop")
        {
            AutoNavGroup = nameof(MainMenu),
            OnPressed = _ => this.StopGame(),
        });

        var initialItem = this.gameMain.World == null ? this.start : this.stop;

        this.AddChild(new VerticalSpace(Grid.Size(1)));

        this.AddChild(new Button(Anchor.AutoLeft, Grid.Size(32, 4), "Options")
        {
            AutoNavGroup = nameof(MainMenu),
            OnPressed = _ => this.OpenOptions(),
        });

        this.AddChild(new VerticalSpace(Grid.Size(1)));

        this.AddChild(this.exit = new Button(Anchor.AutoLeft, Grid.Size(32, 4), "Exit")
        {
            AutoNavGroup = nameof(MainMenu),
            OnPressed = _ => this.Exit(),
        });

        this.AddChild(this.back = new Button(Anchor.AutoLeft, Grid.Size(32, 4), "Back")
        {
            AutoNavGroup = nameof(MainMenu),
            OnPressed = _ => this.Back(),
        });

        this.UpdateUi();

        this.OnAddedToUi += _ => this.Root.SelectElement(initialItem, null, UiControls.NavigationType.Gamepad);
    }

    /// <inheritdoc/>
    public override void Update(GameTime time)
    {
        base.Update(time);

        if (!(this.gameMain.World?.Paused ?? false))
        {
            return;
        }

        var consumeA = this.Input.TryConsumePressed(new GenericInput(Keys.Escape));
        var consumeB = this.Input.TryConsumePressed(new GenericInput(Buttons.Back));

        if (consumeA || consumeB)
        {
            this.Back();
        }
    }

    private void UpdateUi()
    {
        if (this.gameMain.World == null)
        {
            this.start.IsHidden = false;
            this.stop.IsHidden = true;
            this.exit.IsHidden = false;
            this.back.IsHidden = true;
        }
        else
        {
            this.start.IsHidden = true;
            this.stop.IsHidden = false;
            this.exit.IsHidden = true;
            this.back.IsHidden = false;
        }
    }

    private void StartGame()
    {
        var primaryPlayer = InputMethod.KeyboardAndMouse;

        if (this.Controls.NavType == UiControls.NavigationType.Gamepad)
        {
            for (var i = 0; i < this.start.Controls.Input.ConnectedGamepads; i++)
            {
                if (!this.start.Controls.GamepadButtons.IsPressedAvailable(this.start.Controls.Input, i))
                {
                    continue;
                }

                primaryPlayer = (InputMethod)(i + 1);
                break;
            }
        }

        this.System.Add(nameof(InGame), new InGame(this.gameMain, primaryPlayer));
        this.System.Remove(nameof(MainMenu));
    }

    private void StopGame()
    {
        this.gameMain.World?.Dispose();
        this.gameMain.World = null;

        this.UpdateUi();
        this.System.Remove(nameof(InGame));
    }

    private void OpenOptions()
    {
        this.System.Add(nameof(OptionsMenu), new OptionsMenu(this.gameMain));
        this.System.Remove(nameof(MainMenu));
    }

    private void Exit()
    {
        this.gameMain.Game.Exit();
    }

    private void Back()
    {
        if (this.gameMain.World != null)
        {
            this.gameMain.World.Paused = false;
        }

        this.System.Remove(nameof(MainMenu));
    }
}
