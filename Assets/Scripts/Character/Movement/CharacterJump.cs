using Assets.Scripts;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    #region Variables
    private Rigidbody2D body;
    private PlayerState playerState;
    private GrabDetection playerTest;
    private CharacterMovement characterMovement;
    private Vector2 velocity;

    public GameController GameController;

    [Header("Jumping Stats")]
    public float jumpHeight;
    public float timeToJumpApex;

    [Header("Gravs")]
    public float GravMultiplier;
    public float FallGrav;
    public float LowJumpGrav;

    [Header("Calculations")]
    private int direction;
    private int input;
    private float jumpSpeed;
    private bool desiredJump;
    private bool desiredFall;
    private float bufferTimer;
    private float wallTimer;
    private float coyoteTimer;

    [Header("Constants")]
    [SerializeField] private float bufferTime;
    [SerializeField] private float wallTime;
    [SerializeField] private float coyoteTime;
    [SerializeField] private int forseToWall;
    [SerializeField] private float forse;
    [SerializeField] private float slower;
    #endregion

    #region UnityEvent
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.sharedMaterial = new PhysicsMaterial2D() { friction = 0 };

        characterMovement = GetComponent<CharacterMovement>();
        playerTest = GetComponent<GrabDetection>();
        playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        GetInput();
        Timers();
    }

    private void FixedUpdate()
    {
        CalculatePhysics();

        if (desiredJump)
        {
            Jumps();
        }

        var check = GameController.GrabToWalls && playerState.OnWall() && !desiredFall;

        if (check && body.velocity.y < forse)
        {
            GrapToWall();
        }

        if (check && wallTimer < wallTime && !playerState.OnGround())
        {
            characterMovement.CanRun = false;
        }

        else
        {
            characterMovement.CanRun = true;
        }
    }


    #endregion

    #region Jumps
    private void Jumps()
    {
        if ((GameController.CanJumpOffWall ||
            (GameController.CanJumpOffGraps && playerState.OnGrap())) &&
            direction != 0 &&
            playerState.OnWall())
        {
            WallJump();
        }

        if (playerState.OnGround() ||
            (GameController.CanJumpOffGraps && playerState.OnGrap()) ||
            coyoteTimer < coyoteTime)
        {
            GroundJump();
        }

        playerTest.SetNull();
        bufferTimer = 0;
        desiredJump = false;
    }

    public void GroundJump()
    {
        velocity = body.velocity;

        jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * body.gravityScale * jumpHeight);

        if (velocity.y > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
        }
        else if (velocity.y < 0f)
        {
            jumpSpeed += Mathf.Abs(body.velocity.y);
        }

        velocity.y += jumpSpeed;
        body.velocity = velocity;
    }

    private void WallJump()
    {
        if ((playerState.OnRightWall() && direction == -1) ||
            (playerState.OnLeftWall() && direction == 1) ||
            (playerState.OnRightGrap() && direction == -1) ||
            (playerState.OnLeftGrap() && direction == 1))
        {
            velocity = body.velocity;

            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * body.gravityScale * jumpHeight);

            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(body.velocity.y);
            }

            velocity.y += jumpSpeed;
            velocity.x += jumpSpeed * 0.3f * direction;
            body.velocity = velocity;
        }
    }
    #endregion

    #region Privates
    private void Timers()
    {
        if (input != 0 && playerState.OnWall())
        {
            wallTimer += Time.deltaTime;
        }

        else
        {
            wallTimer = 0;
        }

        if (!playerState.OnGround())
        {
            coyoteTimer += Time.deltaTime;
        }

        else
        {
            coyoteTimer = 0;
        }

        if (desiredJump)
        {
            bufferTimer += Time.deltaTime;

            if (bufferTimer > bufferTime)
            {
                desiredJump = false;
                bufferTimer = 0;
            }
        }
    }
    private void GrapToWall()
    {
        velocity = Vector2.Lerp(body.velocity,
                        new Vector2(body.velocity.x, forse), slower);

        body.velocity = velocity;
    }

    private void CalculatePhysics()
    {
        if (playerState.falling)
        {
            GravMultiplier = FallGrav;
        }

        else if (playerState.jumping && !Input.GetKey(KeyCode.Space))
        {
            GravMultiplier = LowJumpGrav;
        }

        else
        {
            GravMultiplier = 1;
        }

        var newGravity = (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
        body.gravityScale = (newGravity / Physics2D.gravity.y) * GravMultiplier;
    }

    private void GetInput()
    {
        input = (int)Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            direction = (int)Input.GetAxisRaw("Horizontal");
            desiredJump = true;
        }

        if (Input.GetAxisRaw("Vertical") == -1)
        {
            desiredFall = true;
        }

        else
        {
            desiredFall = false;
        }
    }
    #endregion
}