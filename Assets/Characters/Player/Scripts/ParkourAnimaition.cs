using UnityEngine;

public class ParkourAnimaition : MonoBehaviour
{
    [SerializeField] Animator Animator;
    [SerializeField] Player_Locomotion Player_Locomotion;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Animator.StopPlayback();
            Animator.Play("Jump to Grab");
            Player_Locomotion.Speed = 0;
        }
    }
    void Update()
    {
            
        if(Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump to Grab")  && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Animator.StopPlayback();
            Animator.Play("Freehang Idle");
        }
        
    }
}
