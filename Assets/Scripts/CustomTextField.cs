using TMPro;
using UnityEngine;

public class CustomTextField : MonoBehaviour {
    public string Prefix;
    public string Postfix;
    
    public bool Round;
    
    TextMeshProUGUI _text;

    TextMeshProUGUI Text {
        get {
            if (_text == null) {
                _text = GetComponent<TextMeshProUGUI>();
            }
            return _text;
        }
    }

    void Start() {
        if (Round) {
            Game.Instance.OnNextRound += Changed;
        }
    }

    void Changed(string val) {
        Text.text = $"{Prefix}{val}{Postfix}";
    }
}
