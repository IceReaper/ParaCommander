// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Interfaces;

using Microsoft.Xna.Framework;

/// <summary>Represents an object that can be updated.</summary>
public interface IUpdate
{
    /// <summary>Update the component.</summary>
    /// <param name="gameTime">Snapshot of the game's timing state.</param>
    public void Update(GameTime gameTime);
}
