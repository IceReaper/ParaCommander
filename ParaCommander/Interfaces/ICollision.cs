// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Interfaces;

using ParaCommander.Scenes;

/// <summary>Represents an object that gets notified when a collision with another entity occured.</summary>
public interface ICollision
{
    /// <summary>Called when a collision with another entity has occured.</summary>
    /// <param name="other">The entity that was collided with.</param>
    public void Collision(Entity other);
}
