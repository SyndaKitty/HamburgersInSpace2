using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float SmoothTime;
    public float MaxSpeed;
    [Range(0f, 1f)]
    public float LeanAmount = 1;

    Vector3 velocity;
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        Vector3 target;
        if (Game.Instance.Player is not null)
        {
            target = Game.Instance.Player.transform.position;
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