using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float SmoothTime;
    public float MaxSpeed;
    [Range(0f, 1f)]
    public float LeanAmount;

    Vector3 velocity;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        Vector3 target;
        if (Game.Instance.Player)
        {
            target = Game.Instance.Player.transform.position;
        }
        else if (Game.Instance.ActiveCheckpoint != null)
        {
            target = Game.Instance.ActiveCheckpoint.transform.position;
        }
        else
        {
            target = Vector3.zero;
        }

        target.z = transform.position.z;
        target.x *= LeanAmount;
        target.y *= LeanAmount;

        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, SmoothTime, MaxSpeed, Time.deltaTime);
    }
}