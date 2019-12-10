﻿using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject plot;
    public int landSize = 200;
    [Range(1, 50)]
    public float size = 15f;
    public float startX = -270;
    public float startZ = -350;
    public float plotBezel = 0.2f;
    Vector3 plotSize;
    private Dictionary<string, GameObject> plotDictionary;
    public bool DrawGizmos = false;



    public void Awake()
    {
        DrawPlots();
        //dictonaryDebugger();
    }

    private void Update()
    {
        if (Input.GetKeyDown("3"))
        {
            destroyPlots();
            DrawPlots();
        }
 
    }

    //Draw plots in the grid
    private void DrawPlots()
    {
        var point = new Vector3();
        int plotNameColumn = 0;
        int plotNameRow = 0;
        string plotEntry;
        plotDictionary = new Dictionary<string, GameObject>();
        plotSize = new Vector3(size - plotBezel, 0.7f, size - plotBezel);


        for (float x = startX; x <= startX + landSize; x += size)
        {
            plotNameColumn += 1;
            for (float z = startZ; z <= startZ + landSize; z += size)
            {
                plotNameRow += 1;
                point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
            
                //Instantiate a new plot
                GameObject newPlot = Instantiate(plot, point, Quaternion.identity) as GameObject;

                //Apply the size
                newPlot.transform.localScale = (plotSize);

                //Name plot in the Hierarchy
                newPlot.name = "Plot_" + plotNameColumn + "-" + plotNameRow;

                //Add plot into a dictonary 
                plotEntry = newPlot.transform.localPosition.x + "," + newPlot.transform.localPosition.z;
                plotDictionary.Add(plotEntry, newPlot);

            }
            plotNameRow = 0;
        }

    }

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3(
            (float)xCount * size,
            (float)yCount * size,
            (float)zCount * size);

        result += transform.position;

        return result;
    }

    private void drawPlotBorder()
    {
        Gizmos.color = Color.white;
        float sphereHeight = 6f;
        //Green Origin
        Gizmos.color = Color.green;
        var point = (new Vector3(startX, sphereHeight, startZ));
        Gizmos.DrawSphere(point, 1f);

        ////Red
        Gizmos.color = Color.red;
        point = (new Vector3(startX + landSize, sphereHeight, startZ + landSize));
        Gizmos.DrawSphere(point, 1f);

        Gizmos.color = Color.blue;
        point = (new Vector3(startX, sphereHeight, startZ + landSize));
        Gizmos.DrawSphere(point, 1f);

        Gizmos.color = Color.yellow;
        point = new Vector3(startX + landSize, sphereHeight, startZ);
        Gizmos.DrawSphere(point, 1f);



        Gizmos.color = Color.white;
        point = new Vector3(startX + (landSize/2), sphereHeight, startZ + (landSize / 2));
        Gizmos.DrawSphere(point, 1f);

    }

    private void dictonaryDebugger()
    {
        foreach (KeyValuePair<string, GameObject> plots in plotDictionary)
        {
            if (plots.Key == "Plot_4_10")
            {
                Debug.Log("HIT***");
            }
            Debug.Log(plots.Key + " = " + plots.Value.ToString());
        }
    }

    //Draw points in grid
    public void OnDrawGizmos()
    {
        if (DrawGizmos)
        {
            Gizmos.color = Color.white;
            var point = new Vector3();

            for (float x = startX; x <= startX + landSize; x += size)
            {
                for (float z = startZ; z <= startZ + landSize; z += size)
                {
                    point = GetNearestPointOnGrid(new Vector3(x, 8f, z));
                    Gizmos.DrawSphere(point, 0.3f);
                }
            }
            drawPlotBorder();
        }
    }

    private void destroyPlots()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag("farm_land");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }
}