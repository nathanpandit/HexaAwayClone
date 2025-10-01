using UnityEngine;

public class Hex : MonoBehaviour
{
    public Tile restTile;
    public int q, r;
    public HexColor color;
    public Direction direction;
    public SpriteRenderer hexRenderer, arrowRenderer;

    public void Awake()
    {
        Transform hexT = transform.Find("HexSprite");
        Transform arrowT = transform.Find("ArrowSprite");

        if (hexT != null) hexRenderer = hexT.GetComponent<SpriteRenderer>();
        if (arrowT != null) arrowRenderer = arrowT.GetComponent<SpriteRenderer>();
        
        
        if (hexRenderer == null || arrowRenderer == null)
        {
            int childCount = transform.childCount;
            if (childCount >= 2)
            {
                if (hexRenderer == null) hexRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
                if (arrowRenderer == null) arrowRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
            }
        }
    }

    public void Initialize(Tile _tile, int _q, int _r, HexColor _color, Direction _direction)
    {
        restTile = _tile;
        q = _q;
        r = _r;
        color = _color;
        hexRenderer.color = GameManager.colorDict[color];
        direction = _direction;
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

    public void PerformMovement()
    {
        Debug.Log($"I AM MOVING {restTile.q} {restTile.r}");
    }
}
