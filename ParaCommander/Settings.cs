// Copyright (c) the ParaCommander developers.
// This file is part of ParaCommander which is free software.
// It is made available to you under the terms of the MIT License.
// For more information, see LICENSE.

namespace ParaCommander;

/// <summary>Represents the settings.</summary>
public class Settings
{
    private const uint Version = 1;

    private static readonly string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ParaCommander", "settings.bin");

    private float masterVolume = 1;
    private float soundVolume = 1;
    private float musicVolume = 1;

    /// <summary>Initializes a new instance of the <see cref="Settings"/> class.</summary>
    public Settings()
    {
        try
        {
            this.LoadSettings();
        }
        catch (Exception)
        {
            this.SaveSettings();
        }
    }

    /// <summary>Occurs when the settings changed.</summary>
    public event EventHandler? OnChanged;

    /// <summary>Gets or sets the master volume.</summary>
    public float MasterVolume
    {
        get => this.masterVolume;
        set
        {
            this.masterVolume = value;
            this.OnChanged?.Invoke(null, EventArgs.Empty);
            this.SaveSettings();
        }
    }

    /// <summary>Gets or sets the sound volume.</summary>
    public float SoundVolume
    {
        get => this.soundVolume;
        set
        {
            this.soundVolume = value;
            this.OnChanged?.Invoke(null, EventArgs.Empty);
            this.SaveSettings();
        }
    }

    /// <summary>Gets or sets the music volume.</summary>
    public float MusicVolume
    {
        get => this.musicVolume;
        set
        {
            this.musicVolume = value;
            this.OnChanged?.Invoke(null, EventArgs.Empty);
            this.SaveSettings();
        }
    }

    /// <summary>Gets the calculated sound volume.</summary>
    public float CalculatedSoundVolume => this.MasterVolume * this.SoundVolume;

    /// <summary>Gets the calculated music volume.</summary>
    public float CalculatedMusicVolume => this.MasterVolume * this.MusicVolume;

    private void LoadSettings()
    {
        using var stream = File.OpenRead(Path);
        using var reader = new BinaryReader(stream);

        var version = reader.ReadUInt32();

        if (version != Version)
        {
            throw new InvalidDataException("Invalid version");
        }

        this.masterVolume = reader.ReadSingle();
        this.soundVolume = reader.ReadSingle();
        this.musicVolume = reader.ReadSingle();
    }

    private void SaveSettings()
    {
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path) ?? string.Empty);

        using var stream = File.OpenWrite(Path);
        using var writer = new BinaryWriter(stream);

        writer.Write(Version);

        writer.Write(this.masterVolume);
        writer.Write(this.soundVolume);
        writer.Write(this.musicVolume);
    }
}
