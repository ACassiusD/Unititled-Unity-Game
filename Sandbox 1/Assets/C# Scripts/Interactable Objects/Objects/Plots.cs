using UnityEngine;

public class Plots : MonoBehaviour, InteractableInterface
{
    public Material tilledMaterial;
    Renderer MR;
    public bool isTilled = false;

    void Awake()
    {
        MR = GetComponent<Renderer>();
    }

	// Update is called once per frame
	void Update () {

    }

    public void interact()
    {
        if(PlayerManager.Instance.getPlayerHeldItem() == "Shovel")
        {
            Till();
        }
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
