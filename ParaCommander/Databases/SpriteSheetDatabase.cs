// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Databases;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements Must Be Documented
public static class SpriteSheetDatabase
{
    public static readonly SpriteSheetEntry ItemAmmo = new("Items/Ammo", 1);
    public static readonly SpriteSheetEntry ItemEnergy = new("Items/Energy", 1);
    public static readonly SpriteSheetEntry ItemExperience = new("Items/Experience", 1);
    public static readonly SpriteSheetEntry ItemGunBack = new("Items/GunBack", 1);
    public static readonly SpriteSheetEntry ItemGunSide = new("Items/GunSide", 1);
    public static readonly SpriteSheetEntry ItemHealth = new("Items/Health", 1);
    public static readonly SpriteSheetEntry ItemLife = new("Items/Life", 4);
    public static readonly SpriteSheetEntry ItemShield = new("Items/Shield", 4);
    public static readonly SpriteSheetEntry ItemUpgradePassive = new("Items/UpgradePassive", 4);
    public static readonly SpriteSheetEntry ItemUpgradeWeapon = new("Items/UpgradeWeapon", 4);

    public static readonly SpriteSheetEntry EffectExplosion = new("Effects/Explosion", 6);
    public static readonly SpriteSheetEntry EffectShield = new("Effects/Shield", 4);

    public static readonly SpriteSheetEntry ShipEnemy = new("Ships/Enemy", 4);
    public static readonly SpriteSheetEntry ShipPlayer = new("Ships/Player", 4);

    public static readonly SpriteSheetEntry ProjectileBullet = new("Projectiles/Bullet", 1);

    public static readonly SpriteSheetEntry ObjectAsteroid = new("Objects/Asteroid", 1);
}
#pragma warning restore SA1600 // Elements Must Be Documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
