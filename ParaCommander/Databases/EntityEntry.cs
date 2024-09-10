// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Databases;

using ParaCommander.Scenes;

/// <summary>Represents an entry in an entity database.</summary>
public record EntityEntry(Func<Entity, Component[]>? CreateComponents = null);
