using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LanguageItem
{
    public string key;
    public string vi;
    public string en;
}

[Serializable]
public class LanguageData
{
    public LanguageItem[] items;
}

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }
    public static event Action OnLanguageChanged;

    private const string LanguagePrefKey = "SavedLanguage";

    public enum Language { vi, en }
    public Language currentLanguage = Language.en;

    private Dictionary<string, LanguageItem> localizedText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLanguageData();
            LoadSavedLanguage();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadSavedLanguage()
    {
        //int savedLang = PlayerPrefs.GetInt(LanguagePrefKey, (int)Language.en);
        //currentLanguage = (Language)savedLang;

        currentLanguage = (Language)DataManager.LanguagePref;
    }

    private void LoadLanguageData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("languages");
        if (jsonFile != null)
        {
            LanguageData data = JsonUtility.FromJson<LanguageData>(jsonFile.text);
            localizedText = new Dictionary<string, LanguageItem>();
            foreach (var item in data.items)
            {
                if (!localizedText.ContainsKey(item.key))
                {
                    localizedText.Add(item.key, item);
                }
            }
        }
    }

    public string GetText(string key)
    {
        if (localizedText != null && localizedText.ContainsKey(key))
        {
            return currentLanguage == Language.vi ? localizedText[key].vi : localizedText[key].en;
        }
        return key;
    }

    public void ChangeLanguage(Language newLanguage)
    {
        if (currentLanguage != newLanguage)
        {
            currentLanguage = newLanguage;

            //PlayerPrefs.SetInt(LanguagePrefKey, (int)currentLanguage);
            //PlayerPrefs.Save();
            DataManager.LanguagePref = (int)currentLanguage;

            OnLanguageChanged?.Invoke();
        }
    }
    public void NextLanguage()
    {
        Language[] langs = (Language[])Enum.GetValues(typeof(Language));
        int currentIndex = Array.IndexOf(langs, currentLanguage);
        int nextIndex = (currentIndex + 1) % langs.Length;

        ChangeLanguage(langs[nextIndex]);
    }
}