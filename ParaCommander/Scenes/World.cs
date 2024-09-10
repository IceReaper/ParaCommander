// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Scenes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ParaCommander.Components;
using ParaCommander.Databases;
using ParaCommander.GameModes;

/// <summary>Represents the world where entities exist in.</summary>
public sealed class World : IDisposable
{
    private readonly SpriteBatch spriteBatch;

    private readonly Background background;
    private readonly List<Entity> entities = [];
    private readonly SoundEffectInstance music;
    private readonly List<SoundEffectInstance> sounds = [];
    private readonly List<Entity> effects = [];

    private bool paused;

    /// <summary>Initializes a new instance of the <see cref="World"/> class.</summary>
    /// <param name="gameMain">The GameMain instance.</param>
    public World(GameMain gameMain)
    {
        this.GameMain = gameMain;

        this.spriteBatch = new SpriteBatch(this.GameMain.Game.GraphicsDevice);

        this.background = new Background(this.GameMain.Game);
        this.music = this.GameMain.Game.Content.Load<SoundEffect>("Music/InGame").CreateInstance();
        this.music.IsLooped = true;
        this.music.Volume = this.GameMain.Settings.CalculatedMusicVolume;
        this.music.Play();

        this.GameMain.Settings.OnChanged += this.SettingsChanged;
    }

    /// <summary>Gets the GameMain instance.</summary>
    public GameMain GameMain { get; }

    /// <summary>Gets the game mode.</summary>
    public required GameMode GameMode { get; init; }

    /// <summary>Gets the transformation matrix to apply to the world.</summary>
    public Camera Camera { get; } = new();

    /// <summary>Gets the entities in the world.</summary>
    public IEnumerable<Entity> Entities => this.entities;

    /// <summary>Gets or sets a value indicating whether the world is paused.</summary>
    public bool Paused
    {
        get => this.paused;
        set
        {
            this.paused = value;

            foreach (var sound in this.sounds)
            {
                if (this.paused)
                {
                    sound.Pause();
                }
                else
                {
                    sound.Resume();
                }
            }
        }
    }

    /// <summary>Spawns a new entity in the world.</summary>
    /// <param name="entry">The entity entry to spawn.</param>
    /// <returns>The spawned entity.</returns>
    public Entity Spawn(EntityEntry entry)
    {
        var entity = new Entity { World = this };
        this.entities.Add(entity);

        if (entry.CreateComponents == null)
        {
            return entity;
        }

        foreach (var component in entry.CreateComponents(entity))
        {
            entity.Add(component);
        }

        return entity;
    }

    /// <summary>Plays a sound.</summary>
    /// <param name="path">The path to the sound.</param>
    public void Play(string path)
    {
        var instance = this.GameMain.Game.Content.Load<SoundEffect>($"Sounds/{path}").CreateInstance();

        instance.Volume = this.GameMain.Settings.CalculatedSoundVolume;
        instance.Play();

        this.sounds.Add(instance);
    }

    /// <summary>Plays a spritesheet.</summary>
    /// <param name="spriteSheet">The spritesheet to play.</param>
    /// <param name="position">The position to play the spritesheet at.</param>
    public void Play(SpriteSheetEntry spriteSheet, Vector2 position)
    {
        var entity = new Entity { World = this };
        entity.Add(new SpriteSheetComponent { Entity = entity, Entry = spriteSheet });
        entity.Position = position;
        this.effects.Add(entity);
    }

    /// <summary>Updates the simulation.</summary>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    public void Update(GameTime gameTime)
    {
        if (this.Paused)
        {
            return;
        }

        this.sounds.RemoveAll(sound => sound.State == SoundState.Stopped);

        foreach (var entity in this.Entities.ToArray())
        {
            entity.Update(gameTime);
        }

        this.entities.RemoveAll(entity => entity.Disposed);

        this.GameMode.Update(gameTime, this);
    }

    /// <summary>Draws the world.</summary>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    public void Draw(GameTime gameTime)
    {
        this.spriteBatch.Begin(
            SpriteSortMode.Immediate,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            DepthStencilState.None,
            RasterizerState.CullCounterClockwise,
            this.GameMain.RemapEffect,
            this.Camera.View);

        this.background.Draw(this.spriteBatch, this.Camera);

        foreach (var entity in this.entities)
        {
            entity.PrepareDraw(gameTime);
            entity.Draw(this.spriteBatch, this.GameMain.RemapEffect);
        }

        for (var i = 0; i < this.effects.Count; i++)
        {
            var effect = this.effects[i];

            effect.PrepareDraw(gameTime);

            if (!effect.GetOne<SpriteSheetComponent>().Finished)
            {
                effect.Draw(this.spriteBatch, this.GameMain.RemapEffect);
                continue;
            }

            this.effects.RemoveAt(i--);
        }

        this.spriteBatch.End();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.music.Stop();
        this.spriteBatch.Dispose();

        this.GameMain.Settings.OnChanged -= this.SettingsChanged;
    }

    private void SettingsChanged(object? sender, EventArgs args)
    {
        this.music.Volume = this.GameMain.Settings.CalculatedMusicVolume;

        foreach (var sound in this.sounds)
        {
            sound.Volume = this.GameMain.Settings.CalculatedSoundVolume;
        }
    }
}
