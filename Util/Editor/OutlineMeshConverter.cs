using Freya;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OutlineMeshConverter : EditorWindow
{
    int numberOfMeshesToConvert = 1;
    Mesh[] mesh = new Mesh[0];

    UnityEngine.Object obj = null;

    [MenuItem("Window/Outline Mesh Converter")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        OutlineMeshConverter window = (OutlineMeshConverter)EditorWindow.GetWindow(typeof(OutlineMeshConverter));
        window.Show();
    }

    void OnGUI()
    {

        obj = EditorGUILayout.ObjectField("Mesh: ", obj, typeof(UnityEngine.Object), false);


        GUIStyle myCustomStyle = new GUIStyle(GUI.skin.GetStyle("label"))
        {
            wordWrap = true
        };

        EditorGUILayout.LabelField("Choose a number of meshes to Convert. " +
            "Select read write enabled meshes and a Convert button will appear. " +
            "The conversion will write smoothed normals to the vertex colors." +
            "i.e The Asset is altered and the mesh can no longer use vertex colors.", myCustomStyle);

        int temp = numberOfMeshesToConvert;
        numberOfMeshesToConvert = EditorGUILayout.IntField(numberOfMeshesToConvert);
        if (numberOfMeshesToConvert < 1 )
        {
            numberOfMeshesToConvert = temp;
        }
        if (numberOfMeshesToConvert != mesh.Length)
        {
            var newList = new Mesh[numberOfMeshesToConvert];
            Array.Copy(mesh, newList, Math.Min(mesh.Length, newList.Length));
            mesh = newList;
        }

        bool allClear = true;
        for (int i = 0; i < numberOfMeshesToConvert; i++)
        {
            mesh[i] = (Mesh)EditorGUILayout.ObjectField("Mesh: ", mesh[i], typeof(Mesh), false);
            if (mesh[i] == null )
            {
                allClear = false;
            }
            else if (!mesh[i].isReadable)
            {
                allClear = false;
                EditorGUILayout.LabelField("^^WARNING^^: The mesh is not read/write enabled");
            }

        }

        if (mesh == null)
            return;

        if (allClear)
        {
            if (GUILayout.Button("Convert"))
            {
                for (int i = 0; i < numberOfMeshesToConvert; i++)
                {
                    Convert(mesh[i]);
                }
            }
        } 
        
    }


    private void Convert(Mesh mesh)
    {
        int count = mesh.vertexCount;
        var verts = mesh.vertices;
        Color[] colors = new Color[count];
        Vector3[] norm = mesh.normals;
        Vector3[] smoothedNorm = new Vector3[norm.Length];

        var closeVertices = new List<Tuple<Vector3, List<int>>>();

        void CheckDistanceAndAdd(Vector3 pos, int vertex)
        {

            List<int> vertices = closeVertices.Find((Tuple<Vector3, List<int>> pair)
                => Vector3.Distance(pair.Item1, pos) < float.Epsilon)?.Item2;

            if (vertices == null)
            {
                //add new grouping
                vertices = new List<int>();
                closeVertices.Add(new Tuple<Vector3, List<int>>(pos, vertices));
            }
            if (!vertices.Contains(vertex))
            {
                vertices.Add(vertex);
            }
        }

        for (int i = 0; i < count; i++)
        {
            CheckDistanceAndAdd(verts[i], i);
        }

        foreach (var vertexGrouping in closeVertices)
        {
            //for each grouping of close vertices, calculate an average normal   
            Vector3 resultNormal = Vector3.zero;
            foreach (int vertex in vertexGrouping.Item2)
                resultNormal += norm[vertex];
            resultNormal.Normalize();
            //write average to all grouped vertices
            foreach (int vertex in vertexGrouping.Item2)
                smoothedNorm[vertex] = resultNormal;
        }

        for (int i = 0; i < count; i++)
        {
            colors[i] = new Color(
                Mathfs.Remap(-1, 1, 0, 1, smoothedNorm[i].x),
                Mathfs.Remap(-1, 1, 0, 1, smoothedNorm[i].y),
                Mathfs.Remap(-1, 1, 0, 1, smoothedNorm[i].z));
        }

        mesh.colors = colors;

        AssetDatabase.SaveAssets();
    }
}
