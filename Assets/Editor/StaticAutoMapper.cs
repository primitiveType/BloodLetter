using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
 
namespace EditorExtensions.StaticMapper.Editor {
   /// <summary>
   /// Automatically adds static component to the gameobjects that contain colliders and are static
   /// </summary>
   public class StaticAutomapper {
      private const float CheckIntervals = 2f;
   
      #region [Fields]
 
      private static readonly List<Static> ComponentBuffer = new List<Static>();
      private static List<RemovedComponent> _overrides;
   
      private static Collider[] _colliders;
      private static int _prevCount;
 
      private static float _timer;
 
      #endregion
 
      [InitializeOnLoadMethod]
      private static void Initialize() {
         EditorApplication.update -= OnUpdate;
         EditorApplication.update += OnUpdate;
 
         AssemblyReloadEvents.afterAssemblyReload -= DoRefreshLogic;
         AssemblyReloadEvents.afterAssemblyReload += DoRefreshLogic;
      }
   
      private static void OnUpdate() {
         if (Application.isPlaying) return;
     
         if (_timer <= 0) {
            DoRefreshLogic();
            return;
         }
 
         _timer -= Time.deltaTime;
      }
 
      private static void DoRefreshLogic() {
         _colliders = Resources.FindObjectsOfTypeAll<Collider>();
         int colliderLength = _colliders.Length;
 
         if (colliderLength != _prevCount) {
            RefreshComponents();
            _prevCount = colliderLength;
         }
 
         _timer = CheckIntervals;
      }
 
      private static void RefreshComponents() {
         foreach (Collider collider in _colliders) {
            GameObject go = collider.gameObject;
            bool isStatic = go.isStatic;
 
            if (go.hideFlags.HasFlag(HideFlags.NotEditable)) continue;
 
            Static staticComponent = go.GetComponent<Static>();
 
            if (!isStatic) {
               // Make sure to remove static components from gameobject that are no longer static
               if (staticComponent != null) {
                  Object.DestroyImmediate(staticComponent);
                  MarkDirtyIfPrefab(go);
               }
 
               continue;
            }
 
            if (staticComponent == null) {
               if (PrefabUtility.IsPartOfPrefabInstance(go)) {
                  _overrides = PrefabUtility.GetRemovedComponents(go);
               } else {
                  _overrides?.Clear();
               }
 
               bool reverted = false;
 
               if (_overrides != null) {
                  foreach (RemovedComponent component in _overrides) {
                     if (component.GetAssetObject() is Static) {
                        component.Revert();
                        reverted = true;
                     }
                  }
               }
 
               if (!reverted) go.AddComponent<Static>();
               MarkDirtyIfPrefab(go);
            }
         }
 
         // Remove duplicates (preferring prefab components over instances)
         foreach (Collider collider in _colliders) {
            collider.GetComponents(ComponentBuffer);
            if (ComponentBuffer.Count <= 1) continue;
         
            for (int i = 0; i < ComponentBuffer.Count; i++) {
               Static comp = ComponentBuffer[i];
 
               PrefabAssetType type = PrefabUtility.GetPrefabAssetType(comp);
               if (type != PrefabAssetType.NotAPrefab) continue;
           
               Object.DestroyImmediate(comp);
               ComponentBuffer.RemoveAt(i);
 
               if (ComponentBuffer.Count <= 1) {
                  break;
               }
            }
         }
      }
 
      private static void MarkDirtyIfPrefab(GameObject go) {
         PrefabAssetType type = PrefabUtility.GetPrefabAssetType(go);
         if (type == PrefabAssetType.NotAPrefab) return;
     
         EditorUtility.SetDirty(go);
      }
   }
}