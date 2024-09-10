// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander.Interfaces;

/// <summary>Represents an object that gets notified when the entity dies.</summary>
public interface IDeath
{
    /// <summary>Called when the entity dies.</summary>
    public void Death();
}
