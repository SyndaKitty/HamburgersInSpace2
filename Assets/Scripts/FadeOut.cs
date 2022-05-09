using UnityEngine;

public class FadeOut : MonoBehaviour {
    public float Duration;
    public bool Shrink;

    public AnimationCurve Curve;

    float at;

    void Start() {
        
    }

    void Update() {
        at += Time.deltaTime;

        float t = Curve.Evaluate(at / Duration);

        if (at >= Duration) {
            Destroy(gameObject);
            return;
        }

        if (Shrink) {
            transform.localScale = Vector3.one * t;
        }
    }
}