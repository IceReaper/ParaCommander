// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using Microsoft.Xna.Framework;
using ParaCommander.Interfaces;
using ParaCommander.Scenes;

/// <summary>Represents a component that despawns an entity when it is far away from the players.</summary>
public class DespawnWhenLeftViewportComponent : Component, IUpdate
{
    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        var distance = Math.Max(this.Entity.World.Camera.Viewport.Width, this.Entity.World.Camera.Viewport.Height);

        if (Vector2.Distance(this.Entity.Position, this.Entity.World.Camera.Viewport.Center.ToVector2()) > distance)
        {
            this.Entity.Dispose();
        }
    }
}
