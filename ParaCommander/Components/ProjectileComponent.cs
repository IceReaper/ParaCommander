// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using ParaCommander.Interfaces;
using ParaCommander.Scenes;

/// <summary>Makes the entity behave like a projectile.</summary>
public class ProjectileComponent : Component, ICollision
{
    /// <summary>Gets or sets the sound to play when the projectile hits something.</summary>
    public string? ImpactSound { get; set; }

    /// <inheritdoc />
    public void Collision(Entity other)
    {
        if (this.ImpactSound != null)
        {
            this.Entity.World.Play(this.ImpactSound);
        }

        this.Entity.Dispose();
    }
}
