using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
using ICSharpCode.NRefactory.Ast;

public class CreateMaterialsForTextures : UnityEditor.Editor
{
    private static readonly int Normalmap = Shader.PropertyToID("Normal");
    private static readonly int Albedo = Shader.PropertyToID("Albedo");

    private static List<string> NormalSuffixes = new List<string>
    {
        "_NORM",
        "_DISP_NORM",
        "_NORM_A",
        "_NORM_B",
        "_normal",
        "_Normal_0000",
        "_NRM",
    };

    private static List<string> ColorSuffixes = new List<string>
    {
        "_COLOR",
        "_COLOR_A",
        "_COLOR_B",
        "_COLOR_C",
        "_basecolor",
        "_FRAMEBUFFER",
        "_FrameBuffer_0000"
    };

    [MenuItem("Tools/CreateMaterialsForTextures")]
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

                var mat = new Material(Shader.Find("Shader Graphs/PixelShader"));
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
                    mat.SetVector("Tiling", new Vector3(.1f, .1f, 0));
                    mat.SetTexture(Normalmap, normalTex);
                    AssetDatabase.CreateAsset(mat, path);
                }
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();
        }
    }
}