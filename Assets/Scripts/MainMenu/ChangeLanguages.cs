using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class ChangeLanguages : MonoBehaviour
{
  private async void Start()
  {
    if (PlayerPrefs.GetInt("CurrentLanguageID") != 0)
    {
      if (GetComponent<TMP_Dropdown>() != null)
      {
        GetComponent<TMP_Dropdown>().value = PlayerPrefs.GetInt("CurrentLanguageID");
        StartCoroutine(SetLanguage(PlayerPrefs.GetInt("CurrentLanguageID")));
        await GetLocale.GetCardsLanguage(PlayerPrefs.GetInt("CurrentLanguageID"));
        await GetLocale.GetMissionsLanguage(PlayerPrefs.GetInt("CurrentLanguageID"));
      }
    }
  }

  public async void ChangeLanguage(TMP_Dropdown dropdown)
  {
    StartCoroutine(SetLanguage(dropdown.value));
    await GetLocale.GetCardsLanguage(dropdown.value);
    await GetLocale.GetMissionsLanguage(dropdown.value);
  }

  IEnumerator SetLanguage(int languageID)
  {
    yield return LocalizationSettings.InitializationOperation;
    LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageID];
    PlayerPrefs.SetInt("CurrentLanguageID", languageID);
  }
}
