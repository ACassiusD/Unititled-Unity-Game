using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public Plots plot;
    public int landSize = 300;
    [Range(1, 50)]
    public float size = 25f;
    public float startX = 700;
    public float startZ = 500;
    public float plotBezel = 0.2f;
    Vector3 plotSize;
    private Dictionary<string, Plots> plotDictionary;
    public bool DrawGizmos = false;
    public bool plotDebugger = false;
    float yposition = -0.3f;
    Text[] sdfds; 



    public void Awake()
    {
        DrawPlots();
       // dictonaryDebugger();
    }

    private void Update()
    {
        if (plotDebugger)
        {
            if (Input.GetKeyDown("3"))
            {
                destroyPlots();
                DrawPlots();
            }
        }
    }

    //Draw plots in the grid
    private void DrawPlots()
    {
        var point = new Vector3();
        int plotNameColumn = 0;
        int plotNameRow = 0;
        string plotEntry;
        plotDictionary = new Dictionary<string, Plots>();
        plotSize = new Vector3(size - plotBezel, 0.7f, size - plotBezel);


        for (float x = startX; x <= startX + landSize; x += size)
        {
            plotNameColumn += 1;
            for (float z = startZ; z <= startZ + landSize; z += size)
            {
                plotNameRow += 1;
                point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
            
                //Instantiate a new plot
                Plots newPlot = Instantiate(plot, point, Quaternion.identity) as Plots;

                newPlot.transform.SetParent(this.transform);

                //Apply the size
                newPlot.transform.localScale = (plotSize);

                //Apply Y position
                newPlot.transform.position += new Vector3(0, yposition, 0);

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
        foreach (KeyValuePair<string, Plots> plots in plotDictionary)
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

        foreach (KeyValuePair<string, Plots> entry in plotDictionary)
        {
            Plots plotToDestory = entry.Value;
            Destroy(plotToDestory.getCrop());
            Destroy(plotToDestory.gameObject);
        }

    }
}