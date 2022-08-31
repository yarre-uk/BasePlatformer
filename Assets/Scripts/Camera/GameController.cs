using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        [Header("Features")]
        public bool CanJumpOffWall;
        public bool CanJumpOffGraps;
        public bool GrabToWalls;

        [Header("Features")]
        public TMP_Text CanJumpOffWallText;
        public TMP_Text CanJumpOffGrapsText;
        public TMP_Text GrabToWallsText;

        private void Update()
        {
            CanJumpOffWallText.text = (CanJumpOffWall ? "+" : "-") + "WallJump";
            CanJumpOffGrapsText.text = (CanJumpOffGraps ? "+" : "-") + "GrapJump";
            GrabToWallsText.text = (GrabToWalls ? "+" : "-") + "GrabToWalls";

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                CanJumpOffWall = !CanJumpOffWall;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                CanJumpOffGraps = !CanJumpOffGraps;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                GrabToWalls = !GrabToWalls;
            }
        }
    }
}
