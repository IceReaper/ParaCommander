// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using Microsoft.Xna.Framework;
using ParaCommander.Interfaces;
using ParaCommander.Scenes;

/// <summary>Gives the entity the option to collide with other entities.</summary>
public class CollisionComponent : Component, IUpdate
{
    /// <summary>Gets or Sets the radius of the collision circle.</summary>
    public required byte Radius { get; set; }

    /// <summary>Gets or Sets the collision group of this entity.</summary>
    public string? CollisionGroup { get; set; }

    /// <summary>Gets or Sets the groups that this entity will collide with.</summary>
    public IReadOnlyCollection<string> CheckCollisionsWith { get; set; } = [];

    /// <summary>Find entities in the world that collide with the circle.</summary>
    /// <param name="world">The world to search in.</param>
    /// <param name="collisionGroups">The groups to search for.</param>
    /// <param name="position">The position of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <param name="filter">The entities to filter out.</param>
    /// <returns>The entities that collide with the circle.</returns>
    public static IList<Entity> Find(World world, IReadOnlyCollection<string> collisionGroups, Vector2 position, byte radius, Entity[]? filter = null)
    {
        var result = new List<Entity>();

        foreach (var entity in world.Entities)
        {
            if (filter != null && filter.Contains(entity))
            {
                continue;
            }

            if (entity
                .GetAll<CollisionComponent>()
                .Where(collisionComponent => collisionGroups.Contains(collisionComponent.CollisionGroup))
                .Any(collisionComponent => Vector2.Distance(position, entity.Position) < radius + collisionComponent.Radius))
            {
                result.Add(entity);
            }
        }

        return result;
    }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        if (this.CheckCollisionsWith.Count == 0)
        {
            return;
        }

        foreach (var entity in Find(this.Entity.World, this.CheckCollisionsWith, this.Entity.Position, this.Radius, [this.Entity]))
        {
            foreach (var collision in entity.GetAll<ICollision>())
            {
                collision.Collision(this.Entity);
            }

            foreach (var collision in this.Entity.GetAll<ICollision>())
            {
                collision.Collision(entity);
            }
        }
    }
}
