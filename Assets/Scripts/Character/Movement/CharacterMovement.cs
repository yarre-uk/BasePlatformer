using Assets.Scripts;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    #region Variables
    private Rigidbody2D body;
    private PlayerState playerState;
    private PlayerStands playerStands;

    [Header("Movement Stats")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float maxAcceleration = 52f;
    [SerializeField] private float maxDecceleration = 52f;
    [SerializeField] private float maxTurnSpeed = 80f;
    [SerializeField] private float maxAirAcceleration;
    [SerializeField] private float maxAirDeceleration;
    [SerializeField] private float maxAirTurnSpeed = 80f;
    [SerializeField] private float friction;

    [Header("Calculations")]
    private Vector2 velocity;
    private Vector2 desiredVelocity;
    private float input;
    public bool CanRun { get; set; }
    #endregion

    #region UnityEvents
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerState = GetComponent<PlayerState>();
        playerStands = GetComponent<PlayerStands>();
    }

    private void Update()
    {
        GetInput();

        if (input != 0)
        {
            transform.localScale = new Vector3(input > 0 ? 1 : -1, 1, 1);
        }

        desiredVelocity = new Vector2(input, 0) * Mathf.Max(maxSpeed - friction, 0f);
    }

    private void FixedUpdate()
    {
        if (CanRun)
        {
            velocity = body.velocity;
            RunWithAcceleration();
        }
    }
    #endregion

    #region Run
    private void RunWithAcceleration()
    {
        var onGround = playerState.OnGround();
        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        float deceleration = onGround ? maxDecceleration : maxAirDeceleration;
        float turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;
        float maxSpeedChange;


        if (input != 0)
        {
            if (Mathf.Sign(input) != Mathf.Sign(velocity.x))
            {
                maxSpeedChange = turnSpeed * Time.fixedDeltaTime;
            }

            else
            {
                maxSpeedChange = acceleration * Time.fixedDeltaTime;
            }
        }

        else
        {
            maxSpeedChange = deceleration * Time.fixedDeltaTime;
        }

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        body.velocity = velocity + playerStands.velocity;
    }
    #endregion

    #region Privates
    private void GetInput()
    {
        input = Input.GetAxisRaw("Horizontal");
    }
    #endregion
}