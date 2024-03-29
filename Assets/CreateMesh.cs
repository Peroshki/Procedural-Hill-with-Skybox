﻿// Created by Angus Forbes, Assistant Professor at the University of California, Santa Cruz

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class CreateMesh : MonoBehaviour
{
    int numCellsX = 20;
    int numCellsY = 20;

    int xSize = 5; 
    int ySize = 5;
    Mesh mesh;


   
    private void Start()
    {
        CreateNewMesh();
    }

    // Added support for key presses
    void Update()
    {
        // Press space to generate a new mesh
        if (Input.GetKeyDown("space"))
        {
            CreateNewMesh();
        }

        // Use the scroll wheel to adjust the depth of the hill terrain
        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
        {
            transform.localScale += new Vector3(0, 0, 0.1f);
        }
        else if (d < 0f)
        {
            transform.localScale -= new Vector3(0, 0, 0.1f);
        }
    }

    void CreateNewMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh oMesh = meshFilter.sharedMesh;

        //make sure not to overwrite this mesh by copying, othere
        mesh = new Mesh(); 
        mesh.name = "Procedural Grid X";
       // mesh.vertices = oMesh.vertices;
       // mesh.triangles = oMesh.triangles;
       // mesh.normals = oMesh.normals;
       // mesh.uv = oMesh.uv;
        meshFilter.mesh = mesh; 

        mesh.Clear();

       

        Vector3[] vertices = new Vector3[(numCellsX + 1) * (numCellsY + 1)];
        Vector2[] uv = new Vector2[vertices.Length];

        float startX = -xSize / 2.0f;
        float startY = -ySize / 2.0f;
        float xInc = (float)xSize / (float)numCellsX;
        float yInc = (float)ySize / (float)numCellsY;

        // A "noise" function which randomly generates z-axis values for the texture
        int idx = 0;
        for (int y = 0; y <= numCellsY; y++)
        {
            for (int x = 0; x <= numCellsX; x++)
            {
                float zVal = Random.Range(0.0f, 1.0f);
                vertices[idx] = new Vector3(startX + xInc * x, startY + yInc * y, zVal);
                uv[idx] = new Vector2((float)x / (float)numCellsX, (float)y / (float)numCellsY) ;
                idx++;
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;

        // Creates the mesh triangle-by-triangle according to the vertices created in the noise function
        int[] triangles = new int[numCellsX * numCellsY * 6];
        int t_idx = 0;
        int v_idx = 0;
        for (int y = 0; y < numCellsY; y++)
        {
            for (int x = 0; x < numCellsX; x++)
            {
                triangles[t_idx] = v_idx;
                triangles[t_idx + 3] = triangles[t_idx + 2] = v_idx + 1;
                triangles[t_idx + 4] = triangles[t_idx + 1] = v_idx + numCellsX + 1;
                triangles[t_idx + 5] = v_idx + numCellsX + 2;

                v_idx++;
                t_idx += 6;
            }
            v_idx++;
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals(); //much easier than doing it ourselves!
    }
}
