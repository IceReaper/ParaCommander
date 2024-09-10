// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components.Items;

using Microsoft.Xna.Framework;
using ParaCommander.Scenes;

/// <summary>Gives health when picked up.</summary>
public class HealthItemComponent : ItemComponent
{
    /// <summary>Gets or sets the base amount of health to give.</summary>
    public uint BaseAmount { get; set; } = 10;

    /// <inheritdoc />
    public override bool ApplyEffect(Entity entity, GameTime gameTime)
    {
        var healthComponent = entity.GetOneOrDefault<HealthComponent>();

        if (healthComponent != null)
        {
            healthComponent.Health = Math.Min(healthComponent.Health + (uint)(this.BaseAmount / this.Rarity.Chance), healthComponent.MaxHealth);
        }

        return true;
    }
}
