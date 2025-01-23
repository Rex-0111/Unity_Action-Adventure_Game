using UnityEngine;

public class SwordEventHandler : MonoBehaviour
{
    [SerializeField] private GameObject SheatheWeapon;
    [SerializeField] private GameObject EquipWeapon;
    private MeshRenderer MR_SheatheWeapon;
    private MeshRenderer MR_EquipWeapon;
    [SerializeField] private Collider weaponCollider;
    
    private void Start()
    {
        MR_SheatheWeapon = SheatheWeapon.GetComponent<MeshRenderer>();
        MR_EquipWeapon = EquipWeapon.GetComponent<MeshRenderer>();
        // Try to get the Collider if not assigned via Inspector
        if (weaponCollider == null)
        {
            weaponCollider = GetComponent<Collider>();
            if (weaponCollider == null)
            {
                Debug.LogError("No Collider component found on this GameObject.");
            }
        }
    }

    // Method to hide the equip weapon and show the sheathed weapon
    // this is an Animation Event
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

    // Method to enable the weapon collider 
    // These Are Animation Event 
    public void WeaponCollider_ON()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;   
        }
        else
        {
            Debug.LogWarning("Weapon collider is not assigned.");
        }
    }

    // Method to disable the weapon collider
    // this is an Animation Event
    public void WeaponCollider_OFF()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
            
        }
        else
        {
            Debug.LogWarning("Weapon collider is not assigned.");
        }
    }
}
