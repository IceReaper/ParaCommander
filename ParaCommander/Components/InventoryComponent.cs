// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using Microsoft.Xna.Framework;
using ParaCommander.Components.Items;
using ParaCommander.Interfaces;
using ParaCommander.Scenes;

/// <summary>Adds an inventory to pick up items.</summary>
public class InventoryComponent : Component, IUpdate, ICollision
{
    /// <summary>Gets the items in the inventory.</summary>
    public IList<ItemComponent> Items { get; } = [];

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        for (var i = 0; i < this.Items.Count; i++)
        {
            var finished = this.Items[i].ApplyEffect(this.Entity, gameTime);

            if (finished)
            {
                this.Items.RemoveAt(i--);
            }
        }
    }

    /// <inheritdoc />
    public void Collision(Entity other)
    {
        var itemComponent = other.GetOneOrDefault<ItemComponent>();

        if (itemComponent == null)
        {
            return;
        }

        this.Items.Add(itemComponent);
        this.Entity.World.Play(itemComponent.PickupSound);
        other.Dispose();
    }
}
