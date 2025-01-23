using UnityEngine;

public class SwordHandler : MonoBehaviour
{
    [SerializeField] private Collider EnemyweaponCollider;
    [SerializeField] MeshRenderer MR_SheatheWeapon;
    [SerializeField] MeshRenderer MR_EquipWeapon;
    public float DamegeToPlayer;
    private void Start()
    {
        // Try to get the Collider if not assigned via Inspector
        if (EnemyweaponCollider == null)
        {
            EnemyweaponCollider = GetComponent<Collider>();
            if (EnemyweaponCollider == null)
            {
                Debug.LogError("No Collider component found on this GameObject.");
            }
        }
    }
    public void DisableEquipWeaponAndEnableSheatheWeapon()
    {


        MR_EquipWeapon.enabled = false;//SetActive(false);
        MR_SheatheWeapon.enabled = true;//SetActive(true);

    }

    // Method to hide the sheathed weapon and show the equipped weapon
    // this is an Animation Event
    public void DisableSheatheWeaponAndEnableEquipWeapon()
    {
        MR_SheatheWeapon.enabled = false;//SetActive(true);
        MR_EquipWeapon.enabled = true;//SetActive(false);
    }
    public void EnemyweaponCollider_ON()
    {
        if (EnemyweaponCollider != null)
        {
            EnemyweaponCollider.enabled = true;
        }
        else
        {
            Debug.LogWarning("Weapon collider is not assigned.");
        }
    }
    public void EnemyweaponCollider_OFF()
    {
        if (EnemyweaponCollider != null)
        {
            EnemyweaponCollider.enabled = false;

        }
        else
        {
            Debug.LogWarning("Weapon collider is not assigned.");
        }
    }
}
