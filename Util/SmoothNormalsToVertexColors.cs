using Freya;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothNormalsToVertexColors : MonoBehaviour
{


    public bool AreYouSure = false;

    private void Start()
    {
        if (!AreYouSure)
            return;

        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        
        int count = mesh.vertexCount;
        var verts = mesh.vertices;
        var colors = mesh.colors;
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
