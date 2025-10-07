using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InputHandler : Singleton<InputHandler>
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.isPaused)
        {
            Vector3 mousePos = Input.mousePosition;
            
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            int q = Mathf.RoundToInt(worldPos.x * 2 / Mathf.Sqrt(3));
            int r;
            if (q % 2 == 0)
            {
                r = Mathf.RoundToInt(worldPos.y);
            }
            else
            {
                r = Mathf.RoundToInt(worldPos.y + 0.5f);
            }

            Hex hexToMove = GameManager.hexes.FirstOrDefault(h => h.restTile.q == q && h.restTile.r == r);
            if (hexToMove != null)
            {
                hexToMove.PerformMovement();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.isPaused)
        {
            ScreenManager.Instance().ShowScreen(ScreenType.PauseScreen);
            GameManager.PauseGame();
        }
    }
}
