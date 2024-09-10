// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components.Items;

using Microsoft.Xna.Framework;
using ParaCommander.Scenes;

/// <summary>Gives ammo when picked up.</summary>
public class AmmoItemComponent : ItemComponent
{
    /// <summary>Gets or sets the base amount of ammo to give.</summary>
    public uint BaseAmount { get; set; } = 10;

    /// <inheritdoc />
    public override bool ApplyEffect(Entity entity, GameTime gameTime)
    {
        var playerComponent = entity.GetOneOrDefault<PlayerComponent>();

        if (playerComponent != null)
        {
            playerComponent.Ammo = Math.Min(playerComponent.Ammo + (uint)(this.BaseAmount / this.Rarity.Chance), playerComponent.MaxAmmo);
        }

        return true;
    }
}
