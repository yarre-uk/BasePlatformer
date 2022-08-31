using UnityEngine;

namespace Assets.Scripts
{
    public class BaseCamera : MonoBehaviour
    {
        private Transform playerPos;
        private Transform cameraPos;

        [SerializeField, Range(1, 10)]
        private float Strengh;

        private void Awake()
        {
            cameraPos = GetComponent<Transform>();
            playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            cameraPos.position = new Vector3(playerPos.position.x, playerPos.position.y, -10);
        }

        private void FixedUpdate()
        {
            var a1 = cameraPos.position;
            var a2 = playerPos.transform.position;

            var res = Time.fixedDeltaTime * Strengh * (a2 - a1);

            cameraPos.Translate(res.x, res.y, 0);
        }
    }
}
