using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private Vector3 prevPos;
    public Vector2 velocity;

    private void FixedUpdate()
    {
        velocity = transform.position - prevPos;
        prevPos = transform.position;
    }
}
