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

    public bool Quit;
    public bool Clickable;

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

        if (Quit && Application.platform == RuntimePlatform.WebGLPlayer) {
            Text.enabled = false;
            gameObject.SetActive(false);
            enabled = false;
        }
    }

    void Update() {
        t += AnimSpeed * Time.deltaTime;

        xScale += StretchSpeed * Time.deltaTime;
        xScale = Mathf.Min(1, xScale);
        transform.localScale = new Vector3(xScale, 1, 1);
        
        if (!Clickable) return;
        var collider = Physics2D.OverlapBox(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.one * 0.05f, 0);
        if (collider != null && collider == col) {
            Text.fontSize = 9;
            Text.fontStyle = FontStyles.Bold;
            if (Input.GetMouseButtonDown(0)) {
                if (Quit) {
                    Application.Quit();
                }
                else {
                    SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
                }
            }
        }
        else {
            Text.fontSize = 8;
            Text.fontStyle = FontStyles.Normal;
        }
    }

    void FixedUpdate() {
        Vector3 force = SpringWeight * (Anchor.position - transform.position);
        velocity += force * Time.fixedDeltaTime;
        velocity *= 0.91f;

        transform.position += velocity * Time.fixedDeltaTime;

        Anchor.position = anchorStart + new Vector3(Mathf.Cos(t), Mathf.Sin(t), 0) * CircleSize;
    }
}