using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class InputManager : Singleton<InputManager>
{
    
    public Vector3? lastMousePos;
    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Camera.main.orthographicSize -= scroll * 5;
        }

        if (Input.GetMouseButtonDown(2))
        {
            lastMousePos = Input.mousePosition; // store starting point
        }

        // While middle mouse button is held
        if (Input.GetMouseButton(2))
        {
            if (lastMousePos.HasValue)
            {
                Vector3 currentMousePos = Input.mousePosition;
                Vector3 delta = currentMousePos - lastMousePos.Value;

                // Only move if delta is non-zero
                if (delta != Vector3.zero)
                {
                    // Example: move camera/grid
                    Camera.main.transform.Translate(-delta.x * 0.01f, -delta.y * 0.01f, 0);
                }

                // Update lastMousePos for next frame
                lastMousePos = currentMousePos;
            }
        }

        // Reset when button is released
        if (Input.GetMouseButtonUp(2))
        {
            lastMousePos = null;
        }

        if (Input.GetMouseButton(0))
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
            
            if (LevelEditor.Instance().paintMode == PaintMode.Tile)
            {
                LevelEditor.Instance().CreateHexagonTileAt(q, r);
                if(LevelEditor.Instance().levelData.tileData.FirstOrDefault(x=>x.q == q && x.r == r) == null)
                {
                    HexagonTileData newData = new HexagonTileData(q,r);
                    LevelEditor.Instance().levelData.tileData.Add(newData);
                }
            }
            else if (LevelEditor.Instance().paintMode == PaintMode.Hex)
            {
                if (LevelEditor.Instance().TryGetTile(q, r, out var tile))
                {
                    HexagonTileData data = LevelEditor.Instance().GetOrCreateData(q, r);
                    if (!data.hasOther)
                    {
                        data.hasHex = true;
                        data.color = LevelEditor.Instance().selectedColor;
                        data.direction = LevelEditor.Instance().selectedDirection;
                        LevelEditor.Instance().ApplyVisualsToTileAt(q, r);
                    }
                }
            }
            else if (LevelEditor.Instance().paintMode == PaintMode.Rest)
            {
                if (LevelEditor.Instance().TryGetTile(q, r, out var tile))
                {
                    HexagonTileData data = LevelEditor.Instance().GetOrCreateData(q, r);
                    if (!data.hasHex)
                    {
                        data.hasHex = false;
                        data.hasOther = true;
                        data.otherType = OtherType.Rest;
                        data.color = HexColor.Black;
                        data.direction = Direction.None;
                        LevelEditor.Instance().ApplyVisualsToTileAt(q, r);
                    }
                }
            }
        }

        if (Input.GetMouseButton(1))
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
            if (LevelEditor.Instance().paintMode == PaintMode.Tile)
            {
                LevelEditor.Instance().RemoveHexagonTileAt(q, r);
            }
            else if (LevelEditor.Instance().paintMode == PaintMode.Hex)
            {
                if (LevelEditor.Instance().TryGetTile(q, r, out var tile))
                {
                    HexagonTileData data = LevelEditor.Instance().GetOrCreateData(q, r);
                    data.hasHex = false;
                    data.direction = Direction.None;
                    LevelEditor.Instance().ApplyVisualsToTileAt(q, r);
                }
            }
            else if (LevelEditor.Instance().paintMode == PaintMode.Rest)
            {
                if (LevelEditor.Instance().TryGetTile(q, r, out var tile))
                {
                    HexagonTileData data = LevelEditor.Instance().GetOrCreateData(q, r);
                    data.hasOther = false;
                    data.otherType = OtherType.None;
                    data.color = HexColor.None;
                    LevelEditor.Instance().ApplyVisualsToTileAt(q, r);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            LevelEditor.Instance().SaveLevel();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LevelEditor.Instance().LoadLevel();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            LevelEditor.Instance().level++;
            LevelEditor.Instance().LoadLevel();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (LevelEditor.Instance().level > 1)
            {
                LevelEditor.Instance().level--;
                LevelEditor.Instance().LoadLevel();
            }
        }
    }
}
