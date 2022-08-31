using UnityEngine;

public class PlayerStands : MonoBehaviour
{
    public Vector2 velocity;

    [SerializeField]
    private Vector3 groungOfset;
    [SerializeField]
    private LayerMask groundLayer;
    public bool OnGround;

    private void FixedUpdate()
    {
        var ray = Physics2D.Raycast(transform.position + groungOfset,
            Vector2.right,
            1,
            groundLayer);

        if (!ray)
        {
            velocity = new Vector2();
            OnGround = false;
            return;
        }

        if (!ray.collider.TryGetComponent<MovingObject>(out var t))
        {
            velocity = new Vector2();
            OnGround = false;
            return;
        }

        OnGround = true;

        velocity = t.velocity;
        transform.Translate(velocity);
    }
}
