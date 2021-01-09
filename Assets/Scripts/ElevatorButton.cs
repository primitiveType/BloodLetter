using UnityEngine;
using UnityEngine.Serialization;

public class ElevatorButton : BaseInteractable, IInteractable
{
    [FormerlySerializedAs("Elevator")] [SerializeField] private Elevator m_Elevator;
    
    public Elevator Elevator
    {
        get => m_Elevator;
        set => m_Elevator = value;
    }

    protected override bool DoInteraction()
    {
        Elevator.Trigger();
        return true;
        //play button sounds
    }
}