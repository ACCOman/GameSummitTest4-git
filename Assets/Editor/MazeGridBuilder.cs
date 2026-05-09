// ============================================================
//  MazeGridBuilder.cs  —  Editor utility
//  Şəkildəki kimi sabit 6x6 grid labirenti qurar
//  Yerləşdir: Assets/Editor/MazeGridBuilder.cs
//
//  İstifadə:
//    1. Maze_Straight.glb və Maze_Corner_L.glb-ni Assets-ə import et
//    2. Hər ikisini Prefab-a çevir
//    3. Tools → Build Grid Maze
//    4. Prefab-ları assign et → Generate düyməsi
// ============================================================

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MazeGridBuilder : EditorWindow
{
    [Header("Prefab-lar")]
    public GameObject straightPrefab;   // Maze_Straight.glb
    public GameObject cornerPrefab;     // Maze_Corner_L.glb
    public GameObject elevatorPrefab;   // optional

    [Header("Parametrlər")]
    public float cellSize = 193f;       // Bir grid hücrəsinin ölçüsü (Unity unit)
    public string mazeRootName = "GridMaze";

    private Vector2 scroll;
    private string lastLog = "";

    [MenuItem("Tools/Build Grid Maze")]
    public static void ShowWindow()
    {
        GetWindow<MazeGridBuilder>("Grid Maze Builder");
    }

    void OnGUI()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        GUILayout.Label("Grid Labirent Builder", EditorStyles.boldLabel);
        GUILayout.Space(8);

        EditorGUILayout.HelpBox(
            "6x6 sabit labirent qurar. Şəkildəki layoutu izləyir.",
            MessageType.Info);

        GUILayout.Space(8);
        straightPrefab  = (GameObject)EditorGUILayout.ObjectField("Straight prefab",  straightPrefab,  typeof(GameObject), false);
        cornerPrefab    = (GameObject)EditorGUILayout.ObjectField("Corner L prefab",  cornerPrefab,    typeof(GameObject), false);
        elevatorPrefab  = (GameObject)EditorGUILayout.ObjectField("Elevator (opt)",   elevatorPrefab,  typeof(GameObject), false);

        GUILayout.Space(6);
        cellSize = EditorGUILayout.FloatField("Cell size (unit)", cellSize);

        GUILayout.Space(16);
        GUI.backgroundColor = new Color(0.6f, 0.85f, 0.6f);
        if (GUILayout.Button("▶  Generate Maze", GUILayout.Height(40)))
            BuildMaze();
        GUI.backgroundColor = Color.white;

        if (GUILayout.Button("✕  Clear Maze", GUILayout.Height(28)))
            ClearMaze();

        GUILayout.Space(10);
        if (!string.IsNullOrEmpty(lastLog))
            EditorGUILayout.HelpBox(lastLog, MessageType.None);

        EditorGUILayout.EndScrollView();
    }

    // ── LABYRINTH LAYOUT (şəkildən) ───────────────────────────
    // Hər char bir grid hücrəsi:
    //   . = açıq yol (display olunmur, sadəcə place keeper)
    //   - = horizontal düz koridor
    //   | = vertical düz koridor
    //   ┌ ┐ └ ┘ = künc parçaları
    //   S = start (lift), E = exit
    // Şəkildəki labirent təxminən belə görünür:
    private static readonly string[] LAYOUT = new string[]
    {
        // 0    1    2    3    4    5
        "S    -    ┐    .    ┌    ┐",   // row 0
        ".    .    |    .    |    |",   // row 1
        "┌    -    ┘    ┌    -    ┘",   // row 2
        "|    .    .    |    .    .",   // row 3
        "└    -    ┐    └    -    ┐",   // row 4
        ".    .    └    -    -    E",   // row 5
    };

    void BuildMaze()
    {
        if (straightPrefab == null || cornerPrefab == null)
        {
            lastLog = "XƏTA: Hər iki prefab-ı assign et!";
            return;
        }

        ClearMaze();

        var root = new GameObject(mazeRootName);
        Undo.RegisterCreatedObjectUndo(root, "Build Maze");

        int placed = 0;

        // Layout-u parse et
        for (int row = 0; row < LAYOUT.Length; row++)
        {
            string line = LAYOUT[row];
            // "    " whitespace ilə bölünmüş hücrələri al
            string[] cells = line.Split(new[] { ' ' },
                System.StringSplitOptions.RemoveEmptyEntries);

            for (int col = 0; col < cells.Length; col++)
            {
                string cell = cells[col].Trim();
                if (string.IsNullOrEmpty(cell) || cell == ".") continue;

                Vector3 pos = new Vector3(col * cellSize, 0, -row * cellSize);
                PlaceCell(cell, pos, root.transform);
                placed++;
            }
        }

        // Lift əlavə et (S markerinin yerinə)
        if (elevatorPrefab != null)
        {
            var lift = (GameObject)PrefabUtility.InstantiatePrefab(elevatorPrefab, root.transform);
            lift.transform.position = new Vector3(0, 0, 0);
            lift.name = "Elevator";
        }

        Selection.activeGameObject = root;
        SceneView.lastActiveSceneView?.FrameSelected();

        lastLog = $"✓ {placed} hücrə yerləşdirildi.\nGrid: 6x6, cell size: {cellSize}u.";
    }

    void PlaceCell(string symbol, Vector3 pos, Transform parent)
    {
        GameObject prefab = null;
        float yRotation = 0;

        switch (symbol)
        {
            case "S":  // Start - boş hücrə (lift bura gələcək)
            case "E":  // Exit - düz parça (çıxış marker burada)
            case "-":  // Horizontal
                prefab = straightPrefab;
                yRotation = 0;
                break;

            case "|":  // Vertical
                prefab = straightPrefab;
                yRotation = 90;
                break;

            case "┌":  // North-East corner (sağa və aşağıya açıq)
                prefab = cornerPrefab;
                yRotation = 0;
                break;

            case "┐":  // North-West corner (sola və aşağıya açıq)
                prefab = cornerPrefab;
                yRotation = 90;
                break;

            case "┘":  // South-West corner (sola və yuxarıya açıq)
                prefab = cornerPrefab;
                yRotation = 180;
                break;

            case "└":  // South-East corner (sağa və yuxarıya açıq)
                prefab = cornerPrefab;
                yRotation = 270;
                break;

            default:
                return;
        }

        var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
        go.transform.position = pos;
        go.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        go.name = $"{symbol}_({pos.x:F0},{pos.z:F0})";

        // Mesh Collider əlavə et (oyunçu divara girməsin)
        var renderers = go.GetComponentsInChildren<MeshFilter>();
        foreach (var mf in renderers)
        {
            if (mf.gameObject.GetComponent<MeshCollider>() == null)
                mf.gameObject.AddComponent<MeshCollider>();
        }
    }

    void ClearMaze()
    {
        var existing = GameObject.Find(mazeRootName);
        if (existing != null)
        {
            Undo.DestroyObjectImmediate(existing);
            lastLog = "Köhnə labirent təmizləndi.";
        }
    }
}
