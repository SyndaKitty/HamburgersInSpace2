using TMPro;
using UnityEngine;

public class CustomTextField : MonoBehaviour {
    public string Prefix;
    public string Postfix;
    public string Property;
    
    TextMeshPro text;

    void Start() {
        text = GetComponent<TextMeshPro>();
        Game.Instance.OnPropertyChanged += PropertyChanged;
    }   

    void OnDestroy() {
        Game.Instance.OnPropertyChanged -= PropertyChanged;
    }

    void PropertyChanged(string prop, string val) {
        if (prop != Property) return;
        text.text = $"{Prefix}{val}{Postfix}";
    }
}
