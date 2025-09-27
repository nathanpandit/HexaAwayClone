using UnityEngine;

public class HexagonTile : MonoBehaviour
{
    public HexagonTile[] neighboringTiles;
    public Vector2Int axialCoordinate;

    private void Awake()
    {
        neighboringTiles = new HexagonTile[6];
        //FORMAT: 0 - 1  - 2  - 3 - 4 -  5
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
}
