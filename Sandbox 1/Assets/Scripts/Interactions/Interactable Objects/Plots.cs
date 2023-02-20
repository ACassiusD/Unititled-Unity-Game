using UnityEngine;
using UnityEngine.Events;

public class Plots : MonoBehaviour, IInteractable
{
    public Crop CropPrefab;
    public Material tilledMaterial;
    public Material wetTilledMaterial;
    public bool isTilled = false;
    public bool isSeeded = false;
    public bool isWatered = false;
    public bool renderMesh = true;
    Renderer MR;
    Crop myCrop;

    public UnityAction<IInteractable> OnInteractionComplete { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    void Awake()
    {
        MR = GetComponent<Renderer>();
        if (!renderMesh)
        {
            MR.enabled = false;
        }
    }

	// Update is called once per frame
	void Update () {

    }
    public Crop getCrop()
    {
        return myCrop;
    }

    public void Interact()
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
        myCrop = Instantiate(CropPrefab, this.transform.position, Quaternion.identity) as Crop;
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
            MR.enabled = true;
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

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        throw new System.NotImplementedException();
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }
}
