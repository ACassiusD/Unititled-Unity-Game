using TMPro;
using UnityEngine;

public class ConsoleLogger : MonoBehaviour
{
    //Singleton
    public static ConsoleLogger Instance { get; private set; }
    private TextMeshProUGUI console;
    private PlayerMovementComponent playerMovementComponent;
    public Enemy enemy;
    string maintext = string.Empty;

    private void Awake()
    {
        console = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        if (Instance == null){
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerMovementComponent = PlayerManager.Instance.getPlayerScript().playerMovementComponent;
    }

    // Update is called once per frame
    void Update()
    {
        maintext =
            "Y Velocity............" + playerMovementComponent.velocity + "\n" +
            "Move Direction..." + playerMovementComponent.movementDirection + "\n" +
            "Move Speed......." + playerMovementComponent.currentSpeed + "\n" +
            "Move State........" + playerMovementComponent.movementStateMachine.CurrentState.ToString() + "\n" +
            "Enenmy Move State........" + enemy.moveComponent.movementStateMachine.CurrentState.ToString() + "\n" +
            "Enenmy Combat State........" + enemy.enemyCombatComponent.combatStateMachine.CurrentState.ToString() + "\n" +
            "Sprint Timer......." + playerMovementComponent.sprintTimer + "\n" +
            "Jumps..............." + playerMovementComponent.jumpCount + "/" + playerMovementComponent.maxJumps;
        console.text = maintext;
    }

}
