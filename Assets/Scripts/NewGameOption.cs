using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameOption : MonoBehaviour {
    Rigidbody2D rb;
    
    public Vector3 InitialPosition;

    public bool TriggerSetInitialPosition;
    public bool Clickable;

    public SpriteRenderer SpriteRendererSelected;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (Clickable) {
            if (Physics2D.OverlapBox(Input.mousePosition, Vector2.one * 0.05f, 0)) {
                SpriteRendererSelected.enabled = true;
                if (Input.GetMouseButtonDown(0)) {
                    SceneManager.LoadScene(1, LoadSceneMode.Single);
                }
            }
            else {
                SpriteRendererSelected.enabled = false;
            }
        }
        if (TriggerSetInitialPosition) {
            TriggerSetInitialPosition = false;
            transform.position = InitialPosition;
        }
    }
}