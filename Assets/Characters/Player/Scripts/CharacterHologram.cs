using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterHologram : MonoBehaviour
{
    IAA_Player inputActions;
    GameObject[] EnemyObjects; // Array to store all enemies
    EnemyAI_And_StateManager[] enemyStateManagers; // Array to store all stateManagers
    bool changedToTransparent = true; // Tracks if player is currently transparent
    bool materialChangeActive = false; // Tracks if material change is active
    [SerializeField] float materialChangeDuration = 15f; // Time in seconds for material to remain changed
    float materialChangeTimer = 0f; // Timer for the material change
    [SerializeField] Material[] hologramMaterial;

    float TimeCount;

    Scrollbar TransprentRemainingTime;
    // Start is called before the first frame update
    private void Awake()
    {

        TransprentRemainingTime = transform.Find("UI/Canvas/Transparent_Timer").GetComponentInChildren<Scrollbar>();
        inputActions = new IAA_Player();
    }

    void Start()
    {
       
        // Try to find the "Transparent_Timer" GameObject and get its TextMeshPro component


        if (TransprentRemainingTime == null)
        {
            Debug.LogError("TextMeshPro component not found on 'Transparent_Timer' or object not found.");
        }

        // Find all GameObjects with the "Enemy" tag
        EnemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        // Initialize the enemyStateManagers array to have the same size as EnemyObjects
        enemyStateManagers = new EnemyAI_And_StateManager[EnemyObjects.Length];

        // Iterate through all enemies and assign their StateManager component
        for (int i = 0; i < EnemyObjects.Length; i++)
        {
            // Get the EnemyAI_And_StateManager component on each enemy
            enemyStateManagers[i] = EnemyObjects[i].GetComponent<EnemyAI_And_StateManager>();
            if (enemyStateManagers[i] == null)
            {
                Debug.LogError("EnemyAI_And_StateManager component not found on enemy: " + EnemyObjects[i].name);
            }
        }

        if (hologramMaterial.Length < 2)
        {
            Debug.LogError("Not enough materials in hologramMaterial array.");
            return;
        }

        // Listen for the input action to change the player's material
        inputActions.Player.ChangeMaterial.performed += ChangePlayerMaterial;
    }


    private void ChangePlayerMaterial(InputAction.CallbackContext context)
    {
        // If material change is already active, do nothing
        if (materialChangeActive)
        {
            Debug.Log("Cannot change material while transparent.");
            return;
        }

        // Check if hologram materials are assigned correctly
        if (hologramMaterial.Length < 2)
        {
            Debug.LogError("Hologram materials not properly assigned.");
            return;
        }

        // Toggle the material change state and start the timer
        materialChangeActive = true;
        changedToTransparent = true;
        materialChangeTimer = materialChangeDuration; // Reset the timer

        // Change the material for each enemy based on their current state
        foreach (var stateManager in enemyStateManagers)
        {
            if (stateManager != null)
            {
                foreach (Renderer objects in GetComponentsInChildren<Renderer>())
                {
                    // Change the material based on transparency state
                    if (changedToTransparent && stateManager.CurrentState != StateManager.States.Attack_State)
                    {
                        // If already transparent, change to non-transparent (hologramMaterial[1])
                        stateManager.chaseStateDistance = 5f;
                        objects.material = hologramMaterial[1];
                    }
                    else if (!changedToTransparent)
                    {
                        // If not transparent, change to transparent (hologramMaterial[0])
                        stateManager.chaseStateDistance = 15f;
                        objects.material = hologramMaterial[0];
                    }
                }
            }
        }

    }

    private void Update()
    {
        // Decrease the materialChangeTimer based on time passing
        materialChangeTimer -= Time.deltaTime;

        // If material change is active, update the scrollbar
        if (materialChangeActive)
        {
            // Normalize the timer value between 0 and 1
            float normalizedTime = Mathf.Clamp01(materialChangeTimer / materialChangeDuration);

            // Update the scrollbar value based on the normalized time
            TransprentRemainingTime.value = normalizedTime;
        }

        // If the timer reaches 0, revert the material change
        if (materialChangeTimer <= 0f && materialChangeTimer > -1f)
        {
            RevertMaterialChange();
        }

        // If the material change duration has passed and is about to restart, reset the timer
        if (materialChangeTimer <= -10f && materialChangeTimer > -11f)
        {
            WaitFortransparentAgain();
            // Ensure the scrollbar is set to 1 when waiting for transparency again
            TransprentRemainingTime.value = 1;
        }
    }



    private void RevertMaterialChange()
    {
        // Revert material for each enemy and reset chaseStateDistance
        foreach (var stateManager in enemyStateManagers)
        {
            if (stateManager != null)
            {
                foreach (Renderer objects in GetComponentsInChildren<Renderer>())
                {
                    // Revert to original material and reset chaseStateDistance
                    objects.material = hologramMaterial[0]; // Assuming original state is hologramMaterial[1]
                    stateManager.chaseStateDistance = 15f;
                }
            }
        }

        // Reset the timer and transparency UI
       
        
    }

    private void WaitFortransparentAgain()
    {
        // Wait for a period before allowing material change again
       
     
        // Reset the change material state
        materialChangeActive = false; // Deactivate material change state
        changedToTransparent = false; // Reset the transparency state
    }



    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
