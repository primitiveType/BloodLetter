using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

[ScriptedImporter(31, "exr", 3)]
public class LightmapImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        AssetDatabase.StartAssetEditing();
        Debug.Log("importing light map");
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(ctx.assetPath);
        texture.minimumMipmapLevel = 0;
        AssetDatabase.SaveAssets();
        AssetDatabase.StopAssetEditing();
    }
}