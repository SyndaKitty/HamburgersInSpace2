using UnityEngine;
using System.Collections.Generic;
using System;

public class CameraZone : MonoBehaviour {
    public List<Bounds> Bounds;
    
    public Transform CameraAnchor;
    public float Speed;
    public float Acceleration;
    public float CameraSize;
    public float Dampening = 0.96f;
    CameraController controller;
    Camera cam;
    Vector3 velocity = Vector2.zero;

    void Start() {
        controller = FindObjectOfType<CameraController>();
        cam = controller.GetComponent<Camera>();
    }

    void Update() {
        if (Game.Instance.Player) {
            var pos = Game.Instance.Player.transform.position;
            bool inBounds = false;
            foreach (var bound in Bounds) {
                var x = pos.x >= bound.UpperLeft.position.x && pos.x <= bound.LowerRight.position.x;
                var y = pos.y >= bound.LowerRight.position.y && pos.y <= bound.UpperLeft.position.y;
                if (x && y) {
                    inBounds = true;
                    break;
                }
            }

            if (inBounds) {
                controller.InBounds();
                var targetPos = CameraAnchor.position;
                targetPos.z = cam.transform.position.z;
                
                var diff = (targetPos - cam.transform.position);
                diff.z = 0;
                diff.Normalize();
                velocity += diff * Speed * Time.deltaTime;

                cam.transform.position += velocity * Acceleration * Time.deltaTime;
                velocity *= Dampening;

                var sizeDiff = CameraSize - cam.orthographicSize;
                cam.orthographicSize += sizeDiff * Time.deltaTime * Speed;
            }
        }
    }
}

[Serializable]
public class Bounds {
    public Transform UpperLeft;
    public Transform LowerRight;
}