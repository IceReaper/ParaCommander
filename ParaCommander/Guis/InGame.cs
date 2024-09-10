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
using MLEM.Input;
using MLEM.Maths;
using MLEM.Ui;
using MLEM.Ui.Elements;
using ParaCommander.Components;
using ParaCommander.Enums;
using ParaCommander.GameModes;
using ParaCommander.Scenes;

/// <summary>Represents the in-game ui.</summary>
public sealed class InGame : Panel
{
    private readonly Paragraph lives;

    /// <summary>Initializes a new instance of the <see cref="InGame"/> class.</summary>
    /// <param name="gameMain">The GameMain instance.</param>
    /// <param name="primaryPlayer">The primary player input method.</param>
    public InGame(GameMain gameMain, InputMethod primaryPlayer)
        : base(Anchor.TopLeft, Vector2.Zero)
    {
        this.GameMain = gameMain;
        this.GameMain.World = this.World = new World(this.GameMain) { GameMode = new EndlessWaveGameMode { Lives = 4, WaveInterval = TimeSpan.FromSeconds(4) } };

        this.Priority = 0;
        this.Texture = null;

        this.Players.Add(new PlayerSlot(Anchor.TopLeft, this, Color.Blue));
        this.Players.Add(new PlayerSlot(Anchor.TopRight, this, Color.Red));
        this.Players.Add(new PlayerSlot(Anchor.BottomLeft, this, Color.Green));
        this.Players.Add(new PlayerSlot(Anchor.BottomRight, this, Color.Yellow));

        this.AddChild(this.lives = new Paragraph(Anchor.TopCenter, Grid.Size(24), " ") { Alignment = TextAlignment.Center, Padding = new Padding(Grid.Size(1)) });

        foreach (var player in this.Players)
        {
            this.AddChild(player);
        }

        this.Players[0].Join(primaryPlayer);
    }

    /// <summary>Gets the GameMain instance.</summary>
    public GameMain GameMain { get; }

    /// <summary>Gets the world.</summary>
    public World World { get; }

    /// <summary>Gets the player slots.</summary>
    public IList<PlayerSlot> Players { get; } = [];

    /// <summary>Gets the players center.</summary>
    public Vector2 PlayersCenter { get; private set; }

    /// <inheritdoc />
    public override void Update(GameTime time)
    {
        base.Update(time);

        this.lives.Text = $"Lives: {this.World.GameMode.Lives}";

        this.Size = this.System.Viewport.Size.ToVector2() / this.System.GlobalScale;

        var consumeA = this.Input.TryConsumePressed(new GenericInput(Keys.Escape));
        var consumeB = this.Input.TryConsumePressed(new GenericInput(Buttons.Back));

        if (consumeA || consumeB)
        {
            this.OpenMenu();
            return;
        }

        if (this.World.Paused)
        {
            return;
        }

        if (this.Input.IsPressed(new GenericInput(Keys.Enter)))
        {
            this.TryJoin(InputMethod.KeyboardAndMouse);
        }

        for (var i = 0; i < 4; i++)
        {
            if (this.Input.IsPressed(new GenericInput(Buttons.Start), i))
            {
                this.TryJoin((InputMethod)(i + 1));
            }
        }
    }

    /// <inheritdoc />
    public override void Draw(GameTime time, SpriteBatch batch, float alpha, SpriteBatchContext context)
    {
        this.UpdatePlayersCenter();

        batch.End();

        this.World.Camera.Update(this.GameMain.Game.GraphicsDevice.Viewport.Bounds, this.PlayersCenter);
        this.World.Draw(time);

        batch.Begin(context);

        base.Draw(time, batch, alpha, context);
    }

    private void OpenMenu()
    {
        this.World.Paused = true;
        this.System.Add(nameof(MainMenu), new MainMenu(this.GameMain));
    }

    private void UpdatePlayersCenter()
    {
        var players = this.World.Entities.Where(entity => entity.GetOneOrDefault<PlayerComponent>() != null).ToList();

        if (players is not { Count: > 0 })
        {
            return;
        }

        var minX = float.PositiveInfinity;
        var minY = float.PositiveInfinity;
        var maxX = float.NegativeInfinity;
        var maxY = float.NegativeInfinity;

        foreach (var player in players)
        {
            minX = Math.Min(minX, player.Position.X);
            minY = Math.Min(minY, player.Position.Y);
            maxX = Math.Max(maxX, player.Position.X);
            maxY = Math.Max(maxY, player.Position.Y);
        }

        this.PlayersCenter = new Vector2(minX + maxX, minY + maxY) / 2;
    }

    private void TryJoin(InputMethod inputMethod)
    {
        if (this.Players.Any(player => player.InputMethod == inputMethod))
        {
            return;
        }

        this.Players.FirstOrDefault(player => player.InputMethod == null)?.Join(inputMethod);
    }
}
