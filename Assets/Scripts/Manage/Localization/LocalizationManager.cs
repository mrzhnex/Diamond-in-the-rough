using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Manage.Localization
{
    public static class LocalizationManager
    {
        private static readonly List<Word> Words = new List<Word>()
        {
            new Word()
            {
                Russian = "Вниз",
                English = "Down"
            },
            new Word()
            {
                Russian = "Влево",
                English = "Left"
            },
            new Word()
            {
                Russian = "Вправо",
                English = "Right"
            },
            new Word()
            {
                Russian = "Вверх",
                English = "Up"
            },
            new Word()
            {
                Russian = "Загрузить",
                English = "Load"
            },
            new Word()
            {
                Russian = "Продолжить",
                English = "Continue"
            },
            new Word()
            {
                Russian = "Настройки",
                English = "Settings"
            },
            new Word()
            {
                Russian = "Выход",
                English = "Exit"
            },
            new Word()
            {
                Russian = "Новый персонаж",
                English = "New character"
            },
            new Word()
            {
                Russian = "Сохранить и выйти",
                English = "Save and exit"
            },
            new Word()
            {
                Russian = "Громкость",
                English = "Volume"
            },
            new Word()
            {
                Russian = "Разрешение",
                English = "Resolution"
            },
            new Word()
            {
                Russian = "Сложность",
                English = "Difficult"
            },
            new Word()
            {
                Russian = "Клавиши",
                English = "Keys"
            },
            new Word()
            {
                Russian = "Перевод языка в данный момент невозможен",
                English = "Language translation is currently not possible"
            },
            new Word()
            {
                Russian = "Язык успешно переведен",
                English = "Language successfully translated"
            },
            new Word()
            {
                Russian = "уровень",
                English = "level"
            },
            new Word()
            {
                Russian = "Громкость",
                English = "Volume"
            },
            new Word()
            {
                Russian = "Эффекты",
                English = "Effects"
            },
            new Word()
            {
                Russian = "Музыка",
                English = "Music"
            },
            new Word()
            {
                Russian = "Движение",
                English = "Movement"
            },
            new Word()
            {
                Russian = "Сложная",
                English = "Hard"
            },
            new Word()
            {
                Russian = "Средняя",
                English = "Medium"
            },
            new Word()
            {
                Russian = "Легкая",
                English = "Easy"
            },
            new Word()
            {
                Russian = "Персонажи",
                English = "Characters"
            },
            new Word()
            {
                Russian = "Назад",
                English = "Back"
            },
            new Word()
            {
                Russian = "Основные",
                English = "Main"
            },
            new Word()
            {
                Russian = "Дополнительные",
                English = "Additional"
            },
            new Word()
            {
                Russian = "Создать",
                English = "Create"
            },
            new Word()
            {
                Russian = "Главное меню",
                English = "Main menu"
            },
            new Word()
            {
                Russian = "",
                English = ""
            },
            new Word()
            {
                Russian = "",
                English = ""
            },
            new Word()
            {
                Russian = "",
                English = ""
            },
            new Word()
            {
                Russian = "",
                English = ""
            },
            new Word()
            {
                Russian = "",
                English = ""
            },
            new Word()
            {
                Russian = "",
                English = ""
            },
            new Word()
            {
                Russian = "",
                English = ""
            },
            new Word()
            {
                Russian = "",
                English = ""
            },
            new Word()
            {
                Russian = "",
                English = ""
            },
            new Word()
            {
                Russian = "",
                English = ""
            },
            new Word()
            {
                Russian = "",
                English = ""
            },

        };

        public static string Translate(string word, Language language)
        {
            if (Words.Where(x => x.Russian == word).FirstOrDefault() == default)
                return word;
            return Words.Where(x => x.Russian == word).FirstOrDefault().Translate(language);
        }

    }

    public enum Language
    {
        Russian, English
    }
}   