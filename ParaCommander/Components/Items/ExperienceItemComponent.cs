// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components.Items;

using Microsoft.Xna.Framework;
using ParaCommander.Scenes;

/// <summary>Gives experience when picked up.</summary>
public class ExperienceItemComponent : ItemComponent
{
    /// <summary>Gets or sets the base amount of experience to give.</summary>
    public uint BaseAmount { get; set; } = 10;

    /// <inheritdoc />
    public override bool ApplyEffect(Entity entity, GameTime gameTime)
    {
        var playerComponent = entity.GetOneOrDefault<PlayerComponent>();

        if (playerComponent != null)
        {
            playerComponent.Experience = Math.Min(playerComponent.Experience + (uint)(this.BaseAmount / this.Rarity.Chance), playerComponent.MaxExperience);

            // TODO handle levelup!
        }

        return true;
    }
}
