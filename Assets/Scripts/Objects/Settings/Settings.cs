using Assets.Scripts.Manage.Localization;
using Assets.Scripts.Objects.Settings;
using System;

[Serializable]
public class Settings
{
    protected Settings() { }

    public Settings(Size Resolution, Difficult Level, KeyBind Controller, float VolumeEffect, float VolumeMusic, Language Language)
    {
        this.Resolution = Resolution;
        this.Level = Level;
        this.Controller = Controller;
        this.VolumeEffect = VolumeEffect;
        this.VolumeMusic = VolumeMusic;
        this.Language = Language;
    }

    public Size Resolution { get; set; }
    public Difficult Level { get; set; }
    public float VolumeEffect { get; set; }
    public float VolumeMusic { get; set; }
    public Language Language { get; set; }
    public KeyBind Controller { get; set; }
}