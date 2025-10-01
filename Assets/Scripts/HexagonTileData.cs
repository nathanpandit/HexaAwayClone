using UnityEngine;


[System.Serializable]
public class HexagonTileData
{
	public int q;
	public int r;
	public bool hasHex;
	public HexColor color;
	public Direction direction;

	public HexagonTileData(int _q, int _r)
	{
		q = _q;
		r = _r;
		hasHex = false;
		color = HexColor.Red;
		direction = Direction.None;
	}
}