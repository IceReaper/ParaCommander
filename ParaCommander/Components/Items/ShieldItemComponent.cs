// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components.Items;

using System.Diagnostics;
using Microsoft.Xna.Framework;
using ParaCommander.Databases;
using ParaCommander.Scenes;

/// <summary>Gives a temporary shield when picked up.</summary>
public class ShieldItemComponent : StackingItemComponent<ShieldItemComponent>
{
    private SpriteSheetComponent? spriteSheetComponent;
    private TimeSpan duration;

    /// <summary>Gets or sets the duration of the item.</summary>
    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(10);

    /// <inheritdoc />
    protected override void Initialize(Entity entity, GameTime gameTime)
    {
        this.spriteSheetComponent = new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.EffectShield };
        entity.Add(this.spriteSheetComponent);

        entity.World.Play("ShieldOn");

        this.duration = this.Duration * Math.Sqrt(1 / this.Rarity.Chance);

        var healthComponent = entity.GetOneOrDefault<HealthComponent>();

        if (healthComponent != null)
        {
            healthComponent.Invincible = true;
        }
    }

    /// <inheritdoc />
    protected override void Stack(Entity entity, GameTime gameTime, ShieldItemComponent instance)
    {
        instance.duration += this.Duration * Math.Sqrt(1 / this.Rarity.Chance);
    }

    /// <inheritdoc />
    protected override bool ApplyEffectInternal(Entity entity, GameTime gameTime)
    {
        this.duration -= gameTime.ElapsedGameTime;

        if (this.duration > TimeSpan.Zero)
        {
            return false;
        }

        entity.World.Play("ShieldOff");
        entity.Remove(this.spriteSheetComponent ?? throw new UnreachableException());

        var healthComponent = entity.GetOneOrDefault<HealthComponent>();

        if (healthComponent != null)
        {
            healthComponent.Invincible = false;
        }

        return true;
    }
}
