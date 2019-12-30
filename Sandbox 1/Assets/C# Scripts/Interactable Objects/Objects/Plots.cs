using UnityEngine;

public class Plots : MonoBehaviour, InteractableInterface
{
    //public GameObject seedsObj;
    public Crops CropPrefab;
    public Material tilledMaterial;
    public Material wetTilledMaterial;
    public bool isTilled = false;
    public bool isSeeded = false;
    public bool isWatered = false;
    Renderer MR;
    Crops myCrop;

    void Awake()
    {
        MR = GetComponent<Renderer>();
    }

	// Update is called once per frame
	void Update () {

    }

    public void interact()
    {
        if (myCrop)
        {
            if (myCrop.isHarvestable)
            {
                HarvestPlot();
            }
            else
            {
                ItemInteractions();
            }
        }
        else
        {
            ItemInteractions();
        }
        
    }

    public void ItemInteractions()
    {
        switch (PlayerManager.Instance.getPlayerHeldItem())
        {
            case "Shovel":
                Till();
                break;
            case "Seeds":
                if (isTilled == true && isSeeded == false)
                {
                    SeedPlot();
                }
                break;
            case "Watering Pail":
                if (isSeeded && isTilled)
                {
                    WaterPlot();
                }
                break;
            default:
                break;
        }
    }

    public void SeedPlot()
    {
        myCrop = Instantiate(CropPrefab, this.transform.position, Quaternion.identity) as Crops;
        myCrop.setPlot(this);
        Debug.Log("Seed plot");
        isSeeded = true;
    }

    //"Tills" the plot, changing its texture
    public void Till()
    {
        if (!isTilled)
        {
            MR.material = tilledMaterial;
            isTilled = true;
        }

    }

    public void WaterPlot()
    {
        isWatered = true;
        MR.material = wetTilledMaterial;

        if (myCrop)
        {
            myCrop.water();
        }
    }

    void HarvestPlot()
    {
        myCrop.harvest();
        Destroy(myCrop);
        isSeeded = false;
    }
}
