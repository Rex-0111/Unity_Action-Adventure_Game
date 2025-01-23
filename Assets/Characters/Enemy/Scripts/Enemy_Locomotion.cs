using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Locomotion : MonoBehaviour
{
    private GameObject Player;
    private EnemyAI_And_StateManager stateManager;
    private Animator animator;
    private NavMeshAgent agent;
   

    [SerializeField] private Vector3[] Petrolling_Positions;
    private int Petrolling_Position_Index;

    private bool Equip = true;
    public bool isAttacking = false;

    private int SpeedId;
    private int UnEquipId;
    private int Idle_Walk_Run_ID;
    private int EquipId;
    private int Atk_01;
    private int Atk_02;
    private int findAnim_01;


    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        stateManager = GetComponent<EnemyAI_And_StateManager>();
        InitializeAnimationHashes();
    }

    private void InitializeAnimationHashes()
    {
        SpeedId = Animator.StringToHash("Speed");
        EquipId = Animator.StringToHash("Equip");
        UnEquipId = Animator.StringToHash("UnEquip");
        Idle_Walk_Run_ID = Animator.StringToHash("Idle_Walk_Run");
        Atk_01 = Animator.StringToHash("Enemy_Attack01");
        Atk_02 = Animator.StringToHash("Enemy_Attack02"); 
        findAnim_01 = Animator.StringToHash("Enemy_TurnL90");       
    }

    private void Update()
    {
        switch (stateManager.CurrentState)
        {
            case EnemyAI_And_StateManager.States.Petrolling_State:
                HandlePetrollingState();
                break;
            case EnemyAI_And_StateManager.States.Chasing_State:
                HandleChasingState();
                break;
            case EnemyAI_And_StateManager.States.Attack_State:
                HandleAttackState();
                break;
            case EnemyAI_And_StateManager.States.Find_State:
                HandleFindState();
                break;
        }
    }

    private void HandlePetrollingState()
    {   
        if (!Equip)
        {
            PlayAnimation(UnEquipId);
            Equip = true;
        }

        if (IsAnimationRunning("UnEquip"))
        {
            agent.speed = 0f;
            animator.SetFloat(SpeedId, agent.speed);
        }

        if (IsAnimationComplete("UnEquip"))
        {
            PlayAnimation(Idle_Walk_Run_ID);
        }

        if (PlayingAnimationName("Idle_Walk_Run"))
        {
            agent.speed = 2f;
            animator.SetFloat(SpeedId, agent.speed);
            Patrol();
        }
    }

    private void HandleChasingState()
    {
        this.gameObject.transform.LookAt(Player.transform.position);
        
        if (Equip)
        {
            PlayAnimation(EquipId);
            Equip = false;
        }

        if (IsAnimationRunning("Equip"))
        {
            agent.speed = 0f;
            animator.SetFloat(SpeedId, agent.speed);
        }

        if (IsAnimationComplete("Equip"))
        {
            PlayAnimation(Idle_Walk_Run_ID);
        }

        if (PlayingAnimationName("Idle_Walk_Run"))
        {
            agent.speed = 5f;
            animator.SetFloat(SpeedId, agent.speed);
            agent.SetDestination(Player.transform.position);
        }
    }

    private void HandleAttackState()
    {
        
        Attack();
    }

    private void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;

            // Randomly select Atk_01 or Atk_02
            int randomAttack = UnityEngine.Random.Range(0, 2); // 0 or 1
            int attackToPlay = randomAttack == 0 ? Atk_01 : Atk_02;

            PlayAnimation(attackToPlay); 
        }

        if (IsAnimationRunning("Enemy_Attack01") || IsAnimationRunning("Enemy_Attack02"))
        {
            agent.speed = 0; // Stop moving during the attack animation
            animator.SetFloat(SpeedId, agent.speed);
            
        }

        if (IsAnimationComplete("Enemy_Attack01") || IsAnimationComplete("Enemy_Attack02"))
        {
           
            isAttacking = false;
            PlayAnimation(Idle_Walk_Run_ID);
            // Transition back to chasing
            this.gameObject.transform.LookAt(Player.transform.position);
        }
        else if (stateManager.CurrentState == EnemyAI_And_StateManager.States.Chasing_State)
        {
            animator.Play(Idle_Walk_Run_ID);
            // Continue chasing while in attack state if the state changes
            agent.SetDestination(Player.transform.position);
            agent.speed = 5f; // Reset speed for chasing
            animator.SetFloat(SpeedId, agent.speed);
        }
    }

    private void HandleFindState()
    {
        agent.speed = 0f;
        animator.SetFloat(SpeedId, agent.speed);
        PlayAnimation(findAnim_01);
        
        if(IsAnimationComplete("Enemy_TurnL90"))
        {
            animator.StopPlayback();
            PlayAnimation(Idle_Walk_Run_ID);
        }
       
    }

    private void Patrol()
    {
        if (Petrolling_Positions.Length > 0)
        {
            if (agent.remainingDistance < 0.01f)
            {
                Petrolling_Position_Index = (Petrolling_Position_Index + 1) % Petrolling_Positions.Length;
            }
            agent.SetDestination(Petrolling_Positions[Petrolling_Position_Index]);
        }
    }

    private bool IsAnimationRunning(string animationName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f && animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    private void PlayAnimation(int animationId)
    {
        animator.Play(animationId);
    }

    private bool IsAnimationComplete(string animationName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f && animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    private bool PlayingAnimationName(string animationName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Petrolling_Positions.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Petrolling_Positions[i], .2f);
            if (i < Petrolling_Positions.Length - 1)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(Petrolling_Positions[i], Petrolling_Positions[i + 1]);
            }
        }
        if (Petrolling_Positions.Length > 1)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Petrolling_Positions[^1], Petrolling_Positions[0]);
        }
    }
}
