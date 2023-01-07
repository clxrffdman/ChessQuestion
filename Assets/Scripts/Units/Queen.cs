using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : BaseUnit
{
    public int maxRange = 30;
    public override List<Vector3Int> GetValidTiles()
    {
        List<Vector3Int> validTiles = new List<Vector3Int>();

        List<Vector3Int> directionsToCheck = new List<Vector3Int>() {
            new Vector3Int(1,0),
            new Vector3Int(0,1),
            new Vector3Int(-1,0),
            new Vector3Int(0,-1),
            new Vector3Int(1,1),
            new Vector3Int(-1,1),
            new Vector3Int(1,-1),
            new Vector3Int(-1,-1),
        };

        BaseUnit unitToCheck = null;

        foreach (Vector3Int direction in directionsToCheck)
        {
            bool foundFinalTile = false;
            int checkedTiles = 1;
            while (!foundFinalTile && checkedTiles < maxRange)
            {
                Vector3Int posToCheck = currentTilePosition + (direction * checkedTiles);
                unitToCheck = ChessManager.Instance.CheckForUnitAtTile(posToCheck);
                if (!ChessManager.Instance.CheckValidTile(posToCheck))
                {
                    foundFinalTile = true;
                    break;
                }

                if(unitToCheck != null)
                {
                    if (unitToCheck.faction != faction)
                    {
                        validTiles.Add(posToCheck);
                    }

                    foundFinalTile = true;
                    break;
                }

                validTiles.Add(posToCheck);
                checkedTiles++;

            }

        }

        return validTiles;

    }
}
