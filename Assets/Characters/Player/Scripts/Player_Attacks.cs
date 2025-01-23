using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Attacks : MonoBehaviour
{
    public bool isNotEquipped;
    private IAA_Player inputActions;
    private Player_Locomotion locomotionScript;
    private Animator animator;

    private int attackPhaseId;
    private int equipWeaponId;
    private int unequipWeaponId;

    [Range(0, 1)][SerializeField] private float attackWindowOpen;
    [Range(0, 1)][SerializeField] private float attackWindowClose;

    private bool canAttackNext;
    private bool canEquipUnequip;

    private int attackIncrement = 0;
    private bool animationState01;
    private bool animationState02;
    private bool animationState03;
    private bool animationState04;
    private float animationsNormalizedTime;
    public bool CanAttack;
    public bool CanPlayerGetHit;
    
    private void Awake()
    {
        CanPlayerGetHit = true;
        inputActions = new IAA_Player();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        CanAttack = true;
        isNotEquipped = true;
        canAttackNext = true;
        animator = GetComponent<Animator>();
        attackPhaseId = Animator.StringToHash("AttackPhases");
        equipWeaponId = Animator.StringToHash("EquipWeapon");
        unequipWeaponId = Animator.StringToHash("UnEquipWeapon");
        locomotionScript = GetComponent<Player_Locomotion>();

        inputActions.Player.Attack.performed += AttackAction;
        inputActions.Player.EquipWeapon.performed += EquipWeapon;
    }

    private void Update()
    {
        UpdateAnimationStates();
        EquipUnequipManage();
        AttackPhaseCheck();
    }

    private void UpdateAnimationStates()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animationState01 = stateInfo.IsName("Attack01_1");
        animationState02 = stateInfo.IsName("Attack01_2");
        animationState03 = stateInfo.IsName("Attack01_3");
        animationState04 = stateInfo.IsName("Attack01_4");
        animationsNormalizedTime = stateInfo.normalizedTime;
    }

    private void EquipUnequipManage()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Equip") || animator.GetCurrentAnimatorStateInfo(0).IsName("UnEquip") || animator.GetCurrentAnimatorStateInfo(0).IsName("Teleport_Attack") || animator.GetCurrentAnimatorStateInfo(0).IsName("Avoid"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                canEquipUnequip = false;
                locomotionScript.canMove = false;
                return;
            }
        }

        canEquipUnequip = true;
        locomotionScript.canMove = true;
    }

    private void AttackPhaseCheck()
    {

        if (animationState04 && animationsNormalizedTime <= .95f)
        {
            locomotionScript.canMove = false;
        }
        if (animationState04 && animationsNormalizedTime > .95f)
        {
            AttackReset();
            locomotionScript.canMove = true;
        }
        if ((animationState01 || animationState02 || animationState03))
        {

            if (animationsNormalizedTime >= 0f && animationsNormalizedTime <= attackWindowOpen)
            {
                canAttackNext = false;
            }
            else if (animationsNormalizedTime > attackWindowOpen && animationsNormalizedTime <= attackWindowClose)
            {
                canAttackNext = true;
            }
            else if (animationsNormalizedTime > attackWindowClose && animationsNormalizedTime < 1f)
            {
                canAttackNext = false;
            }
            else if (animationsNormalizedTime >= 1f)
            {
                AttackReset();
                canAttackNext = true;
                locomotionScript.canMove = true;
            }
            locomotionScript.canMove = false;
        }
    }

    private void AttackAction(InputAction.CallbackContext context)
    {
        if ((CanAttack && !isNotEquipped) && canAttackNext)
        {
            ++attackIncrement;

            if (attackIncrement > 4) { attackIncrement = 0; }
            animator.SetInteger(attackPhaseId, attackIncrement);
        }
    }

    private void EquipWeapon(InputAction.CallbackContext context)
    {
        if (canEquipUnequip)
        {
            isNotEquipped = !isNotEquipped; // Toggle equipped state
            animator.SetBool(equipWeaponId, !isNotEquipped);
            animator.SetBool(unequipWeaponId, isNotEquipped);
            StartCoroutine(ResetEquipUnequipFlags());
        }
    }

    private System.Collections.IEnumerator ResetEquipUnequipFlags()
    {
        yield return new WaitForSeconds(0.2f); // Delay for animation to play
        animator.SetBool(equipWeaponId, false);
        animator.SetBool(unequipWeaponId, false);
    }

    private void AttackReset()
    {
        attackIncrement = 0;
        animator.SetInteger(attackPhaseId, 0);
    }
}
