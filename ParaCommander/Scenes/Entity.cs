// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Scenes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParaCommander.Interfaces;

/// <summary>Represents an entity in the game.</summary>
public sealed class Entity : IDisposable
{
    private readonly List<Component> components = [];

    /// <summary>Gets the world the entity is in.</summary>
    public required World World { get; init; }

    /// <summary>Gets or sets the position of the entity.</summary>
    public Vector2 Position { get; set; }

    /// <summary>Gets or sets the direction of the entity.</summary>
    public Vector2 Direction { get; set; } = new(0, -1);

    /// <summary>Gets a value indicating whether the entity has been disposed.</summary>
    public bool Disposed { get; private set; }

    /// <summary>Adds a component to the entity.</summary>
    /// <param name="component">The component to add.</param>
    public void Add(Component component)
    {
        if (component.Entity != this || this.components.Contains(component))
        {
            return;
        }

        this.components.Add(component);
    }

    /// <summary>Gets exactly one component of the specified type.</summary>
    /// <typeparam name="T">The type of component to get.</typeparam>
    /// <returns>The component of the specified type.</returns>
    public T GetOne<T>()
    {
        var components = this.components.OfType<T>().ToList();

        return components.Count switch
        {
            0 => throw new InvalidOperationException("No components of the specified type were found."),
            1 => components[0],
            _ => throw new InvalidOperationException("Multiple components of the specified type were found."),
        };
    }

    /// <summary>Gets one component of the specified type or null if not present on entity.</summary>
    /// <typeparam name="T">The type of component to get.</typeparam>
    /// <returns>The component of the specified type or null if not present on entity.</returns>
    public T? GetOneOrDefault<T>()
    {
        var components = this.components.OfType<T>().ToList();

        return components.Count switch
        {
            0 => default,
            1 => components[0],
            _ => throw new InvalidOperationException("Multiple components of the specified type were found."),
        };
    }

    /// <summary>Gets all components of the specified type.</summary>
    /// <typeparam name="T">The type of component to get.</typeparam>
    /// <returns>All components of the specified type.</returns>
    public IEnumerable<T> GetAll<T>()
    {
        return this.components.OfType<T>().ToList();
    }

    /// <summary>Removes a component from the entity.</summary>
    /// <param name="component">The component to remove.</param>
    public void Remove(Component component)
    {
        this.components.Remove(component);
    }

    /// <summary>Removes all components of a specific type from the entity.</summary>
    /// <typeparam name="T">The type of component to remove.</typeparam>
    public void RemoveAll<T>()
    {
        this.components.RemoveAll(component => component is T);
    }

    /// <summary>Update the entity.</summary>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    public void Update(GameTime gameTime)
    {
        foreach (var update in this.GetAll<IUpdate>())
        {
            update.Update(gameTime);
        }
    }

    /// <summary>Draw the entity.</summary>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    public void PrepareDraw(GameTime gameTime)
    {
        foreach (var draw in this.GetAll<IDraw>())
        {
            draw.PrepareDraw(gameTime);
        }
    }

    /// <summary>Draw the entity.</summary>
    /// <param name="spriteBatch">The sprite batch to draw the entity with.</param>
    /// <param name="remap">The remap effect to apply to the entity.</param>
    public void Draw(SpriteBatch spriteBatch, Effect remap)
    {
        foreach (var draw in this.GetAll<IDraw>())
        {
            draw.Draw(spriteBatch, remap);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.Disposed = true;
    }
}
