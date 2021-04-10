using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ThreeStageElevator))]
public class ThreeStageElevatorEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        ThreeStageElevator elevator = (ThreeStageElevator) target;
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Missing Targets"))
        {
            elevator.GenerateMissingTargets();
        }
        
        if (GUILayout.Button("Generate Trigger"))
        {
            elevator.GenerateTrigger();
        }
        
        if (GUILayout.Button("Generate Switch"))
        {
            elevator.GenerateSwitch();
        }
    }
}