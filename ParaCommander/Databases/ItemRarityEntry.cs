// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Databases;

using Microsoft.Xna.Framework;

/// <summary>Represents an entry in an item rarity database.</summary>
public record ItemRarityEntry(float Chance, Color Color);
