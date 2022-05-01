using UnityEngine;

public class Game : MonoBehaviour {
    Music music;
    void Start() {
        music = FindObjectOfType<Music>();
        if (!music) return;
        music.PlayTrack(2);
    }
}