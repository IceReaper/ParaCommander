// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using ParaCommander.Interfaces;
using ParaCommander.Scenes;

/// <summary>Gives an entity health, makes it killable.</summary>
public class HealthComponent : Component
{
    /// <summary>Gets or sets the maximum health of the entity.</summary>
    public uint MaxHealth { get; set; } = 100;

    /// <summary>Gets or sets the health of the entity.</summary>
    public uint Health { get; set; } = 100;

    /// <summary>Gets or sets a value indicating whether the entity is invincible.</summary>
    public bool Invincible { get; set; }

    /// <summary>Applies damage to this entity. When the health reaches zero, the entity will despawn.</summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void ApplyDamage(uint damage)
    {
        if (this.Invincible)
        {
            return;
        }

        this.Health = damage > this.Health ? 0 : this.Health - damage;

        if (this.Health > 0)
        {
            return;
        }

        foreach (var death in this.Entity.GetAll<IDeath>())
        {
            death.Death();
        }

        this.Entity.Dispose();
    }
}
