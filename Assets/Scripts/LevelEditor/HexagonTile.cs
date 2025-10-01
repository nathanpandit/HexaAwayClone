using UnityEngine;

public class HexagonTile : MonoBehaviour
{
    public HexagonTile[] neighboringTiles;
    public Vector2Int axialCoordinate;
    [SerializeField] private SpriteRenderer baseRenderer;
    [SerializeField] private SpriteRenderer hexRenderer;
    [SerializeField] private SpriteRenderer arrowRenderer;

    private void Awake()
    {
        neighboringTiles = new HexagonTile[6];
        // Prefer explicit name lookups
        Transform baseT = transform.Find("BaseSprite");
        Transform hexT = transform.Find("HexSprite");
        Transform arrowT = transform.Find("ArrowSprite");

        if (baseT != null) baseRenderer = baseT.GetComponent<SpriteRenderer>();
        if (hexT != null) hexRenderer = hexT.GetComponent<SpriteRenderer>();
        if (arrowT != null) arrowRenderer = arrowT.GetComponent<SpriteRenderer>();

        // Fallback: assign by child order if names not found (expected order: Hex - Arrow - Base)
        if (baseRenderer == null || hexRenderer == null || arrowRenderer == null)
        {
            int childCount = transform.childCount;
            if (childCount >= 3)
            {
                if (hexRenderer == null) hexRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
                if (arrowRenderer == null) arrowRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
                if (baseRenderer == null) baseRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
            }
        }

        if (arrowRenderer != null) arrowRenderer.enabled = false;
        //FORMAT: U - UR - DR - D - DL - UL
    }

    public Vector3 GetPositionOf(Direction direction)
    {
        Vector3 thisPos = transform.position;
        Vector3 posToReturn;
        switch (direction)
        {
            case Direction.U:
                posToReturn = thisPos + new Vector3(0, 1, 0);
                break;
            
            case Direction.UR :
                posToReturn = thisPos + new Vector3(0.866f, 0.5f, 0);
                break;
            
            case Direction.DR:
                posToReturn = thisPos + new Vector3(0.866f, -0.5f, 0);
                break;
            
            case Direction.D:
                posToReturn = thisPos + new Vector3(0, -1, 0);
                break;
            
            case Direction.DL:
                posToReturn = thisPos + new Vector3(-0.866f, -0.5f, 0);
                break;
            
            case Direction.UL:
                posToReturn = thisPos + new Vector3(-0.866f, 0.5f, 0);
                break;
            default:
                posToReturn = thisPos;
                Debug.Log("ENTERED DEFAULT CASE FOR DIRECTION SWITCH IN HEXAGONTILE");
                break;
        }
        return posToReturn;
    }

    public void ApplyHexVisual(bool hasHex, HexColor color, Direction direction, Color mappedColor)
    {
        if (hexRenderer != null)
        {
            hexRenderer.enabled = hasHex;
            if (hasHex)
            {
                hexRenderer.color = mappedColor;
            }
        }

        if (arrowRenderer != null)
        {
            bool showArrow = hasHex && direction != Direction.None;
            arrowRenderer.enabled = showArrow;
            if (showArrow)
            {
                float zRot = 0f;
                switch (direction)
                {
                    case Direction.U:
                        zRot = 0f;
                        break;
                    case Direction.UR:
                        zRot = -60f;
                        break;
                    case Direction.DR:
                        zRot = -120f;
                        break;
                    case Direction.D:
                        zRot = -180f;
                        break;
                    case Direction.DL:
                        zRot = -240f;
                        break;
                    case Direction.UL:
                        zRot = -300f;
                        break;
                }
                arrowRenderer.transform.localRotation = Quaternion.Euler(0, 0, zRot);
                arrowRenderer.color = Color.white;
            }
        }
    }
}
