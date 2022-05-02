using UnityEngine;

public class Game : MonoBehaviour {
    public static Game Instance;
    
    public float VolumeModifier = 1;
    public PlayerController Player;
    public OneShotSound ActivateCheckPointSound;

    PlayerCheckPoint ActiveCheckpoint;

    Music music;
    void Awake() {
        Instance = this;
    }

    void Start() {
        Player = FindObjectOfType<PlayerController>();
        
        music = FindObjectOfType<Music>();
        if (!music) return;
        music.PlayTrack(2);
    }

    public void SetActiveCheckPoint(PlayerCheckPoint checkpoint) {
        if (ActiveCheckpoint == checkpoint) {
            return;
        }
        if (ActiveCheckpoint) {
            ActiveCheckpoint.SetInactive();
        }
        checkpoint.SetActive();
        ActiveCheckpoint = checkpoint;
        if (ActivateCheckPointSound) {
            Instantiate(ActivateCheckPointSound);
        }
    }
}