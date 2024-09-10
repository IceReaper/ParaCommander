// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Guis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Formatting;
using MLEM.Graphics;
using MLEM.Maths;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using ParaCommander.Components;
using ParaCommander.Components.Items;
using ParaCommander.Databases;
using ParaCommander.Enums;
using ParaCommander.Scenes;

/// <summary>Represents a player slot.</summary>
public sealed class PlayerSlot : Panel
{
    private readonly InGame inGame;
    private readonly Color color;

    private readonly Image image;
    private readonly Panel content;
    private readonly ProgressBar health;
    private readonly ProgressBar energy;
    private readonly ProgressBar ammo;
    private readonly ProgressBar experience;
    private readonly Paragraph joinMessage;

    private Entity? player;

    /// <summary>Initializes a new instance of the <see cref="PlayerSlot"/> class.</summary>
    /// <param name="anchor">The panel's anchor.</param>
    /// <param name="inGame">The InGame ui.</param>
    /// <param name="color">The players color.</param>
    public PlayerSlot(Anchor anchor, InGame inGame, Color color)
        : base(anchor, Grid.Size(20, 4))
    {
        this.inGame = inGame;
        this.color = color;

        this.Padding = new Padding(-1);

        var texture = new TextureRegion(this.inGame.GameMain.Game.Content.Load<Texture2D>("Images/Player"));
        this.image = new Image(this.Anchor, new Vector2(0, 0), _ => texture, true) { PositionOffset = Grid.Size(.5f, .5f) };
        this.AddChild(this.image);

        var contentAlign = this.Anchor is Anchor.TopLeft or Anchor.BottomLeft ? Anchor.CenterRight : Anchor.CenterLeft;
        this.content = new Panel(contentAlign, Grid.Size(16, 4)) { IsHidden = true };
        this.AddChild(this.content);

        this.content.AddChild(this.health = new ProgressBar(Anchor.AutoLeft, Grid.Size(16, 1), Direction2.Right, 1, 1)
        {
            Color = Color.Black,
            ProgressColor = Color.Green,
        });

        this.content.AddChild(this.energy = new ProgressBar(Anchor.AutoLeft, Grid.Size(16, 1), Direction2.Right, 1, 1)
        {
            Color = Color.Black,
            ProgressColor = Color.Yellow,
        });

        this.content.AddChild(this.ammo = new ProgressBar(Anchor.AutoLeft, Grid.Size(16, 1), Direction2.Right, 1, 1)
        {
            Color = Color.Black,
            ProgressColor = Color.Red,
        });

        this.content.AddChild(this.experience = new ProgressBar(Anchor.AutoLeft, Grid.Size(16, 1), Direction2.Right, 1, 1)
        {
            Color = Color.Black,
            ProgressColor = Color.Blue,
        });

        this.AddChild(this.joinMessage = new Paragraph(Anchor.Center, Grid.Size(20), "Press Start") { Alignment = TextAlignment.Center });
    }

    /// <summary>Gets or sets the input method this player uses.</summary>
    public InputMethod? InputMethod { get; set; }

    /// <inheritdoc />
    public override void Update(GameTime time)
    {
        if (this.InputMethod == null)
        {
            return;
        }

        if (this.player == null)
        {
            // TODO Might be better placed in the GameMode
            if (this.inGame.World.GameMode.Lives == 0)
            {
                return;
            }

            this.inGame.World.GameMode.Lives--;

            this.player = this.inGame.World.Spawn(EntityDatabase.ShipPlayer);
            this.player.GetOne<SpriteSheetComponent>().Color = this.color;
            this.player.Position = this.inGame.PlayersCenter;

            this.inGame.World.Play("Spawn");

            return;
        }

        if (this.player.Disposed)
        {
            this.player = null;
        }

        var healthComponent = this.player?.GetOneOrDefault<HealthComponent>();

        this.health.MaxValue = healthComponent?.MaxHealth ?? 1;
        this.health.CurrentValue = healthComponent?.Health ?? 0;

        var playerComponent = this.player?.GetOneOrDefault<PlayerComponent>();

        this.energy.MaxValue = playerComponent?.MaxEnergy ?? 1;
        this.energy.CurrentValue = playerComponent?.Energy ?? 0;
        this.ammo.MaxValue = playerComponent?.MaxAmmo ?? 1;
        this.ammo.CurrentValue = playerComponent?.Ammo ?? 0;
        this.experience.MaxValue = playerComponent?.MaxExperience ?? 1;
        this.experience.CurrentValue = playerComponent?.Experience ?? 0;
    }

    /// <inheritdoc />
    public override void Draw(GameTime time, SpriteBatch batch, float alpha, SpriteBatchContext context)
    {
        this.inGame.GameMain.RemapEffect.Parameters["input_color"].SetValue(this.color.ToVector4());

        if (this.player is { Disposed: true })
        {
            this.player = null;
        }

        this.ProcessInput();

        base.Draw(time, batch, alpha, context);
    }

    /// <summary>Joins the player slot.</summary>
    /// <param name="inputMethod">The input method this player uses.</param>
    public void Join(InputMethod inputMethod)
    {
        this.InputMethod = inputMethod;

        this.content.IsHidden = false;
        this.joinMessage.IsHidden = true;
    }

    private void ProcessInput()
    {
        if (this.InputMethod == null || this.inGame.World.Paused || this.player == null)
        {
            return;
        }

        var movableComponent = this.player.GetOneOrDefault<MovableComponent>();
        var armamentComponent = this.player.GetOneOrDefault<ArmamentComponent>();

        if (!this.inGame.GameMain.Game.IsActive)
        {
            if (movableComponent != null)
            {
                movableComponent.Move = Vector2.Zero;
            }

            if (armamentComponent != null)
            {
                armamentComponent.IsFiring = false;
            }
        }
        else if (this.InputMethod == Enums.InputMethod.KeyboardAndMouse)
        {
            this.HandeKeyboardMouseInput(movableComponent, armamentComponent);
        }
        else
        {
            this.HandleControllerInput(movableComponent, armamentComponent);
        }
    }

    private void HandleControllerInput(MovableComponent? movable, ArmamentComponent? armament)
    {
        if (this.InputMethod == null)
        {
            return;
        }

        var controller = GamePad.GetState((int)this.InputMethod - 1);

        if (movable != null)
        {
            movable.Move = controller.ThumbSticks.Left * new Vector2(1, -1);
            movable.Look = controller.ThumbSticks.Right * new Vector2(1, -1);

            if (movable.Look.Length() != 0)
            {
                movable.Look = Vector2.Normalize(movable.Look);
            }
        }

        if (armament != null)
        {
            armament.IsFiring = controller.IsButtonDown(Buttons.RightTrigger);
        }
    }

    private void HandeKeyboardMouseInput(MovableComponent? movable, ArmamentComponent? armament)
    {
        if (this.player == null)
        {
            return;
        }

        var keyboard = Keyboard.GetState();
        var mouse = Mouse.GetState();

        if (movable != null)
        {
            movable.Move = new Vector2(
                (keyboard.IsKeyDown(Keys.A) ? -1 : 0) + (keyboard.IsKeyDown(Keys.D) ? 1 : 0),
                (keyboard.IsKeyDown(Keys.W) ? -1 : 0) + (keyboard.IsKeyDown(Keys.S) ? 1 : 0));

            movable.Look = mouse.Position.ToVector2() - this.inGame.World.Camera.WorldToScreen(this.player.Position);

            if (movable.Look.Length() != 0)
            {
                movable.Look = Vector2.Normalize(movable.Look);
            }
        }

        if (armament != null)
        {
            armament.IsFiring = mouse.LeftButton == ButtonState.Pressed;
        }
    }
}
