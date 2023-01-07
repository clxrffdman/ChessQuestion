using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BoardStage
{
    public Collider cameraConfineCollider;
    public Collider fogCollider;
}

public class BoardStageBehaviour : MonoBehaviour
{
    public BoardStage stage;

    public virtual void EnableStage()
    {

    }

    public virtual void DisableStage()
    {

    }

}
