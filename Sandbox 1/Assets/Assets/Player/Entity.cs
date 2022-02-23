//Extendable class that contains core logic for playable characters such as collisions and movement.
using UnityEngine;

//Playable character may not be the best description for this class
//This class defines any "living" creature and the most basic of functions like movement, health system etc.
public class Entity : MonoBehaviour, InteractableInterface
{
    public CharacterController controller = new CharacterController();
    private static GameObject canvas;
    BetaCharacter playerScript;
    HealthBar healthBarScript;
    private RectTransform rectTransform;

    public float aiRotationSpeed = 1.5f;
    public int aiMoveSpeed = 30;
    public bool canAttack = true;
    public int currentHealth = 50;
    public int maxHealth = 100;
    public bool inHitStun = false;
    protected static GameObject playerReference;



    protected virtual void Start () {
        //if (!playerReference)
        //{
        //    playerReference = GameObject.FindGameObjectWithTag("Player");
        //}
        //gameObject.layer = 8; //Set to interactive layer automatically
        //playerScript = PlayerManager.Instance.getPlayerScript();
        //healthBarScript = this.GetComponentInChildren<HealthBar>();
        //var test = this.gameObject.name;
        //onCreate();
        //controller = GetComponent<CharacterController>();
        //rectTransform = GetComponent<RectTransform>();
        //canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        //updateHealthBar();
    }

    //Update the float UI healthbar above the player in world space
    protected void updateHealthBar()
    {
       // healthBarScript.setHealth(currentHealth, maxHealth);
    }

    //Overloadable function that is called when a mount is initialized 
    public virtual void onCreate()
    {
        Debug.Log("base");
    }

    //public bool checkIfMoving()
    //{
    //    var input = Input.GetAxis("Vertical");
    //    if ( input > 0 || input < 0)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //         return false;
    //    }
    //}

    public virtual void attack()
    {
        Debug.Log(this.name + "Attacks");
    }

    //public int receiveDamage(int damageAmount) //Move to an iFighter class
    //{
    //    currentHealth -= damageAmount;
    //    updateHealthBar();
    //    if(currentHealth <= 0)
    //    {
    //        feint();
    //        return 0;
    //    }
    //    Knockback(300);
    //    return currentHealth;
    //}

    //public void Knockback(float force) //Move to an iFighter class
    //{
    //    inHitStun = true;

    //    //Add Impact
    //    Vector3 direction = this.transform.forward * -1; //Need to make this direction
    //    Vector3 up = this.transform.up;
    //    up.Normalize();
    //    direction.Normalize();
    //    direction.y = up.y;
    //    var impact = Vector3.zero;
    //    impact += direction.normalized * force;

    //    //Apply vector to object
    //    controller.Move(impact * Time.deltaTime);
    //}

    public void interact()
    {
        throw new System.NotImplementedException();
    }

    //Kill/death command, despawn enemy and drop any loot.
    public void feint()
    {
        Destroy(gameObject);
    }
}
