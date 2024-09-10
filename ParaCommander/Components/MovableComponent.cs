// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using Microsoft.Xna.Framework;
using ParaCommander.Interfaces;
using ParaCommander.Scenes;

/// <summary>Gives the entity the option to move.</summary>
public class MovableComponent : Component, IUpdate
{
    /// <summary>Gets or sets the velocity of the entity.</summary>
    public Vector2 Velocity { get; set; }

    /// <summary>Gets or sets the speed of the entity.</summary>
    public uint Speed { get; set; }

    /// <summary>Gets or sets the acceleration of the entity.</summary>
    public float Acceleration { get; set; } = uint.MaxValue;

    /// <summary>Gets or sets the move vector of the entity.</summary>
    public Vector2 Move { get; set; }

    /// <summary>Gets or sets the look vector of the entity.</summary>
    public Vector2 Look { get; set; }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        this.UpdateVelocity(gameTime);
        this.UpdateDirection();

        this.Entity.Position += this.Velocity * this.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    /// <summary>Updates the velocity of the entity.</summary>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    protected virtual void UpdateVelocity(GameTime gameTime)
    {
        if (this.Move.Length() > 1)
        {
            this.Move.Normalize();
        }

        var frameAcceleration = new Vector2(this.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds);
        this.Velocity += Vector2.Clamp(this.Move - this.Velocity, -frameAcceleration, frameAcceleration);
    }

    private void UpdateDirection()
    {
        if (this.Look.Length() == 0)
        {
            return;
        }

        this.Look.Normalize();
        this.Entity.Direction = this.Look;
    }
}
