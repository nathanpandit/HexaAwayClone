using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class LevelManager : Singleton<LevelManager>
{

    [SerializeField] public TextMeshProUGUI moveText;
    void Start()
    {
        ScreenManager.Instance().ShowScreen(ScreenType.MainMenu);
        Debug.Log("Start of LevelManager is called");
    }

    public void StartGame()
    {
        GameManager.StartLevel();
        HandleCameraSettings();
        HandleMoveText();
    }

    void HandleCameraSettings()
    {
        var tiles = TileParent.Instance().gameObject.GetComponentsInChildren<Tile>();
        if (tiles != null)
        {
            int minQ = 0;
            int maxQ = 0;
            int minR = 0;
            int maxR = 0;
            
            foreach (Tile tile in tiles)
            {
                if (tile.q < minQ)
                {
                    minQ = tile.q;
                }

                if (tile.q > maxQ)
                {
                    maxQ = tile.q;
                }

                if (tile.r < minR)
                {
                    minR = tile.r;
                }

                if (tile.r > maxR)
                {
                    maxR = tile.r;
                }
            }
            
            float minX = minQ * Mathf.Sqrt(3) / 2;
            float maxX = maxQ * Mathf.Sqrt(3) / 2;
            float minY, maxY;
            
            if (minQ % 2 == 0)
            {
                minY = minR;
            }
            else
            {
                minY = minR - 1 / 2f;
            }
            
            if (maxQ % 2 == 0)
            {
                maxY = maxR;
            }
            else
            {
                maxY = maxR - 1 / 2f;
            }

            // Compute bounds and desired camera settings
            float width = Mathf.Max(0.0001f, maxX - minX);
            float height = Mathf.Max(0.0001f, maxY - minY);

            // Tunables: uniform padding on all sides (do not zoom to tight fit)
            float padding = 0.6f;

            Camera cam = Camera.main;
            if (cam == null || !cam.orthographic)
            {
                return;
            }

            float aspect = (float)Screen.width / Mathf.Max(1, Screen.height);

            // Required ortho size to fit width: halfWidth + padding, then divide by aspect
            float requiredSizeByWidth = ((width * 0.5f) + padding) / Mathf.Max(0.0001f, aspect);

            // Vertical half-size must cover halfHeight + padding
            float requiredSizeByHeight = (height * 0.5f) + padding;

            float targetOrthoSize = Mathf.Max(requiredSizeByWidth, requiredSizeByHeight);

            // Center camera horizontally and vertically (no bottom offset)
            float centerX = (minX + maxX) * 0.5f;
            float centerY = (minY + maxY) * 0.5f;

            cam.orthographicSize = targetOrthoSize * (float)1.3;
            Vector3 camPos = cam.transform.position;
            camPos.x = centerX;
            camPos.y = centerY;
            cam.transform.position = camPos;
        }
    }

    public void HandleMoveText()
    {
        moveText = ScreenManager.Instance().transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        moveText.text = GameManager.numberOfMoves.ToString();
    }
}