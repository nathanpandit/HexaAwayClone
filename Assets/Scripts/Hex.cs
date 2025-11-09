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
    public bool isAnimating;

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
		if (isAnimating) return;
		Debug.Log($"I AM MOVING {restTile.q} {restTile.r}");
		var result = PathTest();
		float duration = result.forwardSteps * GameManager.unitDuration;
		if (result.forwardSteps > 0)
		{
			isAnimating = true;
			// If path is clear to exit, remove this hex from occupancy listing so it doesn't block others during movement
			if (result.allClear)
			{
				GameManager.hexes.Remove(this);
			}
			transform.DOMove(result.targetPos, duration).SetEase(Ease.Linear).OnComplete(() =>
			{
				if (result.allClear)
				{
					HexFinish();
				}
				else if (result.reachedRest)
				{
					// Reached a rest tile - update coordinates and restTile
					if (result.restTile != null)
					{
						q = result.restTile.q;
						r = result.restTile.r;
						restTile = result.restTile;
						Debug.Log($"Hex reached rest tile at ({q}, {r})");
					}
					isAnimating = false;
				}
				else
				{
					ReturnToRestPos(duration);
				}
			});
		}
		else
		{
			float shakeDuration = 0.2f;
			float shakeStrength = 0.1f;
			int vibrato = 10;
			isAnimating = true;
			transform.DOShakePosition(shakeDuration, shakeStrength, vibrato, 90f, false, true).OnComplete(() =>
			{
				isAnimating = false;
			});
		}
		GameManager.numberOfMoves--;
		LevelManager.Instance().HandleMoveText();
		if (GameManager.numberOfMoves < 1 && GameManager.hexes.Count != 0)
		{
			GameManager.LevelLost();
		}
    }

    void HexFinish()
    {
        Debug.Log("Exited the map!");
        // Trigger hex finished event - inventory increment and other handlers will respond
        // Use try-catch to ensure Destroy is always called even if event handler throws exception
        try
        {
            EventManager.Instance()?.OnHexFinished();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Exception in HexFinished event handler: {e.Message}");
        }
        
        // Always destroy the hex, regardless of event handler success
        Destroy(gameObject);
        HexParent.Instance().CheckChildrenChange();
    }

	void ReturnToRestPos(float duration)
	{
		isAnimating = true;
		transform.DOMove(restTile.transform.position, duration).SetEase(Ease.Linear).OnComplete(() =>
		{
			isAnimating = false;
		});
	}

	private Vector2 GetNextPositionFrom(Vector2 fromPos, Direction dir)
	{
		float xoffset = Mathf.Sqrt(3) / 2;
		float yoffset = 0.5f;
		switch (dir)
		{
			case Direction.D: return new Vector2(fromPos.x, fromPos.y - 2*yoffset);
			case Direction.DR: return new Vector2(fromPos.x + xoffset, fromPos.y - yoffset);
			case Direction.UR: return new Vector2(fromPos.x + xoffset, fromPos.y + yoffset);
			case Direction.U: return new Vector2(fromPos.x, fromPos.y + 2*yoffset);
			case Direction.UL: return new Vector2(fromPos.x - xoffset, fromPos.y + yoffset);
			case Direction.DL: return new Vector2(fromPos.x - xoffset, fromPos.y - yoffset);
			default: return Vector2.zero;
		}
	}

	private struct PathTestResult
	{
		public bool allClear;
		public int forwardSteps;
		public Vector2 targetPos;
		public bool reachedRest;
		public Tile restTile;
	}

	private PathTestResult PathTest()
	{
		Vector2 currentPos = transform.position;
		int steps = 0;
		while (true)
		{
			Vector2 nextPos = GetNextPositionFrom(currentPos, direction);
			Tile nextTile = ThereIsTile(nextPos);
			if (nextTile == null)
			{
				// Reached edge; path is clear to the edge, and we move one extra step off-map
				Vector2 offMapPos = nextPos;
				return new PathTestResult { allClear = true, forwardSteps = steps + 1, targetPos = offMapPos, reachedRest = false, restTile = null };
			}
			if (nextTile.HasHex())
			{
				// Blocked; stop before the blocker
				return new PathTestResult { allClear = false, forwardSteps = steps, targetPos = currentPos, reachedRest = false, restTile = null };
			}
			// Check if this tile is a rest tile - if so, stop here
			if (nextTile is Rest)
			{
				// Found a rest tile; stop movement here and update restTile
				return new PathTestResult { allClear = false, forwardSteps = steps + 1, targetPos = nextPos, reachedRest = true, restTile = nextTile };
			}
			// Empty tile, advance
			steps++;
			currentPos = nextPos;
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

    void SetRestTile(Tile tile)
    {
	    restTile = tile;
    }

    public void SelfDestruct()
    {
	    Destroy(gameObject);
    }
}
