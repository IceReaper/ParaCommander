// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components.Items;

using System.Diagnostics;
using Microsoft.Xna.Framework;
using ParaCommander.Scenes;

/// <summary>Base class for stacking item components.</summary>
/// <typeparam name="T">The type of the item component to stack.</typeparam>
public abstract class StackingItemComponent<T> : ItemComponent
    where T : ItemComponent
{
    private bool initialized;

    /// <inheritdoc />
    public override bool ApplyEffect(Entity entity, GameTime gameTime)
    {
        if (this.initialized)
        {
            return this.ApplyEffectInternal(entity, gameTime);
        }

        var instance = entity.GetOne<InventoryComponent>().Items.OfType<T>().FirstOrDefault() ?? throw new UnreachableException();

        if (instance != this)
        {
            this.Stack(entity, gameTime, instance);
            return true;
        }

        this.Initialize(entity, gameTime);
        this.initialized = true;
        return false;
    }

    /// <summary>Initializes the item component.</summary>
    /// <param name="entity">The entity to apply the effect to.</param>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    protected abstract void Initialize(Entity entity, GameTime gameTime);

    /// <summary>Stacks the item component.</summary>
    /// <param name="entity">The entity to apply the effect to.</param>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    /// <param name="instance">The item component instance.</param>
    protected abstract void Stack(Entity entity, GameTime gameTime, T instance);

    /// <summary>Applies the effect of the item.</summary>
    /// <param name="entity">The entity to apply the effect to.</param>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    /// <returns>Whether the effect has been finished and the item should be removed.</returns>
    protected abstract bool ApplyEffectInternal(Entity entity, GameTime gameTime);
}
