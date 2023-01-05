using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    public enum Faction { Friendly, Hostile };
    public enum Orientation { Forward, Right};

    [Header("Base Piece Information")]
    public string id;
    public string displayName;
    public string description;
    public float baseHealth;
    public Faction faction;
    public float pointValue;
    public bool isAlive = true;

    [Header("Current Piece State")]
    public Vector3Int currentTilePosition;
    public float currentHealth;

    public virtual void Start()
    {
        InitializeHealth();
        SetStartingTile();
        AddToManagerLists();

    }

    public virtual void AddToManagerLists()
    {
        ChessManager.Instance.allUnits.Add(currentTilePosition, this);

        if(faction == Faction.Friendly)
        {
            ChessManager.Instance.allFriendlyUnits.Add(this);
        }

        if (faction == Faction.Hostile)
        {
            ChessManager.Instance.allHostileUnits.Add(this);
        }
    }

    public virtual void SetStartingTile()
    {
        currentTilePosition = ChessManager.Instance.validTilemap.WorldToCell(transform.position);
    }

    public virtual List<Vector3Int> GetValidTiles()
    {
        return new List<Vector3Int>();
    }

    public virtual Vector3Int GetOrientation(Orientation direction)
    {
        if(direction == Orientation.Forward)
        {
            return new Vector3Int(Mathf.RoundToInt(transform.forward.x), Mathf.RoundToInt(transform.forward.z), 0);
        }

        if (direction == Orientation.Right)
        {
            return new Vector3Int(Mathf.RoundToInt(transform.right.x), Mathf.RoundToInt(transform.right.z), 0);
        }

        return new Vector3Int();

    }

    public virtual void MoveToTilePos(Vector3Int tilePos)
    {
        ChessManager.Instance.allUnits.RenameKey(currentTilePosition, tilePos);
        currentTilePosition = tilePos;
        transform.position = ChessManager.Instance.validTilemap.GetCellCenterWorld(tilePos);
    }

    //HEALTH STATUS-RELATED

    public virtual void InitializeHealth()
    {
        currentHealth = baseHealth;
    }

    public virtual void Attack(BaseUnit target)
    {
        Debug.Log(this.name + " attacked " + target.name + "!");
        target.TakeDamage(pointValue);
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, baseHealth);

        if(currentHealth <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        isAlive = false;
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

}
