//Extendable class that contains core logic for playable characters such as collisions and movement.
using UnityEngine;

//Playable character may not be the best description for this class
//This class defines any "living" creature and the most basic of functions like movement, health system etc.
public class Entity : MonoBehaviour, InteractableInterface
{
    public CharacterController controller = new CharacterController();   
    protected static GameObject playerReference;
    private static GameObject canvas;
    static BetaCharacter playerScript;
    public HealthBar healthBarScript;
    private RectTransform rectTransform;
    public float aiRotationSpeed = 1.5f;
    public int aiMoveSpeed = 30;
    public bool canAttack = true;
    int currentHealth = 50;
    //public int currentHealth = 50;
    //public int maxHealth = 100;
    //public bool inHitStun = false;


    protected virtual void Start () {
        if (!playerReference)
        {
            playerReference = GameObject.FindGameObjectWithTag("Player");
        }
        gameObject.layer = 8; //Set to interactive layer automatically
        playerScript = PlayerManager.Instance.getPlayerScript();
        //healthBarScript = this.GetComponentInChildren<HealthBar>();
        //var test = this.gameObject.name;
        //onCreate();
        //controller = GetComponent<CharacterController>();
        //rectTransform = GetComponent<RectTransform>();
        //canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        //updateHealthBar();
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

    public void interact()
    {
        throw new System.NotImplementedException();
    }



}
