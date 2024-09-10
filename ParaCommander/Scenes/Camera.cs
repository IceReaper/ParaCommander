// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Scenes;

using Microsoft.Xna.Framework;

/// <summary>Represents a camera.</summary>
public class Camera
{
    /// <summary>Gets the matrix used to make the game render like it would have a camera.</summary>
    public Matrix View { get; private set; }

    /// <summary>Gets the viewport of the camera, required for properly projecting the mouse position and tiling the background.</summary>
    public Rectangle Viewport { get; private set; }

    /// <summary>Update the camera properties.</summary>
    /// <param name="viewport">The viewport of the game.</param>
    /// <param name="center">The center of the camera.</param>
    public void Update(Rectangle viewport, Vector2 center)
    {
        var scale = Math.Min(viewport.Width / (float)GameMain.TargetWidth, viewport.Height / (float)GameMain.TargetHeight);

        var viewportSize = viewport.Size.ToVector2();

        var topLeft = (viewportSize / -2 / scale) + center;
        var bottomRight = (viewportSize / 2 / scale) + center;
        this.Viewport = new Rectangle(topLeft.ToPoint(), (bottomRight - topLeft).ToPoint());

        this.View = Matrix.Identity;
        this.View *= Matrix.CreateTranslation(-center.X, -center.Y, 0);
        this.View *= Matrix.CreateScale(scale);
        this.View *= Matrix.CreateTranslation(viewportSize.X / 2, viewportSize.Y / 2, 0);
    }

    /// <summary>Transforms a position from screen-space to world-space.</summary>
    /// <param name="position">The screen-space position.</param>
    /// <returns>The world-space position.</returns>
    public Vector2 ScreenToWorld(Vector2 position)
    {
        return Vector2.Transform(position, Matrix.Invert(this.View));
    }

    /// <summary>Transforms a position from world-space to screen-space.</summary>
    /// <param name="position">The world-space position.</param>
    /// <returns>The screen-space position.</returns>
    public Vector2 WorldToScreen(Vector2 position)
    {
        return Vector2.Transform(position, this.View);
    }
}
