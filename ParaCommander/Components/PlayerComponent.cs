// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using Microsoft.Xna.Framework;
using ParaCommander.Interfaces;
using ParaCommander.Scenes;

/// <summary>Adds player specific stats to an entity.</summary>
public class PlayerComponent : Component, IUpdate
{
    private TimeSpan ammoCounter;

    /// <summary>Gets or sets the maximum energy of the player.</summary>
    public uint MaxEnergy { get; set; } = 100;

    /// <summary>Gets or sets the energy of the player.</summary>
    public uint Energy { get; set; }

    /// <summary>Gets or sets the maximum energy of the player.</summary>
    public uint MaxAmmo { get; set; } = 100;

    /// <summary>Gets or sets the energy of the player.</summary>
    public uint Ammo { get; set; } = 100;

    /// <summary>Gets or sets the rate at which the player's ammo regenerates.</summary>
    public TimeSpan RegenerateAmmoRate { get; set; }

    /// <summary>Gets or sets the maximum experience of the player.</summary>
    public uint MaxExperience { get; set; } = 100;

    /// <summary>Gets or sets the experience of the player.</summary>
    public uint Experience { get; set; }

    /// <inheritdoc />
    public void Update(GameTime gameTime)
    {
        this.RegenerateAmmo(gameTime);
    }

    private void RegenerateAmmo(GameTime gameTime)
    {
        if (this.RegenerateAmmoRate == TimeSpan.Zero)
        {
            return;
        }

        this.ammoCounter += gameTime.ElapsedGameTime;

        if (this.ammoCounter < this.RegenerateAmmoRate)
        {
            return;
        }

        this.ammoCounter -= this.RegenerateAmmoRate;
        this.Ammo = Math.Min(this.Ammo + 1, this.MaxAmmo);
    }
}
