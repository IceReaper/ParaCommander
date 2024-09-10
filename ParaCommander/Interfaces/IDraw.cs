// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>Represents an object that can be drawn.</summary>
public interface IDraw
{
    /// <summary>Progress the animation before drawing.</summary>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    public void PrepareDraw(GameTime gameTime);

    /// <summary>Draw the component.</summary>
    /// <param name="spriteBatch">The sprite batch to draw with.</param>
    /// <param name="remap">The remap effect to apply.</param>
    public void Draw(SpriteBatch spriteBatch, Effect remap);
}
