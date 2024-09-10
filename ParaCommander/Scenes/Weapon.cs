// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Scenes;

using Microsoft.Xna.Framework;
using ParaCommander.Components;
using ParaCommander.Databases;

/// <summary>Represents a weapon.</summary>
public class Weapon
{
    private float cooldown;

    /// <summary>Gets the entity that owns the weapon.</summary>
    public required Entity Entity { get; init; }

    /// <summary>Gets the offsets relative to the entity position.</summary>
    public required IList<WeaponOffset> Offsets { get; init; } = [];

    /// <summary>Gets or sets the weapon's cooldown.</summary>
    public float FireDelay { get; set; }

    /// <summary>Gets or sets the weapon's required ammo.</summary>
    public uint Ammo { get; set; }

    /// <summary>Gets or sets the weapon's damage.</summary>
    public uint Damage { get; set; }

    /// <summary>Gets or sets the projectile speed.</summary>
    public uint Speed { get; set; }

    /// <summary>Gets or Sets the collision group of spawned projectiles.</summary>
    public string? CollisionGroup { get; set; }

    /// <summary>Gets or Sets the groups that spawned projectiles will collide with.</summary>
    public IReadOnlyCollection<string> CheckCollisionsWith { get; set; } = [];

    /// <summary>Update the weapon.</summary>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    public void Update(GameTime gameTime)
    {
        this.cooldown = Math.Max(this.cooldown - (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
    }

    /// <summary>Attack with the weapon.</summary>
    /// <returns>True if the attack was successful, false otherwise.</returns>
    public string? Attack()
    {
        if (this.cooldown > 0)
        {
            return null;
        }

        this.cooldown = this.FireDelay;

        if (this.Ammo > 0)
        {
            var player = this.Entity.GetOneOrDefault<PlayerComponent>();

            if (player != null)
            {
                if (player.Ammo < this.Ammo)
                {
                    return null;
                }

                player.Ammo -= this.Ammo;
            }
        }

        foreach (var offset in this.Offsets)
        {
            var worldSpaceOffset = Vector2.Rotate(offset.Offset, MathF.PI / 2);
            var entityRotation = (float)Math.Atan2(this.Entity.Direction.Y, this.Entity.Direction.X);
            var fireDirection = Vector2.Rotate(offset.Direction, entityRotation + (MathF.PI / 2));

            var projectile = this.Entity.World.Spawn(new EntityEntry
            {
                CreateComponents = entity =>
                [
                    new SpriteSheetComponent { Entity = entity, Entry = SpriteSheetDatabase.ProjectileBullet },
                    new MovableComponent { Entity = entity, Speed = this.Speed, Move = fireDirection, Look = fireDirection },
                    new ProjectileComponent { Entity = entity, ImpactSound = "Hit" },
                    new CollisionComponent { Entity = entity, Radius = 1, CollisionGroup = this.CollisionGroup, CheckCollisionsWith = this.CheckCollisionsWith },
                    new ApplyDamageOnCollisionComponent { Entity = entity, Damage = this.Damage },
                    new DespawnWhenLeftViewportComponent { Entity = entity },
                ],
            });

            projectile.Position = this.Entity.Position + Vector2.Rotate(worldSpaceOffset, entityRotation);
        }

        return "Bullet";
    }
}
