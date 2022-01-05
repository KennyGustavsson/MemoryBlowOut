using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using Freya;

class ModelPostProcessor : AssetPostprocessor
{

    void OnPostprocessModel(GameObject g)
    {
        Debug.Log("Processed " + g.name);

        var importer = (assetImporter as ModelImporter);

        
        
        if (Path.GetExtension(importer.assetPath) == ".fbx")
        {
            //      RotateObject(object);
            MeshFilter root = g.GetComponent<MeshFilter>();
            if (root != null)
            {
                Convert(root.sharedMesh); 
            }
            MeshFilter[] childMeshes = g.GetComponentsInChildren<MeshFilter>();
            foreach (var filter in childMeshes)
            {
                Convert(filter.sharedMesh); 
            }

        }

    }

    void Convert(Mesh mesh)
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
    }



}

