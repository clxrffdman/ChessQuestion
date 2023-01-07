using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : BaseUnit
{

    public override List<Vector3Int> GetValidTiles()
    {
        List<Vector3Int> validTiles = new List<Vector3Int>();

        Vector3Int forwardOrientation = GetOrientation(Orientation.Forward);
        Vector3Int rightOrientation = GetOrientation(Orientation.Right);
        Vector3Int forward = currentTilePosition + forwardOrientation;
        Vector3Int forwardLeft = forward - rightOrientation;
        Vector3Int forwardRight = forward + rightOrientation;

        //Check forward left position for a validity and for a unit.
        BaseUnit unitToCheck = ChessManager.Instance.CheckForUnitAtTile(forwardLeft);
        if (ChessManager.Instance.CheckValidTile(forwardLeft) && unitToCheck != null && unitToCheck.faction != faction)
        {
            validTiles.Add(forwardLeft);
        }

        //Check forward position for a validity and for a unit.
        unitToCheck = ChessManager.Instance.CheckForUnitAtTile(forward);
        if (ChessManager.Instance.CheckValidTile(forward) && unitToCheck == null)
        {
            validTiles.Add(forward);
        }

        //Check forward right position for a validity and for a unit.
        unitToCheck = ChessManager.Instance.CheckForUnitAtTile(forwardRight);
        if (ChessManager.Instance.CheckValidTile(forwardRight) && unitToCheck != null && unitToCheck.faction != faction)
        {
            validTiles.Add(forwardRight);
        }

        if(timesMoved > 0)
        {
            return validTiles;
        }


        Vector3Int forwardTwo = currentTilePosition + (forwardOrientation*2);
        //Check forward position for a validity and for a unit.
        unitToCheck = ChessManager.Instance.CheckForUnitAtTile(forwardTwo);
        if (ChessManager.Instance.CheckValidTile(forwardTwo) && unitToCheck == null)
        {
            validTiles.Add(forwardTwo);
        }


        return validTiles;

    }
}
