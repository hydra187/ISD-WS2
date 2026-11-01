using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class CoinSnatcherSetup
{
    [MenuItem("Coin Snatcher/Create Red Yellow Game")]
    public static void Create()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        cameraObject.tag = "MainCamera";
        cameraObject.transform.position = new Vector3(0, 0, -10);
        var camera = cameraObject.GetComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 5;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(.10f, .015f, .02f);

        System.IO.Directory.CreateDirectory("Assets/CoinSnatcher/Art");
        var texture = new Texture2D(128, 128, TextureFormat.RGBA32, false);
        for (int y = 0; y < 128; y++)
            for (int x = 0; x < 128; x++)
            {
                float distance = Vector2.Distance(new Vector2(x + .5f, y + .5f), new Vector2(64, 64));
                texture.SetPixel(x, y, new Color(1, 1, 1, Mathf.Clamp01(63 - distance)));
            }
        texture.Apply();
        System.IO.File.WriteAllBytes("Assets/CoinSnatcher/Art/Disc.png", texture.EncodeToPNG());
        Object.DestroyImmediate(texture);
        AssetDatabase.ImportAsset("Assets/CoinSnatcher/Art/Disc.png");
        var importer = (TextureImporter)AssetImporter.GetAtPath("Assets/CoinSnatcher/Art/Disc.png");
        importer.textureType = TextureImporterType.Sprite;
        importer.alphaIsTransparency = true;
        importer.mipmapEnabled = false;
        importer.SaveAndReimport();

        var disc = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/CoinSnatcher/Art/Disc.png");
        var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/CoinSnatcher/Fonts/Roboto Black SDF.asset");
        if (font == null) throw new System.Exception("Roboto Black font asset is missing.");

        var root = new GameObject("Coin Snatcher");
        var game = root.AddComponent<CoinSnatcher>();
        game.BuildLayout(disc, font);
        EditorUtility.SetDirty(game);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/CoinSnatcher.unity");
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene("Assets/Scenes/CoinSnatcher.unity", true) };
        Selection.activeGameObject = root;
        AssetDatabase.SaveAssets();
        Debug.Log("Red and yellow Coin Snatcher is ready.");
    }
}
