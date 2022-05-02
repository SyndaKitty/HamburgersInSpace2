using UnityEngine;

public class Game : MonoBehaviour {
    public static float VolumeModifier = 1;
    public static PlayerController Player;

    Music music;
    void Start() {
        Player = FindObjectOfType<PlayerController>();
        
        music = FindObjectOfType<Music>();
        if (!music) return;
        music.PlayTrack(2);
    }
}