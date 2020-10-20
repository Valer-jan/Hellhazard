using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [Range(.01f, .99f)]
    public float Lerp;
    public Transform StartPosition;

    Transform position;

    private void Start()
    {
        transform.position = StartPosition.position;
        transform.rotation = StartPosition.rotation;
        position = transform;
    }

    public void NewPosition(Transform Position)
    {
        position = Position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, position.position, Lerp);
        transform.rotation = Quaternion.Lerp(transform.rotation, position.rotation, Lerp);
    }
}
