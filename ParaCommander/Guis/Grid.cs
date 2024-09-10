// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Guis;

using Microsoft.Xna.Framework;

/// <summary>Represents a grid.</summary>
public static class Grid
{
    private const byte BaseSize = 8;

    /// <summary>Returns a grid scaled size.</summary>
    /// <param name="size">The target value.</param>
    /// <returns>The scaled size.</returns>
    public static float Size(float size)
    {
        return size * BaseSize;
    }

    /// <summary>Returns a grid scaled size.</summary>
    /// <param name="width">The target width.</param>
    /// <param name="height">The target height.</param>
    /// <returns>The scaled size.</returns>
    public static Vector2 Size(float width, float height)
    {
        return new Vector2(width, height) * BaseSize;
    }
}
