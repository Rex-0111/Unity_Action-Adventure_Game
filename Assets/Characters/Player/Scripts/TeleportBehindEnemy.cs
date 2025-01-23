using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportBehindEnemy : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float teleportDelay = 2f;

    private IAA_Player inputActions;
    private Animator animator;
    private Player_Attacks playerAttacks;
    DodgeBack DodgeBack;
    private KatanaCollisionHandle katanaCollision;
    public bool isTeleporting = false;

    private void Awake() => inputActions = new IAA_Player();

    private void OnEnable() => inputActions.Enable();

    private void Start()
    {   DodgeBack = GetComponent<DodgeBack>();
        katanaCollision = GetComponentInChildren<KatanaCollisionHandle>();
        playerAttacks = GetComponent<Player_Attacks>();
        animator = GetComponent<Animator>();
        inputActions.Player.Teleport.performed += _ => Teleport();
    }

    private void Update()
    {
        if (isTeleporting)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < .99f && animator.GetCurrentAnimatorStateInfo(0).IsName("Teleport_Attack"))
            {
                if (DodgeBack.isDodging) { animator.StopPlayback(); }
                katanaCollision.Buffs = 3;
                playerAttacks.CanPlayerGetHit = false;
                animator.SetInteger("AttackPhases", 0);
            }
            else
            {
                katanaCollision.Buffs = 1;
                playerAttacks.CanPlayerGetHit = true;
                animator.StopPlayback();
                animator.Play("Idle_Walk_Run");
                isTeleporting = false;
            }
        }
    }

    private void Teleport()
    {
        if (isTeleporting || playerAttacks.isNotEquipped) return;
        if (!isTeleporting || !playerAttacks.isNotEquipped)
        {
           // animator.SetInteger("AttackPhases", 0);
            playerAttacks.CanAttack = false;
        GameObject closestEnemy = DetectClosestEnemy();
        if (closestEnemy != null)
            StartCoroutine(TeleportBehind(closestEnemy));
        }
    }

    private IEnumerator TeleportBehind(GameObject enemy)
    {
        isTeleporting = true;
        Transform teleportPosition = enemy.transform.Find("Player_teleport_Position");

        if (teleportPosition != null)
        {
            transform.SetPositionAndRotation(teleportPosition.position, teleportPosition.rotation);
            animator.Play("Teleport_Attack");
            yield return new WaitForSeconds(teleportDelay);
            katanaCollision.Buffs = 1;
            playerAttacks.CanPlayerGetHit = true;
            playerAttacks.CanAttack = true;
            animator.Play("Idle_Walk_Run");
        }
        else
        {
            Debug.LogWarning("Player_teleport_Position not found.");
            isTeleporting = false;
        }
    }


    public GameObject DetectClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = collider.gameObject;
                }
            }
        }

        return closestEnemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void OnDisable() => inputActions.Disable();
}
