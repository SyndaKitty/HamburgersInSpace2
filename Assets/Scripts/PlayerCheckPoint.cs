using UnityEngine;

public class PlayerCheckPoint : MonoBehaviour
{
    public Color InactiveTint;
    public Color ActiveTint;

    public SpriteRenderer InnerSr;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Game.Instance.SetActiveCheckPoint(this);
        }
    }

    public void SetActive()
    {
        InnerSr.color = ActiveTint;
    }

    public void SetInactive()
    {
        InnerSr.color = InactiveTint;
    }
}
