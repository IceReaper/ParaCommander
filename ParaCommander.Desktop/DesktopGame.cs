// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Desktop;

using Microsoft.Xna.Framework;

/// <summary>Wraps and initializes the MonoGame engine for desktop platforms.</summary>
public class DesktopGame : Game
{
    private readonly GraphicsDeviceManager graphicsDeviceManager;

    private GameMain? game;

    /// <summary>Initializes a new instance of the <see cref="DesktopGame"/> class.</summary>
    public DesktopGame()
    {
        this.graphicsDeviceManager = new GraphicsDeviceManager(this);
    }

    /// <inheritdoc />
    protected override void Initialize()
    {
        this.Content.RootDirectory = "Content";
        this.Window.AllowUserResizing = true;
        this.IsMouseVisible = true;
    }

    /// <inheritdoc />
    protected override void BeginRun()
    {
        this.game = new GameMain(this);
    }

    /// <inheritdoc />
    protected override void Update(GameTime gameTime)
    {
        this.game?.Update(gameTime);
    }

    /// <inheritdoc />
    protected override void Draw(GameTime gameTime)
    {
        this.game?.Draw(gameTime);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        this.graphicsDeviceManager.Dispose();
    }
}
