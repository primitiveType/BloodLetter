using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BloodLetterEditor
{
    [CustomEditor(typeof(Elevator))]
    public class ElevatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            Elevator elevator;
            elevator = target as Elevator;

            if (GUILayout.Button("Move to start"))
            {
                elevator.ElevatorTransform.position = elevator.StartTarget.position;
            }

            if (GUILayout.Button("Move to end"))
            {
                elevator.ElevatorTransform.position = elevator.EndTarget.position;
            }
        }
    }
}