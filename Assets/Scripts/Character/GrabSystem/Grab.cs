using UnityEngine;

public class Grab : MonoBehaviour
{
    public GameObject Parent;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<GrabDetection>().Grab = gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<GrabDetection>().Grab = null;
    }
}
