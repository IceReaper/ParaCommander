// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Scenes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>Represents the background of the game.</summary>
public class Background
{
    private readonly Texture2D texture;

    /// <summary>Initializes a new instance of the <see cref="Background"/> class.</summary>
    /// <param name="game">The game engine api.</param>
    public Background(Game game)
    {
        this.texture = game.Content.Load<Texture2D>("Images/Background");
    }

    /// <summary>Draw the background.</summary>
    /// <param name="spriteBatch">The sprite batch to draw with.</param>
    /// <param name="camera">The camera to draw with.</param>
    public void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        var startX = MathF.Floor(camera.Viewport.Left / (float)this.texture.Width);
        var startY = MathF.Floor(camera.Viewport.Top / (float)this.texture.Height);
        var endX = MathF.Ceiling(camera.Viewport.Right / (float)this.texture.Width);
        var endY = MathF.Ceiling(camera.Viewport.Bottom / (float)this.texture.Height);

        for (var y = startY; y < endY; y++)
        {
            for (var x = startX; x < endX; x++)
            {
                spriteBatch.Draw(this.texture, new Vector2(x * this.texture.Width, y * this.texture.Height), Color.White);
            }
        }
    }
}
