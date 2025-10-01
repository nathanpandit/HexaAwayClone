using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int q, r;

    public bool HasHex()
    {
        Hex hex = GameManager.hexes.FirstOrDefault(h => h.restTile.q == q && h.restTile.r == r);
        return hex != null;
    }
}
