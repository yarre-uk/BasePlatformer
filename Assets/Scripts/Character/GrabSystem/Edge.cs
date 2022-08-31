using UnityEngine;

public class Edge : MonoBehaviour
{
    public GameObject Parent;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<GrabDetection>().Edge = gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<GrabDetection>().Edge = null;
    }
}
