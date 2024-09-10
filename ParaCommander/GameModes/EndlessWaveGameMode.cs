// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.GameModes;

using Microsoft.Xna.Framework;
using ParaCommander.Components;
using ParaCommander.Databases;
using ParaCommander.Scenes;

/// <summary>Represents the endless wave game mode.</summary>
public class EndlessWaveGameMode : GameMode
{
    private readonly Random random;

    private bool initialized;
    private TimeSpan timeSinceLastWave;

    /// <summary>Initializes a new instance of the <see cref="EndlessWaveGameMode"/> class.</summary>
    public EndlessWaveGameMode()
    {
        this.random = new Random();
    }

    /// <summary>Gets or sets the wave interval.</summary>
    public TimeSpan WaveInterval { get; set; }

    /// <inheritdoc />
    public override void Update(GameTime gameTime, World world)
    {
        if (this.initialized)
        {
            this.timeSinceLastWave += gameTime.ElapsedGameTime;

            if (this.timeSinceLastWave < this.WaveInterval)
            {
                return;
            }

            this.timeSinceLastWave -= this.WaveInterval;
        }
        else
        {
            this.initialized = true;
        }

        if (world.Entities.All(entity => entity.GetOneOrDefault<PlayerComponent>() == null))
        {
            return;
        }

        if (this.random.NextSingle() < 0.5f)
        {
            this.SpawnAsteroids(world);
        }
        else
        {
            this.SpawnEnemies(world);
        }
    }

    private void SpawnAsteroids(World world)
    {
        for (var i = 0; i < 3; i++)
        {
            this.GetObjectPath(world, out var position, out var direction);

            var entity = world.Spawn(EntityDatabase.Asteroid);
            entity.Position = position;

            var movableComponent = entity.GetOneOrDefault<MovableComponent>();

            if (movableComponent == null)
            {
                continue;
            }

            movableComponent.Move = direction;
            movableComponent.Speed = (uint)this.random.Next(2, 5) * 50;
        }
    }

    private void SpawnEnemies(World world)
    {
        for (var i = 0; i < 3; i++)
        {
            this.GetObjectPath(world, out var position, out _);

            var entity = world.Spawn(EntityDatabase.ShipEnemy);
            entity.Position = position;
        }
    }

    private void GetObjectPath(World world, out Vector2 position, out Vector2 direction)
    {
        var distance = Math.Max(world.Camera.Viewport.Width, world.Camera.Viewport.Height) * .75f;
        var offset = Vector2.Rotate(new Vector2(0, distance), this.random.NextSingle() * MathF.PI * 2);
        direction = Vector2.Rotate(Vector2.Normalize(offset * -1), (this.random.NextSingle() - .5f) * MathF.PI / 2);
        position = world.Camera.Viewport.Center.ToVector2() + offset;
    }
}
