// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components;

using ParaCommander.Databases;
using ParaCommander.Interfaces;
using ParaCommander.Scenes;

/// <summary>Allows to play a spritesheet and/or sound when this entitie dies.</summary>
public class DeathEffectComponent : Component, IDeath
{
    /// <summary>Gets or sets the spritesheet to play.</summary>
    public SpriteSheetEntry? SpriteSheet { get; set; }

    /// <summary>Gets or sets the sound to play.</summary>
    public string? Sound { get; set; }

    /// <inheritdoc />
    public void Death()
    {
        if (this.SpriteSheet != null)
        {
            this.Entity.World.Play(this.SpriteSheet, this.Entity.Position);
        }

        if (this.Sound != null)
        {
            this.Entity.World.Play(this.Sound);
        }
    }
}
