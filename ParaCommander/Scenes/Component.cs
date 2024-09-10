// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Scenes;

/// <summary>Base class for all components.</summary>
public abstract class Component
{
    /// <summary>Gets the entity.</summary>
    public required Entity Entity { get; init; }
}
