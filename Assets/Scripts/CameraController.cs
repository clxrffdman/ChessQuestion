using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform vcamPosition;
    [SerializeField] private Camera pixelCamera;
    public ChessManager chessManager;
    private Plane clickablePlane;

    private void Awake()
    {
        clickablePlane = new Plane(Vector3.up, new Vector3(0, 0.55f, 0));
    }

    public Vector3Int GetClickedTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;
        Vector3Int selectedTilePos = Vector3Int.zero;

        if (clickablePlane.Raycast(ray, out enter))
        {
            //Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(enter);
            selectedTilePos = chessManager.validTilemap.WorldToCell(hitPoint);
        }

        return selectedTilePos;
    }

}
