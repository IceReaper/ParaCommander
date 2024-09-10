// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using ParaCommander.Components.Items;
using ParaCommander.Databases;
using ParaCommander.Interfaces;
using ParaCommander.Scenes;

/// <summary>Causes an entity to spawn an entity when it dies.</summary>
public class SpawnEntityOnDeath : Component, IDeath
{
    /// <summary>Gets the possible entities to spawn along their chance.</summary>
    public required Dictionary<EntityEntry, float> Entities { get; init; }

    /// <inheritdoc />
    public void Death()
    {
        var random = new Random();

        foreach (var (entityEntry, chance) in this.Entities.OrderBy(entry => entry.Value))
        {
            if (random.NextSingle() > chance)
            {
                continue;
            }

            var entity = this.Entity.World.Spawn(entityEntry);
            entity.Position = this.Entity.Position;

            var itemComponent = entity.GetOneOrDefault<ItemComponent>();

            if (itemComponent == null)
            {
                return;
            }

            foreach (var rarity in ItemRarityDatabase.All.OrderBy(rarity => rarity.Chance))
            {
                if (random.NextSingle() > rarity.Chance)
                {
                    continue;
                }

                itemComponent.Rarity = rarity;
                return;
            }
        }
    }
}
