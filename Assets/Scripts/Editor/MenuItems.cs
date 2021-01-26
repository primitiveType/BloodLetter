using UnityEditor;
using UnityEngine;

public class MenuItems
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    private static void NewMenuOption()
    {
        PlayerPrefs.DeleteAll();
    }
    
    [MenuItem("GameObject/Create Elevator From This", false, 0)]
    private static void CreateElevatorFromModel()
    {
        var selected = (GameObject)Selection.activeObject;
        var rb = selected.GetComponent<Rigidbody>();
        if (!rb)
        {
            rb = selected.AddComponent<Rigidbody>();
        }

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.isKinematic = true;
        
        var parent = selected.transform.parent;
        
        var elevatorRoot = new GameObject($"Elevator - {selected.name}");
        elevatorRoot.transform.position = selected.transform.position;
        elevatorRoot.transform.SetParent(parent);
        
        selected.transform.SetParent(elevatorRoot.transform);

        var elevator =  elevatorRoot.AddComponent<ThreeStageElevator>();
        elevator.ElevatorTransform = selected.transform;
        
        elevator.GenerateMissingTargets();
    }
}