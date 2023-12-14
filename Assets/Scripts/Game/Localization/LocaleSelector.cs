using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Snakefun.Game.Localization
{
    public class LocaleSelector : MonoBehaviour
    {
        bool active;

        private void Awake()
        {
            int ID = PlayerPrefs.GetInt("LocaleKey", 0);
            ChangeLocale(ID);
        }

        public void ChangeLocale(int localeID)
        {
            if (active) return;
            StartCoroutine(SetLocale(localeID));
        }

        private IEnumerator SetLocale(int localeID)
        {
            active = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
            PlayerPrefs.SetInt("LocaleKey", localeID);
            PlayerPrefs.Save();
            active = false;
        }
    }
}