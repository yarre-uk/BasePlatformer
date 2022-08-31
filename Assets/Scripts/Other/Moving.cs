using UnityEngine;

namespace Assets.Scripts
{
    public class Moving : MonoBehaviour
    {
        public Vector2 startPos;
        public Vector2 currentPos;
        public float velocity;
        public float top;
        public float bottom;
        public int direction;
        public bool vertical;

        private void Awake()
        {
            startPos = transform.position;
        }

        private void FixedUpdate()
        {
            currentPos = transform.position;

            if (vertical)
            {
                if (currentPos.y > startPos.y + top)
                {
                    direction = -1;
                }

                if (currentPos.y < startPos.y - bottom)
                {
                    direction = 1;
                }

                currentPos.y += velocity * direction * Time.fixedDeltaTime;
            }

            else
            {
                if (currentPos.x > startPos.x + top)
                {
                    direction = -1;
                }

                if (currentPos.x < startPos.x - bottom)
                {
                    direction = 1;
                }

                currentPos.x += velocity * direction * Time.fixedDeltaTime;
            }

            transform.position = currentPos;
        }
    }
}
