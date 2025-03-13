using Assets.Scripts;
using TMPro;
using UnityEngine;

public class CustomTextField : MonoBehaviour
{
    public string Prefix;
    public string Postfix;

    public bool Round;

    TextMeshProUGUI _text;

    TextMeshProUGUI Text
    {
        get
        {
            if (_text == null)
            {
                _text = GetComponent<TextMeshProUGUI>();
            }
            return _text;
        }
    }

    void Start()
    {
        if (Round)
        {
            EventBus.OnNextWave += Changed;
        }
    }

    void Changed(int val)
    {
        Text.text = $"{Prefix}{val}{Postfix}";
    }
}
