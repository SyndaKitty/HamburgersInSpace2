using UnityEngine;

public class CameraController : MonoBehaviour {
    public float Speed;

    Vector2 velocity;

    void Update() {
        if (Game.Instance.Player) {
            var newpos = Vector3.MoveTowards(transform.position, Game.Instance.Player.transform.position, Speed * Time.deltaTime);
            newpos.z = transform.position.z;
            transform.position = newpos;
        }
    }
}
