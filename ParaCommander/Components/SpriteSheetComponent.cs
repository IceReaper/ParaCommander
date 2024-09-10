// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParaCommander.Databases;
using ParaCommander.Interfaces;
using ParaCommander.Scenes;

/// <summary>Gives the entity a visible spritesheet.</summary>
public class SpriteSheetComponent : Component, IDraw
{
    private const int Fps = 12;

    private Texture2D? texture;
    private int width;
    private int height;
    private Vector2 center;
    private float animationProgress;

    /// <summary>Gets the entry for this SpriteSheet.</summary>
    public required SpriteSheetEntry Entry { get; init; }

    /// <summary>Gets or sets the color of the SpriteSheet.</summary>
    public Color? Color { get; set; }

    /// <summary>Gets a value indicating whether the animation has finished.</summary>
    public bool Finished => this.animationProgress * Fps >= this.Entry.Frames;

    /// <inheritdoc />
    public void PrepareDraw(GameTime gameTime)
    {
        if (this.texture == null)
        {
            this.texture = this.Entity.World.GameMain.Game.Content.Load<Texture2D>($"SpriteSheets/{this.Entry.Path}");
            this.width = this.texture.Width / this.Entry.Frames;
            this.height = this.texture.Height;
            this.center = new Vector2(this.width, this.height) / 2;
        }

        if (!this.Entity.World.Paused)
        {
            this.animationProgress += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    /// <inheritdoc />
    public void Draw(SpriteBatch spriteBatch, Effect remap)
    {
        var frame = (int)(this.animationProgress * Fps) % this.Entry.Frames;
        var source = new Rectangle(frame * this.width, 0, this.width, this.height);
        var target = new Rectangle((int)this.Entity.Position.X, (int)this.Entity.Position.Y, this.width, this.height);
        var rotation = (float)Math.Atan2(this.Entity.Direction.Y, this.Entity.Direction.X) + (MathF.PI / 2);

        if (this.Color != null)
        {
            remap.Parameters["input_color"].SetValue(this.Color.Value.ToVector4());
        }

        spriteBatch.Draw(this.texture, target, source, Microsoft.Xna.Framework.Color.White, rotation, this.center, SpriteEffects.None, 0);
    }
}
