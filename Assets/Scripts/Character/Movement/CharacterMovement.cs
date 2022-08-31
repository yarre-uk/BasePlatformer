using Assets.Scripts;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    #region Variables
    private Rigidbody2D body;
    private PlayerState playerState;
    private GrabDetection playerTest;
    private PlayerStands playerStands;
    private GameController gameController;

    [Header("Movement Stats")]
    [SerializeField] private float maxSpeed = 11f;
    [SerializeField] private float maxAcceleration = 100f;
    [SerializeField] private float maxDecceleration = 100f;
    [SerializeField] private float maxTurnSpeed = 150f;
    [SerializeField] private float friction = 0;

    [Header("Jumping Stats")]
    [SerializeField] private float jumpHeight = 5;
    [SerializeField] private float timeToJumpApex = 0.5f;

    [Header("Gravs")]
    [SerializeField] private float GravMultiplier = 1;
    [SerializeField] private float FallGrav = 1.25f;
    [SerializeField] private float LowJumpGrav = 3f;

    [Header("Constants")]
    [SerializeField] private float bufferTime = 0.15f;
    [SerializeField] private float wallTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float forse = -1; // rename
    [SerializeField] private float slower = 0.25f; // rename

    [Header("Calculations")]
    private int direction;
    private int input;
    private float jumpSpeed;
    private float bufferTimer;
    private float wallTimer;
    private float coyoteTimer;
    private bool desiredJump;
    private bool desiredFall;
    private Vector2 velocity;
    private Vector2 velocityMov;
    private Vector2 desiredVelocity;
    #endregion

    #region UnityEvent
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.sharedMaterial = new PhysicsMaterial2D() { friction = 0 };

        playerTest = GetComponent<GrabDetection>();
        playerState = GetComponent<PlayerState>();
        playerStands = GetComponent<PlayerStands>();
        gameController = GameController.Get();
    }

    private void Update()
    {
        GetInput();
        Timers();

        if (input != 0)
        {
            transform.localScale = new Vector3(input > 0 ? 1 : -1, 1, 1);
        }

        desiredVelocity = new Vector2(input, 0) * Mathf.Max(maxSpeed - friction, 0f);
    }

    private void FixedUpdate()
    {
        if (gameController.CanRun)
        {
            velocityMov = body.velocity;
            RunWithAcceleration();
        }

        CalculatePhysics();

        if (desiredJump)
        {
            Jumps();
        }

        var check = gameController.GrabToWalls && playerState.OnWall() && !desiredFall;

        if (check && body.velocity.y < forse)
        {
            GrapToWall();
        }

        if (check && wallTimer < wallTime && !playerState.OnGround())
        {
            gameController.CanRun = false;
        }

        else
        {
            gameController.CanRun = true;
        }
    }
    #endregion

    #region Movement
    private void Jumps()
    {
        if ((gameController.CanJumpOffWall ||
            (gameController.CanJumpOffGraps && playerState.OnGrap())) &&
            direction != 0 &&
            playerState.OnWall())
        {
            WallJump();
        }

        if (playerState.OnGround() ||
            (gameController.CanJumpOffGraps && playerState.OnGrap()) ||
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

    private void RunWithAcceleration()
    {
        //float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        //float deceleration = onGround ? maxDecceleration : maxAirDeceleration;
        //float turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;
        float maxSpeedChange;


        if (input != 0)
        {
            if (Mathf.Sign(input) != Mathf.Sign(velocityMov.x))
            {
                maxSpeedChange = maxTurnSpeed * Time.fixedDeltaTime;
            }

            else
            {
                maxSpeedChange = maxAcceleration * Time.fixedDeltaTime;
            }
        }

        else
        {
            maxSpeedChange = maxDecceleration * Time.fixedDeltaTime;
        }

        velocityMov.x = Mathf.MoveTowards(velocityMov.x, desiredVelocity.x, maxSpeedChange);
        body.velocity = velocityMov + playerStands.velocity;
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