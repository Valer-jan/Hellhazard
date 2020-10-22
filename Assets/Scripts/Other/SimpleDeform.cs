using UnityEngine;

public class SimpleDeform : MonoBehaviour
{
    public Vector3 RotationSpeed;
    Quaternion startRot;

    private void Start()
    {
        startRot = transform.rotation;
    }

    private void Update()
    {
        transform.rotation = startRot * Quaternion.Euler(RotationSpeed);
    }
}
