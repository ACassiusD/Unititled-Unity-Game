//Extendable class that contains core logic for playable characters such as collisions and movement.
using UnityEngine;
using UnityEngine.Events;

//Playable character may not be the best description for this class
//This class defines any "living" creature and the most basic of functions like movement, health system etc.
public class Entity : MonoBehaviour, IInteractable
{
    public CharacterController controller = new CharacterController();
    protected static GameObject playerReference;
    private static GameObject canvas;
    static PlayerEntity playerScript;
    public HealthBar healthBarScript;
    private RectTransform rectTransform;
    public float aiRotationSpeed = 1.5f;
    public int aiMoveSpeed = 30;
    public bool canAttack = true;
    int currentHealth = 50;

    public UnityAction<IInteractable> OnInteractionComplete { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    protected virtual void Start()
    {
        if (!playerReference)
        {
            playerReference = GameObject.FindGameObjectWithTag("Player");
        }
        gameObject.layer = 8; //Set to interactive layer automatically
        playerScript = PlayerManager.Instance.getPlayerScript();
    }

    //Overloadable function that is called when a mount is initialized 
    public virtual void onCreate()
    {
        Debug.Log("base");
    }

    public virtual void attack()
    {
        Debug.Log(this.name + "Attacks");
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
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
