// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using Microsoft.Xna.Framework;
using ParaCommander.Interfaces;
using ParaCommander.Scenes;

/// <summary>Represents a component that despawns an entity after a certain amount of time.</summary>
public class DespawnAfterComponent : Component, IUpdate
{
    private TimeSpan timer;

    /// <summary>Gets or sets the time span after which the entity should despawn.</summary>
    public required TimeSpan TimeSpan { get; set; }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        this.timer += gameTime.ElapsedGameTime;

        if (this.timer >= this.TimeSpan)
        {
            this.Entity.Dispose();
        }
    }
}
