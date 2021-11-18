using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
/*
[CustomEditor(typeof(Classes.World.World))]
public class RenderButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var myScript = (Classes.World.World)target;
        if (GUILayout.Button("Render Noise"))
        {
            var tuple = myScript.DrawNoise();
            var mapTexture = tuple.Item1;
            var decoTexture = tuple.Item2;
            var path = $"{Application.dataPath}/SaveImages";
            
            SaveTextureAsPNG(mapTexture, path, "map");
            SaveTextureAsPNG(decoTexture, path, "deco");
        }
    }
    
    public static void SaveTextureAsPNG(Texture2D texture, string fullPath, string name)
    {
        var bytes = texture.EncodeToPNG();
        File.WriteAllBytes($"{fullPath}/{name}.png", bytes);
        Debug.Log(bytes.Length/1024  + "Kb was saved as: " + fullPath);
    }
}
*/