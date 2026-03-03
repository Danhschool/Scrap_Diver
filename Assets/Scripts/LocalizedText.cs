using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    public string key;
    private TextMeshProUGUI textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateText();
    }

    private void OnEnable()
    {
        LanguageManager.OnLanguageChanged += UpdateText;
        UpdateText();
    }

    private void OnDisable()
    {
        LanguageManager.OnLanguageChanged -= UpdateText;
    }

    public void UpdateText()
    {
        if (LanguageManager.Instance != null && !string.IsNullOrEmpty(key))
        {
            textComponent.text = LanguageManager.Instance.GetText(key);
            //Debug.Log($"Updated text for key: {key} to: {textComponent.text}");
        }
    }

    public void SetKeyAndUpdate(string newKey)
    {
        key = newKey;
        UpdateText();
    }
}