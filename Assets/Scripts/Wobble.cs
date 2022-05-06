using UnityEngine;

public class Wobble : MonoBehaviour {
    public float Amount;
    public float Speed;
    public float TimeOffset;
    public float XWobbleMultiplier;
    public float YWobbleMultiplier;

    Vector2 initialOffset;
    float t;

    void Start() {
        initialOffset = transform.localPosition;
        t = TimeOffset;
    }

    void Update() {
        t += Speed * Time.deltaTime;

        transform.localPosition = initialOffset + new Vector2(Mathf.Cos(t * XWobbleMultiplier), Mathf.Sin(t * YWobbleMultiplier)) * Amount;
    }
}