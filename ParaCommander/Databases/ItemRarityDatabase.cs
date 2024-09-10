// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Databases;

using Microsoft.Xna.Framework;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements Must Be Documented
public static class ItemRarityDatabase
{
    public static readonly ItemRarityEntry Common = new(1f / 1, Color.White);
    public static readonly ItemRarityEntry Uncommon = new(1f / 2, Color.Green);
    public static readonly ItemRarityEntry Rare = new(1f / 4, Color.Blue);
    public static readonly ItemRarityEntry Epic = new(1f / 8, Color.Purple);
    public static readonly ItemRarityEntry Legendary = new(1f / 16, Color.Orange);
    public static readonly ItemRarityEntry Artifact = new(1f / 32, Color.Red);

    public static readonly IReadOnlyList<ItemRarityEntry> All = [Common, Uncommon, Rare, Epic, Legendary, Artifact];
}
#pragma warning restore SA1600 // Elements Must Be Documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
