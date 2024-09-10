// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using Microsoft.Xna.Framework;

/// <summary>A special variant of the movable component which ensures that players cannot move too far away from each other.</summary>
public class PlayerMovableComponent : MovableComponent
{
    private const uint MaxDistanceBetweenPlayers = GameMain.TargetHeight - 64;

    /// <inheritdoc />
    protected override void UpdateVelocity(GameTime gameTime)
    {
        base.UpdateVelocity(gameTime);

        var targetPosition = this.Entity.Position + (this.Velocity * this.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds);

        var furthestPlayer = this.Entity.World.Entities
            .Where(entity => entity.GetOneOrDefault<PlayerComponent>() != null)
            .Where(entity => entity != this.Entity)
            .MaxBy(entity => Vector2.Distance(entity.Position, targetPosition));

        if (furthestPlayer == null)
        {
            return;
        }

        var distanceToFurthestPlayer = Vector2.Distance(furthestPlayer.Position, targetPosition);

        if (!(distanceToFurthestPlayer > MaxDistanceBetweenPlayers))
        {
            return;
        }

        var move = furthestPlayer.Position + (Vector2.Normalize(targetPosition - furthestPlayer.Position) * MaxDistanceBetweenPlayers) - this.Entity.Position;
        this.Velocity = move / this.Speed / (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}
