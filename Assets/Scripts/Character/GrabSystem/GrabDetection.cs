using Assets.Scripts;
using UnityEngine;

public class GrabDetection : MonoBehaviour
{
    private Rigidbody2D body;
    private CharacterMovement characterMovement;

    private float timer;

    public GameObject Grab;
    public GameObject Edge;
    public GameController GameController;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    public void SetNull()
    {
        Grab = null;
        Edge = null;
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Vertical") == -1)
        {
            Grab = null;
        }
    }

    private void FixedUpdate()
    {
        if (!GameController.CanJumpOffGraps)
        {
            return;
        }

        if (Grab != null)
        {
            var diff = transform.position.y - Grab.transform.position.y;

            if (body.velocity.y < -0.01f && diff < 0.1)
            {
                characterMovement.CanRun = false;
                transform.position = Grab.transform.position;
                body.velocity = new Vector2();
                return;
            }
        }

        if (Edge != null)
        {
            var diff = transform.position.y - Edge.transform.position.y;
            var check = Input.GetAxisRaw("Vertical") == -1 && Input.GetAxisRaw("Horizontal") != 0;

            if ((body.velocity.y < -0.01f && diff < 0.1) || check)
            {
                characterMovement.CanRun = false;
                transform.position = Edge.transform.position;
                body.velocity = new Vector2();

                timer += Time.fixedDeltaTime;
            }

            else
            {
                timer = 0;
            }

            if (timer > 0.05 && Input.GetKeyDown(KeyCode.S))
            {
                Edge = null;
            }
        }

        else
        {
            timer = 0;
        }
    }
}
