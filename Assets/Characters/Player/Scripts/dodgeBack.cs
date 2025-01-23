using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DodgeBack : MonoBehaviour
{
    private IAA_Player inputActions;
    private Animator animator;
    private Player_Locomotion playerLocomotion;
    private Player_Attacks playerAttacks;
    public bool isDodging {  get; private set; }    
    private float nextDodgeTime;
    TeleportBehindEnemy TeleportBehindEnemy;
    private void Awake()
    {
        TeleportBehindEnemy = GetComponent<TeleportBehindEnemy>();
        playerLocomotion = GetComponent<Player_Locomotion>();
        playerAttacks = GetComponent<Player_Attacks>();
        animator = GetComponent<Animator>();
        inputActions = new IAA_Player();
    }

    private void Start()
    {
        inputActions.Player.JumpBack.performed += ctx => Dodge();
    }

    private void Dodge()
    {
        if (isDodging || playerAttacks.isNotEquipped ) return;
        if (!isDodging || !playerAttacks.isNotEquipped)
        {
           // animator.SetInteger("AttackPhases", 0);
            TeleportBehindEnemy.StopAllCoroutines();
            isDodging = true;
            playerAttacks.CanAttack = false;
            animator.StopPlayback();
            animator.Play("Avoid");
            Invoke("canattacktrue", 1.1f);
            Invoke("MakedodgeFalse", 1.1f);
        }
    }
    void canattacktrue()
    {
        playerAttacks.CanAttack = true;
    }
    void MakedodgeFalse()
    {
        isDodging = false;
    }

    private void Update()
    {
        
        if (!isDodging) return;

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Avoid"))
        {
            
          //  animator.SetInteger("AttackPhases", 0);
            playerAttacks.CanPlayerGetHit = stateInfo.normalizedTime >= .99f;
            if (stateInfo.normalizedTime < .99f && TeleportBehindEnemy.isTeleporting)
            {
                isDodging = false;
                animator.StopPlayback();
            }
            if (stateInfo.normalizedTime >= .99f)
            {
                isDodging = false;
                
                animator.StopPlayback();
                animator.Play("Idle_Walk_Run");
            }
        }
    }

    private void OnEnable() => inputActions.Enable();

    private void OnDisable() => inputActions.Disable();
}
