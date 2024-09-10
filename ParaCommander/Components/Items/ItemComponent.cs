// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components.Items;

using Microsoft.Xna.Framework;
using ParaCommander.Databases;
using ParaCommander.Scenes;

/// <summary>Adds item behavior on entities.</summary>
public abstract class ItemComponent : Component
{
    private ItemRarityEntry rarity = ItemRarityDatabase.Common;

    /// <summary>Gets or sets the rarity of the item.</summary>
    public ItemRarityEntry Rarity
    {
        get => this.rarity;
        set
        {
            this.rarity = value;

            foreach (var spriteSheet in this.Entity.GetAll<SpriteSheetComponent>())
            {
                spriteSheet.Color = this.rarity.Color;
            }
        }
    }

    /// <summary>Gets or sets the sound to play when the item is picked up.</summary>
    public required string PickupSound { get; set; }

    /// <summary>Applies the effect of the item.</summary>
    /// <param name="entity">The entity to apply the effect to.</param>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    /// <returns>Whether the effect has been finished and the item should be removed.</returns>
    public abstract bool ApplyEffect(Entity entity, GameTime gameTime);
}
