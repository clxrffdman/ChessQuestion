using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class ChessManager : UnitySingleton<ChessManager>
{
    public enum TurnState {FriendlyUnitSelect, FriendlySpaceSelect, EnemyUnitSelect, EnemySpaceSelect };

    [Header("Serialized Fields")]
    public CameraController cameraController;
    public Tilemap validTilemap;
    public Tilemap visibleTilemap;
    [SerializeField] private TileBase validTile;
    [SerializeField] private TileBase selectedTile;
    [SerializeField] private TileBase availableTile;
    public int tileCheckSize = 50;

    [Header("Current Board State")]
    public List<Vector2Int> validTiles = new List<Vector2Int>();
    public List<Vector3Int> currentlyAvailableTiles = new List<Vector3Int>();
    public Dictionary<Vector3Int, BaseUnit> allUnits = new Dictionary<Vector3Int, BaseUnit>();
    public List<BaseUnit> allFriendlyUnits;
    public List<BaseUnit> allHostileUnits;
    public Vector3Int currentSelectedTilePos = new Vector3Int(0,0,-1);
    public BaseUnit selectedUnit;
    public bool isPlayerTurn = true;
    public TurnState currentTurnState = TurnState.FriendlyUnitSelect;

    [Header("Board Stages")]
    public int currentBoardStage;
    public List<BoardStageBehaviour> boardStages;

    [Header("Events")]
    public UnityEvent OnUnitSelectedEvent;

    public override void Awake()
    {
        base.Awake();
        cameraController = GetComponent<CameraController>();
    }

    public void Start()
    {
        AddValidTiles(validTile);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AttemptSelectTile(cameraController.GetClickedTile());
            
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            DebugDisplaySelectableTiles();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddValidTiles(availableTile);
        }
    }

    public void ClearSelectableTiles()
    {
        foreach (Vector3Int tile in currentlyAvailableTiles)
        {
            visibleTilemap.SetTile(tile, null);
        }
        currentlyAvailableTiles.Clear();
    }

    public void DisplaySelectableTiles()
    {
        if (selectedUnit == null || selectedUnit.faction != BaseUnit.Faction.Friendly)
        {
            ClearSelectableTiles();
            return;
        }

        selectedUnit.SetStartingTile();

        ClearSelectableTiles();

        
        foreach (Vector3Int tile in selectedUnit.GetValidTiles())
        {
            currentlyAvailableTiles.Add(tile);
            visibleTilemap.SetTile(tile, availableTile);
        }
    }

    public void DebugDisplaySelectableTiles()
    {
        if (selectedUnit == null)
        {
            ClearSelectableTiles();
            return;
        }

        selectedUnit.SetStartingTile();

        ClearSelectableTiles();


        foreach (Vector3Int tile in selectedUnit.GetValidTiles())
        {
            currentlyAvailableTiles.Add(tile);
            visibleTilemap.SetTile(tile, availableTile);
        }
    }

    public void AddValidTiles(TileBase tileToAdd)
    {
        for (int i = -tileCheckSize; i <= tileCheckSize; i++)
        {
            for (int j = -tileCheckSize; j <= tileCheckSize; j++)
            {
                Vector3Int posToCheck = new Vector3Int(i, j, 0);
                if (validTilemap.GetTile(posToCheck) == tileToAdd)
                {
                    validTiles.Add(new Vector2Int(posToCheck.x, posToCheck.y));

                }

            }
        }
    }

    public bool CheckValidTile(Vector3Int tilePos)
    {
        if (!validTiles.Contains((Vector2Int)tilePos))
        {
            return false;
        }


        return true;
    }

    public BaseUnit CheckForUnitAtTile(Vector3Int tilePos)
    {
        BaseUnit rv = null;

        if (!allUnits.ContainsKey(tilePos))
        {
            return rv;
        }

        rv = allUnits[tilePos];
        return rv;

    }

    public void SetNewSelectedUnit(BaseUnit newSelectedUnit)
    {
        selectedUnit = newSelectedUnit;
        DisplaySelectableTiles();
        OnUnitSelectedEvent.Invoke();
    }

    public void AttemptSelectTile(Vector3Int tilePos)
    {
        if(tilePos == currentSelectedTilePos)
        {
            return;
        }

        if (!CheckValidTile(tilePos))
        {
            Debug.Log("Invalid Tile Position!");
            return;
        }

        visibleTilemap.SetTile(currentSelectedTilePos, null);
        currentSelectedTilePos = tilePos;
        visibleTilemap.SetTile(tilePos, selectedTile);

        BaseUnit unitAtTile = CheckForUnitAtTile(tilePos);

        

        switch (currentTurnState)
        {
            case TurnState.FriendlyUnitSelect:
                if (unitAtTile == null)
                {
                    return;
                }

                SetNewSelectedUnit(unitAtTile);
                currentTurnState = TurnState.FriendlySpaceSelect;

                break;
            case TurnState.FriendlySpaceSelect:

                if (currentlyAvailableTiles.Contains(tilePos))
                {
                    if(unitAtTile != null && unitAtTile.faction != BaseUnit.Faction.Friendly)
                    {
                        selectedUnit.Attack(unitAtTile);

                        if (!unitAtTile.isAlive)
                        {
                            selectedUnit.MoveToTilePos(tilePos);
                        }
                    }
                    else
                    {
                        selectedUnit.MoveToTilePos(tilePos);
                    }

                    currentSelectedTilePos = new Vector3Int(0, 0, -1);
                    SetNewSelectedUnit(null);
                    currentTurnState = TurnState.FriendlyUnitSelect;
                    break;
                }
                
                if(unitAtTile != null)
                {
                    SetNewSelectedUnit(unitAtTile);
                }


                break;

        }

        

    }



}
