using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerState : MonoBehaviour
    {
        #region Variables
        private CollisionDetector collisionDecetor;
        private GrabDetection playerTest;
        private Rigidbody2D body;

        [Header("States")]
        public bool onGround;
        public bool onLeftWall;
        public bool onRightWall;
        public bool onLeftGrab;
        public bool onRightGrab;
        public bool jumping;
        public bool falling;
        #endregion

        #region UnityEvent
        private void Awake()
        {
            collisionDecetor = GetComponent<CollisionDetector>();
            body = GetComponent<Rigidbody2D>();
            playerTest = GetComponent<GrabDetection>();
        }

        private void FixedUpdate()
        {
            StateCheck();
        }
        #endregion

        #region Walls
        public bool OnWall()
        {
            return OnLeftWall() || OnRightWall();
        }

        public bool OnLeftWall()
        {
            return onLeftWall || onLeftGrab;
        }

        public bool OnRightWall()
        {
            return onRightWall || onRightGrab;
        }
        #endregion

        #region Ground
        public bool OnGround()
        {
            return onGround;
        }
        #endregion

        #region Graps
        public bool OnGrap()
        {
            return OnLeftGrap() || OnRightGrap();
        }

        public bool OnLeftGrap()
        {
            if (playerTest.Grab != null)
            {
                return playerTest.Grab.GetComponent<Grab>()
                    .Parent.transform.localScale.x == 1f;
            }

            if (playerTest.Edge != null)
            {
                return playerTest.Edge.GetComponent<Edge>()
                    .Parent.transform.localScale.x == 1f;
            }

            return false;
        }

        public bool OnRightGrap()
        {
            if (playerTest.Grab != null)
            {
                return playerTest.Grab.GetComponent<Grab>()
                    .Parent.transform.localScale.x == -1f;
            }

            if (playerTest.Edge != null)
            {
                return playerTest.Edge.GetComponent<Edge>()
                    .Parent.transform.localScale.x == -1f;
            }

            return false;
        }
        #endregion

        private void StateCheck()
        {
            SetStatesToFalse();

            if (collisionDecetor.OnGround())
            {
                onGround = true;
                return;
            }

            if (OnGrap())
            {
                if (OnLeftGrap())
                {
                    onLeftWall = true;
                    onLeftGrab = true;
                    return;
                }

                if (OnRightGrap())
                {
                    onRightWall = true;
                    onRightGrab = true;
                    return;
                }

                return;
            }

            if (collisionDecetor.OnWall())
            {
                if (collisionDecetor.OnLeftWall())
                {
                    onLeftWall = true;
                    return;
                }

                if (collisionDecetor.OnRightWall())
                {
                    onRightWall = true;
                    return;
                }

                return;
            }

            if (body.velocity.y < -0.01f)
            {
                falling = true;
                return;
            }

            if (body.velocity.y > 0.01f)
            {
                jumping = true;
                return;
            }
        }

        private void SetStatesToFalse()
        {
            onGround = false;
            onLeftWall = false;
            onRightWall = false;
            onLeftGrab = false;
            onRightGrab = false;
            jumping = false;
            falling = false;
        }
    }
}
