using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;

public class LocaleDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    // Public variable to assign the image in the Unity Editor
    public Sprite defaultLocaleImage;

    IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);

        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        // Generate list of available Locales
        var options = new List<TMP_Dropdown.OptionData>();
        int selected = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selected = i;

            // Add the option with the locale name and default image
            options.Add(new TMP_Dropdown.OptionData(locale.name, defaultLocaleImage));
        }
        dropdown.options = options;

        dropdown.value = selected;
        dropdown.onValueChanged.AddListener(LocaleSelected);
    }

    public void LocaleSelected(int index)
    {
        Debug.Log("Switching Locale to " + index);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
