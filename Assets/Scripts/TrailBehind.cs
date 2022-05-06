using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBehind : MonoBehaviour
{
    public Transform Target;
    public float Amount;

    Vector3 previous;

    void Start() {
        previous = Target.position;
    }

    void Update() {
        var diff = previous - Target.position;
        if (diff.magnitude < 0.01f) {
            transform.position = Target.position;
        }
        else {
            transform.position = Target.position + diff.normalized * Amount;
        }
        
        previous = Target.position;
    }
}
