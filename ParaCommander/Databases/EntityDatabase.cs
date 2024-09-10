// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Databases;

using Microsoft.Xna.Framework;
using ParaCommander.Components;
using ParaCommander.Components.Items;
using ParaCommander.Scenes;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements Must Be Documented
public static class EntityDatabase
{
    public static readonly EntityEntry ItemAmmo = new(entity =>
    [
        new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.ItemAmmo },
        new CollisionComponent { Entity = entity, Radius = 12, CollisionGroup = "Item" },
        new AmmoItemComponent { Entity = entity, PickupSound = "Item" },
        new DespawnAfterComponent { Entity = entity, TimeSpan = TimeSpan.FromMinutes(1) },
    ]);

    public static readonly EntityEntry ItemEnergy = new(entity =>
    [
        new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.ItemEnergy },
        new CollisionComponent { Entity = entity, Radius = 12, CollisionGroup = "Item" },
        new EnergyItemComponent { Entity = entity, PickupSound = "Item" },
        new DespawnAfterComponent { Entity = entity, TimeSpan = TimeSpan.FromMinutes(1) },
    ]);

    public static readonly EntityEntry ItemExperience = new(entity =>
    [
        new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.ItemExperience },
        new CollisionComponent { Entity = entity, Radius = 12, CollisionGroup = "Item" },
        new ExperienceItemComponent { Entity = entity, PickupSound = "ItemSmall" },
        new DespawnAfterComponent { Entity = entity, TimeSpan = TimeSpan.FromMinutes(1) },
    ]);

    public static readonly EntityEntry ItemGunBack = new(entity =>
    [
        new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.ItemGunBack },
        new CollisionComponent { Entity = entity, Radius = 12, CollisionGroup = "Item" },
        new GunBackItemComponent { Entity = entity, PickupSound = "ItemLarge", Duration = TimeSpan.FromSeconds(30) },
        new DespawnAfterComponent { Entity = entity, TimeSpan = TimeSpan.FromMinutes(1) },
    ]);

    public static readonly EntityEntry ItemGunSide = new(entity =>
    [
        new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.ItemGunSide },
        new CollisionComponent { Entity = entity, Radius = 12, CollisionGroup = "Item" },
        new GunSideItemComponent { Entity = entity, PickupSound = "ItemLarge", Duration = TimeSpan.FromSeconds(30) },
        new DespawnAfterComponent { Entity = entity, TimeSpan = TimeSpan.FromMinutes(1) },
    ]);

    public static readonly EntityEntry ItemLife = new(entity =>
    [
        new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.ItemLife },
        new CollisionComponent { Entity = entity, Radius = 12, CollisionGroup = "Item" },
        new LifeItemComponent { Entity = entity, PickupSound = "ItemLarge" },
        new DespawnAfterComponent { Entity = entity, TimeSpan = TimeSpan.FromMinutes(1) },
    ]);

    public static readonly EntityEntry ItemHealth = new(entity =>
    [
        new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.ItemHealth },
        new CollisionComponent { Entity = entity, Radius = 12, CollisionGroup = "Item" },
        new HealthItemComponent { Entity = entity, PickupSound = "Item" },
        new DespawnAfterComponent { Entity = entity, TimeSpan = TimeSpan.FromMinutes(1) },
    ]);

    public static readonly EntityEntry ItemShield = new(entity =>
    [
        new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.ItemShield },
        new CollisionComponent { Entity = entity, Radius = 12, CollisionGroup = "Item" },
        new ShieldItemComponent { Entity = entity, PickupSound = "ItemLarge", Duration = TimeSpan.FromSeconds(30) },
        new DespawnAfterComponent { Entity = entity, TimeSpan = TimeSpan.FromMinutes(1) },
    ]);

    public static readonly EntityEntry ItemUpgradePassive = new(entity =>
    [
        new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.ItemUpgradePassive },
        new CollisionComponent { Entity = entity, Radius = 12, CollisionGroup = "Item" },
        new PassiveItemComponent { Entity = entity, PickupSound = "Item" },
        new DespawnAfterComponent { Entity = entity, TimeSpan = TimeSpan.FromMinutes(1) },
    ]);

    public static readonly EntityEntry ItemUpgradeWeapon = new(entity =>
    [
        new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.ItemUpgradeWeapon },
        new CollisionComponent { Entity = entity, Radius = 12, CollisionGroup = "Item" },
        new WeaponItemComponent { Entity = entity, PickupSound = "Item" },
        new DespawnAfterComponent { Entity = entity, TimeSpan = TimeSpan.FromMinutes(1) },
    ]);

    public static readonly EntityEntry ShipEnemy = new(entity =>
    [
        new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.ShipEnemy },
        new HealthComponent { Entity = entity, MaxHealth = 100, Health = 100 },
        new DeathEffectComponent { Entity = entity, SpriteSheet = SpriteSheetDatabase.EffectExplosion, Sound = "Explosion" },
        new MovableComponent { Entity = entity, Speed = 200, Acceleration = 1 },
        new CollisionComponent { Entity = entity, Radius = 16, CollisionGroup = "Enemy", CheckCollisionsWith = ["Player"] },
        new ApplyDamageOnCollisionComponent { Entity = entity },
        new ArmamentComponent
        {
            Entity = entity, Weapons =
            {
                new Weapon
                {
                    Entity = entity,
                    Offsets = [new WeaponOffset(new Vector2(0, -8), new Vector2(0, -1))],
                    FireDelay = 1,
                    Damage = 10,
                    Speed = 256,
                    CollisionGroup = "Projectile",
                    CheckCollisionsWith = ["Player"],
                },
            },
        },
        new EnemyAiComponent { Entity = entity, NearDistance = 128 },
        new DespawnWhenLeftViewportComponent { Entity = entity },
        new SpawnEntityOnDeath
        {
            Entity = entity,
            Entities = new Dictionary<EntityEntry, float>
            {
                { ItemAmmo, .1f },
                ////{ ItemEnergy, .1f }, // TODO implement
                ////{ ItemExperience, 1 }, // TODO implement
                { ItemGunBack, .01f },
                { ItemGunSide, .01f },
                { ItemLife, .01f },
                { ItemShield, .01f },
                { ItemHealth, .1f },
                ////{ ItemUpgradePassive, .01f }, // TODO implement
                ////{ ItemUpgradeWeapon, .01f }, // TODO implement
            },
        }
    ]);

    public static readonly EntityEntry ShipPlayer = new(entity =>
    [
        new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.ShipPlayer },
        new PlayerComponent { Entity = entity, RegenerateAmmoRate = TimeSpan.FromSeconds(.5f) },
        new HealthComponent { Entity = entity, MaxHealth = 100, Health = 100 },
        new DeathEffectComponent { Entity = entity, SpriteSheet = SpriteSheetDatabase.EffectExplosion, Sound = "Explosion" },
        new PlayerMovableComponent { Entity = entity, Speed = 350, Acceleration = 4 },
        new CollisionComponent { Entity = entity, Radius = 16, CollisionGroup = "Player", CheckCollisionsWith = ["Item", "Object", "Enemy"] },
        new ApplyDamageOnCollisionComponent { Entity = entity },
        new InventoryComponent { Entity = entity },
        new ArmamentComponent
        {
            Entity = entity, Weapons =
            {
                new Weapon
                {
                    Entity = entity,
                    Offsets =
                    [
                        new WeaponOffset(new Vector2(-10, -8), new Vector2(0, -1)),
                        new WeaponOffset(new Vector2(10, -8), new Vector2(0, -1))
                    ],
                    FireDelay = .25f,
                    Ammo = 1,
                    Damage = 10,
                    Speed = 512,
                    CollisionGroup = "Projectile",
                    CheckCollisionsWith = ["Object", "Enemy"],
                },
            },
        }
    ]);

    public static readonly EntityEntry Asteroid = new(entity =>
    [
        new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.ObjectAsteroid },
        new CollisionComponent { Entity = entity, Radius = 28, CollisionGroup = "Object" },
        new HealthComponent { Entity = entity, MaxHealth = 100, Health = 100 },
        new DeathEffectComponent { Entity = entity, SpriteSheet = SpriteSheetDatabase.EffectExplosion, Sound = "Explosion" },
        new DespawnWhenLeftViewportComponent { Entity = entity },
        new MovableComponent { Entity = entity },
        new SpawnEntityOnDeath
        {
            Entity = entity,
            Entities = new Dictionary<EntityEntry, float>
            {
                { ItemAmmo, .1f },
                ////{ ItemEnergy, .1f }, // TODO implement
                ////{ ItemExperience, 1 }, // TODO implement
                { ItemGunBack, .01f },
                { ItemGunSide, .01f },
                { ItemLife, .01f },
                { ItemShield, .01f },
                { ItemHealth, .1f },
                ////{ ItemUpgradePassive, .01f }, // TODO implement
                ////{ ItemUpgradeWeapon, .01f }, // TODO implement
            },
        }
    ]);
}
#pragma warning restore SA1600 // Elements Must Be Documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
