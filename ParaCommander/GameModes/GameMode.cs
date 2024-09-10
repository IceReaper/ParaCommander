// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.GameModes;

using Microsoft.Xna.Framework;
using ParaCommander.Scenes;

/// <summary>Represents a game mode.</summary>
public abstract class GameMode
{
    /// <summary>Gets or sets the remaining lives.</summary>
    public uint Lives { get; set; }

    /// <summary>Update the GameMode.</summary>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    /// <param name="world">The world to update.</param>
    public abstract void Update(GameTime gameTime, World world);
}
