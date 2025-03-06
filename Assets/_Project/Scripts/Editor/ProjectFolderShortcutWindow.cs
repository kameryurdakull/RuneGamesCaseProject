/**
 * ProjectFolderShortcutWindow.cs
 * Created by: Kamer
 * Created on: 2024-06-04
 */

namespace Game.Editor
{
    using UnityEngine;
    using UnityEditor;

    public class ProjectFolderShortcutWindow : EditorWindow
    {
        public string[] folderPaths = new string[0];

        Vector2 scrollPosition;

        [MenuItem("Tools/Project Folder Shortcuts")]
        public static void ShowWindow()
        {
            GetWindow<ProjectFolderShortcutWindow>("Project Folder Shortcuts");
        }

        void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            foreach (string folderPath in folderPaths)
            {
                string folderName = System.IO.Path.GetFileName(folderPath);
                if (!string.IsNullOrEmpty(folderPath) && GUILayout.Button(new GUIContent(folderName, folderPath)))
                {
                    Object folder = AssetDatabase.LoadAssetAtPath<Object>(folderPath);
                    if (folder != null)
                    {
                        EditorGUIUtility.PingObject(folder);
                    }
                    else
                    {
                        Debug.LogWarning("Klasör bulunamadý: " + folderPath);
                    }
                }
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Klasörleri Temizle"))
            {
                folderPaths = new string[0];
            }

            Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Klasörleri Buraya Sürükleyin");

            Event evt = Event.current;
            if (evt.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                evt.Use();
            }
            else if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();

                foreach (Object draggedObject in DragAndDrop.objectReferences)
                {
                    string path = AssetDatabase.GetAssetPath(draggedObject);
                    if (System.IO.Directory.Exists(path))
                    {
                        System.Array.Resize(ref folderPaths, folderPaths.Length + 1);
                        folderPaths[^1] = path;
                    }
                }
                evt.Use();
            }

            EditorGUILayout.EndScrollView();
        }
    }
}