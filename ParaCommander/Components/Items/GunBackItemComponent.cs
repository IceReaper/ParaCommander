// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components.Items;

using Microsoft.Xna.Framework;
using ParaCommander.Scenes;

/// <summary>Gives a temporary backwards gun when picked up.</summary>
public class GunBackItemComponent : StackingItemComponent<GunBackItemComponent>
{
    private readonly Dictionary<Weapon, List<WeaponOffset>> temporaryWeaponOffsets = [];

    private ArmamentComponent? armamentComponent;
    private TimeSpan duration;

    /// <summary>Gets or sets the duration of the item.</summary>
    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(10);

    /// <inheritdoc />
    protected override void Initialize(Entity entity, GameTime gameTime)
    {
        this.armamentComponent = entity.GetOneOrDefault<ArmamentComponent>();

        if (this.armamentComponent == null)
        {
            return;
        }

        this.duration = this.Duration * Math.Sqrt(1 / this.Rarity.Chance);

        foreach (var weapon in this.armamentComponent.Weapons)
        {
            var weaponOffsets = new List<WeaponOffset>();

            foreach (var weaponOffset in weapon.Offsets.Where(weaponOffset => !weaponOffset.Temporary))
            {
                weaponOffsets.Add(new WeaponOffset(
                    Vector2.Rotate(weaponOffset.Offset, MathHelper.Pi),
                    Vector2.Rotate(weaponOffset.Direction, MathHelper.Pi),
                    true));
            }

            foreach (var weaponOffset in weaponOffsets)
            {
                weapon.Offsets.Add(weaponOffset);
            }

            this.temporaryWeaponOffsets.Add(weapon, weaponOffsets);
        }
    }

    /// <inheritdoc />
    protected override void Stack(Entity entity, GameTime gameTime, GunBackItemComponent instance)
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

        foreach (var (weapon, weaponOffsets) in this.temporaryWeaponOffsets)
        {
            foreach (var weaponOffset in weaponOffsets)
            {
                weapon.Offsets.Remove(weaponOffset);
            }
        }

        return true;
    }
}
