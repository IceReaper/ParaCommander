// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Components.Items;

using Microsoft.Xna.Framework;
using ParaCommander.Scenes;

/// <summary>Gives a random passive skill when picked up.</summary>
public class PassiveItemComponent : ItemComponent
{
    /// <inheritdoc />
    public override bool ApplyEffect(Entity entity, GameTime gameTime)
    {
        // TODO implement
        return true;
    }
}
