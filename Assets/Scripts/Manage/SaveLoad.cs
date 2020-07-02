using Assets.Scripts.Manage;
using System;
using System.IO;
using System.Xml.Serialization;
using Assets.Scripts.Objects.Settings;
using System.Collections.Generic;
using System.Linq;

public static class SaveLoad
{
    private static readonly XmlSerializer SettingsFormatter = new XmlSerializer(typeof(Settings), new XmlRootAttribute("Settings"));
    private static readonly XmlSerializer CharacterFormatter = new XmlSerializer(typeof(Character));

    #region settings
    public static Settings LoadSettings()
    {
        try
        {
            using (FileStream fs = new FileStream(Global.GetSettingsFile(), FileMode.Open))
            {
                Global.Debug("Файл настроек загружен");
                return (Settings)SettingsFormatter.Deserialize(fs);
            }
        }
        catch (Exception ex)
        {
            Global.Debug("Catch exception: " + ex.Message);
            SaveSettings();
            using (FileStream fs = new FileStream(Global.GetSettingsFile(), FileMode.Open))
            {
                Global.Debug("Файл настроек восстановлен");
                
                return (Settings)SettingsFormatter.Deserialize(fs);
            }
        }

    }
    public static void SaveSettings()
    {
        File.Delete(Global.GetSettingsFile());
        using (FileStream fs = new FileStream(Global.GetSettingsFile(), FileMode.Create))
        {
            SettingsFormatter.Serialize(fs, Global.Settings);
        }
        Global.Debug("Файл настроек сохранен");
    }
    #endregion

    #region character
    public static List<Character> LoadCharacters()
    {
        List<Character> characters = new List<Character>();
        if (!Directory.Exists(Global.GetCharactersFolder()))
        {
            return characters;
        }
        foreach (string filename in Directory.GetFiles(Global.GetCharactersFolder()).Where(x => x.Contains(Global.CharacterFileName)))
        {
            Character character = LoadCharacter(filename);
            if (character != null)
                characters.Add(character);
        }
            
        return characters;
    }
    private static Character LoadCharacter(string filename)
    {
        try
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                return (Character)CharacterFormatter.Deserialize(fs);
            }
        }
        catch (InvalidOperationException ex)
        {
            Global.Debug("Catch exception: " + ex.Message);
            return null;
        }

    }
    public static void SaveCharacter(Character character)
    {
        if (!Directory.Exists(Global.GetCharactersFolder()))
        {
            Directory.CreateDirectory(Global.GetCharactersFolder());
        }
        using (FileStream fs = new FileStream(Path.Combine(Global.GetCharactersFolder(), character.Name + Global.CharacterFileName), FileMode.OpenOrCreate))
        {
            CharacterFormatter.Serialize(fs, character);
        }
        Global.Debug("Файл персонажа сохранен: " + character.Name);
    }
    #endregion
}