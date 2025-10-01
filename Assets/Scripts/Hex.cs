using UnityEngine;
using DG.Tweening;
using UnityEditor;

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
        Vector2 posToGo = GetNextPosition(direction);
        Tile tileToGo = ThereIsTile(posToGo);
        if (tileToGo != null)
        {
            if(!tileToGo.HasHex()) transform.DOMove(posToGo, 0.25f).SetEase(Ease.Linear).OnComplete(PerformMovement);
            else
            {
                ReturnToRestPos();
            }
        }
        else
        {
            transform.DOMove(posToGo, 0.25f).SetEase(Ease.Linear).OnComplete(HexFinish);
        }
    }

    void HexFinish()
    {
        Debug.Log("Exited the map!");
        Destroy(gameObject);
    }

    void ReturnToRestPos()
    {
        transform.DOMove(restTile.transform.position, 0.5f).SetEase(Ease.Linear);
    }

    public Vector2 GetNextPosition(Direction dir)
    {
        float xoffset = Mathf.Sqrt(3) / 2;
        float yoffset = 0.5f;
        switch (dir)
        {
            case Direction.D: return new Vector2(transform.position.x, transform.position.y - 2*yoffset);
            case Direction.DR: return new Vector2(transform.position.x + xoffset, transform.position.y - yoffset);
            case Direction.UR: return new Vector2(transform.position.x + xoffset, transform.position.y + yoffset);
            case Direction.U: return new Vector2(transform.position.x, transform.position.y + 2*yoffset);
            case Direction.UL: return new Vector2(transform.position.x - xoffset, transform.position.y + yoffset);
            case Direction.DL: return new Vector2(transform.position.x - xoffset, transform.position.y - yoffset);
            default: return Vector2.zero;
        }
    }

    public Tile ThereIsTile(Vector2 position)
    {
        int q = Mathf.RoundToInt(position.x * 2f / Mathf.Sqrt(3));

        // Step 2: get r depending on q parity
        int r;
        if (q % 2 == 0)
        {
            r = Mathf.RoundToInt(position.y);
        }
        else
        {
            r = Mathf.RoundToInt(position.y + 0.5f);
        }

        Vector2Int key = new Vector2Int(q, r);
        if (GameManager.tileDict.ContainsKey(key))
        {
            return GameManager.tileDict[key];
        }

        return null;
    }
}
