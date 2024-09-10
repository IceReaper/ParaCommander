// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Font;
using MLEM.Graphics;
using MLEM.Maths;
using MLEM.Ui;
using MLEM.Ui.Style;
using ParaCommander.Guis;
using ParaCommander.Scenes;

//// TODO energy is not used yet (for skills)
//// TODO experience is unused yet (for level up. level up = boost stats)
//// TODO there are no passive skills yet (movement, regeneration, etc)
//// TODO weapon upgrade should allow the player to pick a weapon to upgrade
//// TODO spawn: make invincible for 3 seconds
//// TODO after death, wait 3 seconds before respawning
//// TODO game mode should get harder over time
//// TODO enemies should try to not stack into each other. Maybe orbit player?
//// TODO end-game score screen showing survival time.

/// <summary>Represents the main game logic.</summary>
public sealed class GameMain : IDisposable
{
    /// <summary>Gets the target width of the camera.</summary>
    public const int TargetWidth = 640;

    /// <summary>Gets the target height of the camera.</summary>
    public const int TargetHeight = 480;

    private readonly SpriteBatch spriteBatch;
    private readonly UiSystem uiSystem;

    /// <summary>Initializes a new instance of the <see cref="GameMain"/> class.</summary>
    /// <param name="game">The game engine api.</param>
    public GameMain(Game game)
    {
        this.Game = game;
        this.Settings = new Settings();

        this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

        this.RemapEffect = this.Game.Content.Load<Effect>("Effects/Remap");
        this.RemapEffect.Parameters["input_remap"].SetValue(300f);

        this.uiSystem = new UiSystem(this.Game, new UiStyle
        {
            Font = new GenericSpriteFont(this.Game.Content.Load<SpriteFont>("Fonts/PressStart2P")),
            PanelChildPadding = new Padding(0),
            PanelTexture = this.spriteBatch.GenerateTexture(Color.Black, Color.DimGray),
            ButtonTexture = this.spriteBatch.GenerateTexture(Color.Black, Color.DimGray),
            ButtonHoveredTexture = this.spriteBatch.GenerateTexture(Color.DimGray, Color.DimGray),
            ScrollBarBackground = this.spriteBatch.GenerateTexture(Color.Black, Color.DimGray),
            ScrollBarScrollerTexture = this.spriteBatch.GenerateTexture(Color.DimGray, Color.DimGray),
            ProgressBarTexture = this.spriteBatch.GenerateTexture(Color.Black, Color.White),
            SelectionIndicator = this.spriteBatch.GenerateTexture(Color.Transparent, Color.White),
        })
        {
            AutoScaleWithScreen = true,
            AutoScaleReferenceSize = new Point(TargetWidth, TargetHeight),
            SpriteBatchContext = new SpriteBatchContext(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                this.RemapEffect),
        };

        this.uiSystem.Add(nameof(MainMenu), new MainMenu(this));
    }

    /// <summary>Gets the game engine api.</summary>
    public Game Game { get; }

    /// <summary>Gets the settings.</summary>
    public Settings Settings { get; }

    /// <summary>Gets or sets the currently running world.</summary>
    public World? World { get; set; }

    /// <summary>Gets the remap effect.</summary>
    public Effect RemapEffect { get; }

    /// <summary>Updates the game.</summary>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    public void Update(GameTime gameTime)
    {
        this.World?.Update(gameTime);
        this.uiSystem.Update(gameTime);
    }

    /// <summary>Draws the game.</summary>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    public void Draw(GameTime gameTime)
    {
        this.Game.GraphicsDevice.Clear(Color.Black);

        this.uiSystem.Draw(gameTime, this.spriteBatch);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.uiSystem.Dispose();
        this.World?.Dispose();
        this.spriteBatch.Dispose();
        this.Game.Content.Unload();
    }
}
