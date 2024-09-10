// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using ParaCommander.Interfaces;
using ParaCommander.Scenes;

/// <summary>Applies damage to collided entities.</summary>
public class ApplyDamageOnCollisionComponent : Component, ICollision
{
    /// <summary>Gets or sets the damage to apply. When set to 0, the health is used.</summary>
    public uint Damage { get; set; }

    /// <inheritdoc />
    public void Collision(Entity other)
    {
        if (this.Damage == 0)
        {
            var healthComponent = this.Entity.GetOneOrDefault<HealthComponent>();
            var otherHealthComponent = other.GetOneOrDefault<HealthComponent>();

            if (healthComponent == null || healthComponent.Invincible || otherHealthComponent == null || otherHealthComponent.Invincible)
            {
                return;
            }

            var damage = Math.Min(healthComponent.Health, otherHealthComponent.Health);
            healthComponent.ApplyDamage(damage);
            otherHealthComponent.ApplyDamage(damage);
        }
        else
        {
            other.GetOneOrDefault<HealthComponent>()?.ApplyDamage(this.Damage);
        }
    }
}
