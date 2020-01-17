using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A crop exists until the grow time reaches 0 and is harvested
public class Crop : MonoBehaviour {

    Plots myplot;
    GameObject cropAppearence;
    public bool growing = false;
    public bool isHarvestable = false;
    public bool isWatered = false;
    //Varaibles to be overrided when created with cropFactory
    public GameObject seedPrefab;
    public GameObject fullyGrownPrefab;
    private float critSizeScale = 50f;
    private float normalSizeScale = 25f;
    private float growTime = 10.0f;
    float critThreshold = 80;

    public void setPlot(Plots plot)
    {
        myplot = plot;
    }

    // Use this for initialization
    void Start () {
        //When a crop is initially instatiiated, use the seed prefab 
        cropAppearence = Instantiate(seedPrefab, this.transform.position, Quaternion.identity) as GameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (growing)
        {
            growTime -= Time.deltaTime;

            if (growTime <= 0.0f)
            {
                growTime = 0.0f;
                grow();
            }
        }
        else if(myplot.isWatered && !isHarvestable)
        {
            growing = true;
        }
    }

    void grow()
    {
        isHarvestable = true; 

        if (cropAppearence)
        {
            Destroy(cropAppearence);
        }

        Vector3 cropPosition = this.transform.position;
        cropPosition.y = cropPosition.y + 1;
        cropAppearence = Instantiate(fullyGrownPrefab, cropPosition, Quaternion.identity) as GameObject;

        if (rollForCritical())
            cropAppearence.transform.localScale += new Vector3(critSizeScale, critSizeScale, critSizeScale);
        else
            cropAppearence.transform.localScale += new Vector3(normalSizeScale, normalSizeScale, normalSizeScale);

        growing = false;
        Debug.Log("The crop has grown");
    }

    public void harvest(){

        isHarvestable = false;

        if (cropAppearence)
        {
            Destroy(cropAppearence);
        }
    }

    public void water()
    {
        isWatered = true;
    }

    bool rollForCritical()
    {
        float randnum = Random.Range(0f, 100f);

        if(randnum > critThreshold)
        {
            return true;
        }

        return false;
    }
}
