// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using Microsoft.Xna.Framework;
using ParaCommander.Interfaces;
using ParaCommander.Scenes;

/// <summary>Gives the entity the option to use weapons.</summary>
public class ArmamentComponent : Component, IUpdate
{
    /// <summary>Gets the weapons of the entity along their offset.</summary>
    public IList<Weapon> Weapons { get; } = [];

    /// <summary>Gets or sets a value indicating whether the entity is firing.</summary>
    public bool IsFiring { get; set; }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        foreach (var weapon in this.Weapons)
        {
            weapon.Update(gameTime);
        }

        if (!this.IsFiring)
        {
            return;
        }

        var sounds = new HashSet<string>();

        foreach (var weapon in this.Weapons)
        {
            var sound = weapon.Attack();

            if (sound != null)
            {
                sounds.Add(sound);
            }
        }

        foreach (var sound in sounds)
        {
            this.Entity.World.Play(sound);
        }
    }
}
