// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using Microsoft.Xna.Framework;
using ParaCommander.Interfaces;
using ParaCommander.Scenes;

/// <summary>Makes the enemy fly towards the nearest player and shoot.</summary>
public class EnemyAiComponent : Component, IUpdate
{
    /// <summary>Gets or sets the range the entity tries to move near the player.</summary>
    public uint NearDistance { get; set; }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        var nearestPlayer = this.Entity.World.Entities
            .Where(entity => entity.GetOneOrDefault<PlayerComponent>() != null)
            .MinBy(entity => Vector2.Distance(entity.Position, this.Entity.Position));

        var movableComponent = this.Entity.GetOneOrDefault<MovableComponent>();
        var armamentComponent = this.Entity.GetOneOrDefault<ArmamentComponent>();

        if (nearestPlayer == null)
        {
            if (movableComponent != null)
            {
                movableComponent.Move = Vector2.Zero;
                movableComponent.Look = Vector2.Zero;
            }

            if (armamentComponent != null)
            {
                armamentComponent.IsFiring = false;
            }
        }
        else
        {
            var direction = Vector2.Normalize(nearestPlayer.Position - this.Entity.Position);
            var distance = Vector2.Distance(nearestPlayer.Position, this.Entity.Position);

            if (movableComponent != null)
            {
                movableComponent.Move = distance > this.NearDistance ? direction : Vector2.Zero;
                movableComponent.Look = direction;
            }

            if (armamentComponent != null)
            {
                armamentComponent.IsFiring = this.Entity.World.Camera.Viewport.Contains(this.Entity.Position);
            }
        }
    }
}
