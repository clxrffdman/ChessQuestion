using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : BaseUnit
{

    public override List<Vector3Int> GetValidTiles()
    {
        List<Vector3Int> validTiles = new List<Vector3Int>() {
            currentTilePosition + new Vector3Int(1,2,0),
            currentTilePosition + new Vector3Int(2,1,0),
            currentTilePosition + new Vector3Int(2,-1,0),
            currentTilePosition + new Vector3Int(1,-2,0),
            currentTilePosition + new Vector3Int(-1,-2,0),
            currentTilePosition + new Vector3Int(-2,-1,0),
            currentTilePosition + new Vector3Int(-2,1,0),
            currentTilePosition + new Vector3Int(-1,2,0),
        };

        BaseUnit unitToCheck = null;

        for(int i = validTiles.Count - 1; i >= 0; i--)
        {
            unitToCheck = ChessManager.Instance.CheckForUnitAtTile(validTiles[i]);
            if (!ChessManager.Instance.CheckValidTile(validTiles[i]) || (unitToCheck != null && unitToCheck.faction == faction))
            {
                validTiles.RemoveAt(i);
            }
        }

        

        return validTiles;

    }
}
