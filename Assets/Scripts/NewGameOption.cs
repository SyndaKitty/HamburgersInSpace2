using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NewGameOption : MonoBehaviour {
    Rigidbody2D rb;
    
    public Vector3 InitialPosition;
    public Vector2 StartingVelocity;
    public Transform Anchor;
    public float SpringWeight;
    public float Stretch;
    public float StretchSpeed;
    public TextMeshPro Text;
    public float CircleSize;
    public float AnimSpeed;

    float xScale;

    Vector3 velocity;
    Vector3 anchorStart;
    float t;
    Collider2D col;

    void Start() {
        
        transform.position = InitialPosition;
        velocity = StartingVelocity;
        xScale = Stretch;
        anchorStart = Anchor.position;
        col = GetComponent<Collider2D>();
    }

    void Update() {
        t += AnimSpeed * Time.deltaTime;

        xScale += StretchSpeed * Time.deltaTime;
        xScale = Mathf.Max(1, xScale);
        transform.localScale = new Vector3(xScale, 1, 1);
        var collider = Physics2D.OverlapBox(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.one * 0.05f, 0);
        if (collider != null && collider == col) {
            Text.fontSize = 9;
            Text.fontStyle = FontStyles.Bold;
            if (Input.GetMouseButtonDown(0)) {
                SceneManager.LoadScene(1, LoadSceneMode.Single);
            }
        }
        else {
            Text.fontSize = 8;
            Text.fontStyle = FontStyles.Normal;
        }
        Vector3 force = SpringWeight * (Anchor.position - transform.position);
        velocity += force * Time.deltaTime;
        velocity *= 0.99f;

        transform.position += velocity * Time.deltaTime;

        Anchor.position = anchorStart + new Vector3(Mathf.Cos(t), Mathf.Sin(t), 0) * CircleSize;
    }
}