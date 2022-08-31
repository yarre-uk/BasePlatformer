using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    #region Variables
    [Header("Lenghts")]
    [SerializeField]
    private float groundLength;
    [SerializeField]
    private float wallLength;

    [Header("Positions")]
    public Vector3 groungOfset;
    public Vector3 leftOfset;
    public Vector3 rightOfset;

    [Header("Layer Masks")]
    [SerializeField]
    private LayerMask groundLayer;
    #endregion

    #region Collisions
    public bool OnGround()
    {
        var ray = Physics2D.Raycast(transform.position + groungOfset,
            Vector2.right,
            groundLength,
            groundLayer);

        return ray;
    }

    public bool OnLeftWall()
    {
        var ray = Physics2D.Raycast(transform.position + leftOfset, Vector2.up, wallLength, groundLayer);

        return ray;
    }

    public bool OnRightWall()
    {
        var ray = Physics2D.Raycast(transform.position + rightOfset, Vector2.up, wallLength, groundLayer);

        return ray;
    }

    public bool OnWall()
    {
        return OnLeftWall() || OnRightWall();
    }
    #endregion

    #region Gizmos
    private void SetColor(bool condition)
    {
        if (condition)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
    }

    private void OnDrawGizmos()
    {
        SetColor(OnGround());
        Gizmos.DrawLine(transform.position + groungOfset, transform.position + groungOfset + Vector3.right * groundLength);

        SetColor(OnLeftWall());
        Gizmos.DrawLine(transform.position + leftOfset, transform.position + leftOfset + Vector3.up * wallLength);

        SetColor(OnRightWall());
        Gizmos.DrawLine(transform.position + rightOfset, transform.position + rightOfset + Vector3.up * wallLength);
    }
    #endregion
}