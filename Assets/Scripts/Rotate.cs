using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float Speed;

    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, Speed * Time.deltaTime);
    }
}
