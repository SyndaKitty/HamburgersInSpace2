using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;

    public float VolumeModifier = 1;
    public PlayerController Player;

    Music music;
    void Awake()
    {
        Instance = this;
    }
}