using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CreateMaterialsForPickupTextures : UnityEditor.Editor
{
    private static readonly int Normalmap = Shader.PropertyToID("Normals");
    private static readonly int AlphaMap = Shader.PropertyToID("Alpha");
    private static readonly int Albedo = Shader.PropertyToID("Textures");

    private static List<string> NormalSuffixes = new List<string>
    {
        "_Normal_0000",
    };

    private static List<string> ColorSuffixes = new List<string>
    {
        "_FrameBuffer_0000"
    };

    private static List<string> AlphaSuffixes = new List<string>
    {
        "_Alpha_0000"
    };
    [MenuItem("Tools/CreateMaterialsForPickups")]
    static void CreateMaterials()
    {
        try
        {
            AssetDatabase.StartAssetEditing();

            // var textures = Selection.GetFiltered(typeof(Texture), SelectionMode.Deep).Cast<Texture>();
            var textures = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets).Cast<Texture>();

            IEnumerable<Texture> enumerable = textures.ToList();
            Debug.Log($"Processing {enumerable.Count()} textures total.");
            foreach (var tex in enumerable)
            {
                string path = AssetDatabase.GetAssetPath(tex);
                string extension = path.Substring(path.LastIndexOf("."));
                string baseFilename = path.Substring(0, path.LastIndexOf("."));

                string colorSuffix = null;
                foreach (var suffix in ColorSuffixes)
                {
                    if (baseFilename.Contains(suffix))
                    {
                        colorSuffix = suffix;
                    }
                }

                if (colorSuffix == null)
                {
                    continue; //this isn't a color texture
                }

                var mat = new Material(Shader.Find("Shader Graphs/3dSpriteUnlit"));
                mat.SetTexture(Albedo, tex);
                string baseFilenameWithoutColorSuffix = baseFilename.Substring(0,
                    baseFilename.LastIndexOf(colorSuffix, StringComparison.Ordinal));
                path = baseFilenameWithoutColorSuffix + ".mat";
                if (AssetDatabase.LoadAssetAtPath(path, typeof(Material)) != null)
                {
                    Debug.LogWarning("Can't create material, it already exists: " + path);
                    continue;
                }

                Texture2D normalTex = null;
                foreach (var suffix in NormalSuffixes)
                {
                    string normalMapName = baseFilenameWithoutColorSuffix + suffix + extension;

                    if (File.Exists(normalMapName))
                    {
                        normalTex = AssetDatabase.LoadAssetAtPath<Texture2D>(normalMapName);
                        if (normalTex != null)
                        {
                            break;
                        }
                    }
                }
                
                if (normalTex == null)
                {
                    Debug.Log($"Unable to find normal texture for {baseFilename}");
                }
                else
                {
                    mat.SetTexture(Normalmap, normalTex);
                }
                
                Texture2D alphaTex = null;

                foreach (var suffix in AlphaSuffixes)
                {
                    string alphaMapName = baseFilenameWithoutColorSuffix + suffix + extension;

                    if (File.Exists(alphaMapName))
                    {
                        alphaTex = AssetDatabase.LoadAssetAtPath<Texture2D>(alphaMapName);
                        if (alphaTex != null)
                        {
                            break;
                        }
                    }
                }
                if (alphaTex == null)
                {
                    Debug.Log($"Unable to find alpha texture for {baseFilename}");
                }
                else
                {
                    mat.SetTexture(AlphaMap, alphaTex);
                }
                AssetDatabase.CreateAsset(mat, path);

            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();
        }
    }
}