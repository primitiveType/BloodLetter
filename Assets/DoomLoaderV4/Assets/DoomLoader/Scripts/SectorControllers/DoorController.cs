using UnityEditor;
using UnityEngine;

public abstract class DoorController : MonoBehaviour
{
    public float originalHeight;
    public float targetHeight;
    public float currentHeight;
    public abstract bool returns { get; }

    protected void AddElevatorButton(GameObject go, Elevator elevator, KeyType requiredKey)
    {
        var oldButton = go.GetComponent<ElevatorButton>();
        if (oldButton && oldButton.Elevator == elevator)
        {
            return;
        }
            
        var button = go.AddComponent<ElevatorButton>();
        button.Elevator = elevator;
        GameObjectUtility.SetStaticEditorFlags(go, 0);

        button.RequiredKeys = requiredKey;

    }

    protected void MakeInteractable(GameObject go)
    {
        go.layer = LayerMask.NameToLayer("Interactable");
    }

    protected bool IsInteractable { get; private set; }
    public void Init(Sector sector, KeyType requiredKey, bool isInteractable)
    {
        IsInteractable = isInteractable; 
        DoInitialize(sector);
        SetupRuntimeElevatorScripts(sector,  requiredKey);
        foreach (Transform child in transform)
        {//just make all children non-static.
            GameObjectUtility.SetStaticEditorFlags(child.gameObject, 0);
        }
    }

    protected abstract void DoInitialize(Sector sector);

    private void SetupRuntimeElevatorScripts(Sector sector, KeyType requiredKey)
    {
        if (gameObject.GetComponent<Elevator>())
        {
            Debug.Log($"{name} already has elevator");
            return;
        }

        //place elevator script on this.
        Elevator elevator = gameObject.AddComponent<Elevator>();
        elevator.ElevatorTransform = transform;
        elevator.speed = 2f;
        GameObject end = new GameObject($"{name} end target");
        end.transform.position = Vector3.up * (targetHeight - originalHeight);
        end.transform.SetParent(transform.parent);
        GameObject start = new GameObject($"{name} start target");
        start.transform.position = Vector3.zero;
        start.transform.SetParent(transform.parent);
        elevator.EndTarget = end.transform;
        elevator.StartTarget = start.transform;
        elevator.returns = returns;
        //place rigidbody on this
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (!rb)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true;
        rb.useGravity = false;
        if (IsInteractable)
        {
            //set layer to Interactable
            MakeInteractable(gameObject);
            //place elevator button script on ceiling and walls of sector
            AddElevatorButton(sector.ceilingObject, elevator, requiredKey);
            MakeInteractable(sector.ceilingObject);
            foreach (Sidedef s in sector.Sidedefs)
            {
                if (s.Line.Back != null)
                {
                    if (s.Line.Back.Sector == sector)
                    {
                        if (s.Line.TopFrontObject != null)
                        {
                            AddElevatorButton(s.Line.TopFrontObject, elevator, requiredKey);
                            MakeInteractable(s.Line.TopFrontObject);
                        }
                    }

                    if (s.Line.Front.Sector == sector)
                    {
                        if (s.Line.TopBackObject != null)
                        {
                            AddElevatorButton(s.Line.TopBackObject, elevator, requiredKey);
                            MakeInteractable(s.Line.TopBackObject);
                        }
                    }
                }
            }
        }
    }
}