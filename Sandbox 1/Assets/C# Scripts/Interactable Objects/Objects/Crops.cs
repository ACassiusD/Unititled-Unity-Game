using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crops : MonoBehaviour {

    public GameObject seedsObj;
    public GameObject grownObj;
    GameObject cropAppearence;
    GameObject myplot;
    public float growTime = 120.0f;
    public bool growing = false;
    public bool isHarvestable = false;

    void Crop(GameObject plot)
    {
        myplot = plot;
    }

    // Use this for initialization
    void Start () {
        //When a crop is initially instatiiated, use the seed prefab 
        cropAppearence = Instantiate(seedsObj, this.transform.position, Quaternion.identity) as GameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (growing)
        {
            growTime -= Time.deltaTime;

            if (growTime <= 0.0f)
            {
                grow();
            }
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
        cropAppearence = Instantiate(grownObj, cropPosition, Quaternion.identity) as GameObject;
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
}
