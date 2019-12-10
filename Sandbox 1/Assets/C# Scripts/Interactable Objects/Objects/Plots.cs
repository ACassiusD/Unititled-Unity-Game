using UnityEngine;

public class Plots : MonoBehaviour, InteractableInterface
{
    public GameObject seedsObj;
    public Material tilledMaterial;
    Renderer MR;
    public bool isTilled = false;
    public bool isSeeded = false;
    GameObject mySeeds;

    void Awake()
    {
        MR = GetComponent<Renderer>();
    }

	// Update is called once per frame
	void Update () {

    }

    public void interact()
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
            default:
                break;
        }
    }

    public void SeedPlot()
    {
        mySeeds = Instantiate(seedsObj, this.transform.position, Quaternion.identity) as GameObject;
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
}
