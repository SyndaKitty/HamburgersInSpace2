using UnityEngine;

public class CameraController : MonoBehaviour {
    public float Speed;

    public float OffsetSpeed;
    public float OffsetAcceleration;
    public float OffsetAmount = 1.5f;

    Vector2 velocity;
    Vector3 position;
    Vector3 offset;
    Vector3 targetOffset;

    void Update() {
        if (Game.Instance.Player) {
            var newpos = Vector3.MoveTowards(position, Game.Instance.Player.transform.position, Speed * Time.deltaTime);
            newpos.z = transform.position.z;
            position = newpos;

            
            var vel = Game.Instance.Player.LastStickPosition;
            if (vel.magnitude > 0.1f) {
                targetOffset = vel.normalized * OffsetAmount;
            }

            var diff = ((Vector3)targetOffset - offset) * OffsetSpeed;
            offset += diff * OffsetAcceleration * Time.deltaTime;

            transform.position = position + offset;
        }
    }
}
