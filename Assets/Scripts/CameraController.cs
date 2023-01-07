using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform vcamPosition;
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Camera pixelCamera;
    private Vector3 ogCamPos;
    public ChessManager chessManager;
    public ParticleSystem fogParticleSystem;
    private Plane clickablePlane;
    public float camMoveSpeed;
    public float minMouseCamDistance;

    private void Awake()
    {
        clickablePlane = new Plane(Vector3.up, new Vector3(0, 0.55f, 0));
        ogCamPos = vcamPosition.transform.position;
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

    private void Update()
    {
        MoveCamWithMouse();
    }

    public void MoveCamWithMouse()
    {
        Vector3 relativeMousePos = new Vector3((Input.mousePosition.x - (Screen.width/2))/(Screen.width / 2), 0, (Input.mousePosition.y - (Screen.height / 2))/(Screen.height / 2));
        Vector3 offset = relativeMousePos * camMoveSpeed;
        Vector3 moveOffset = offset * Time.deltaTime;

        if(offset.magnitude > minMouseCamDistance)
        {
            vcamPosition.position = pixelCamera.transform.position + moveOffset;
            var sh = fogParticleSystem.shape;
            sh.position = pixelCamera.transform.position - ogCamPos;
        }

        
    }


}
