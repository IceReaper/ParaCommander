// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Scenes;

using Microsoft.Xna.Framework;

/// <summary>Represents a weapon offset.</summary>
public record WeaponOffset(Vector2 Offset, Vector2 Direction, bool Temporary = false);
