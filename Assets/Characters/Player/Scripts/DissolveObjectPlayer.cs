using System;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Renderer))]
public class DissolveObjectPlayer : MonoBehaviour
{
    float currentValue = 0f;
    readonly float targetValue = 1f;

    [Range(0f, 1f)][SerializeField] float transitionSpeed = 0.05f;

    public bool SpawnEffect;
    public bool DeSpawnEffect;
    private Player_Health_System Player_Health_System;
    private Player_Attacks Player_Attacks;
    public Material material;
    private float t = 0f; // Start t at 0 initially.
    private float ColorBlendTime = 0f;

    // Events
    public event Action Spawn;
    public event Action DeSpawn;
    
    private void Awake()
    {
        Player_Attacks = GetComponentInParent<Player_Attacks>();

        Player_Health_System = GetComponentInParent<Player_Health_System>();
        if (Player_Health_System == null)
        {
            Debug.LogError("Player_Health_System is null");
        }
    }
    private void Start()
    {
        Player_Health_System.PlayerDeath += PlayerDespawn;
        Player_Health_System.PlayerRespawn += PlayerRespawn;
    }

    void PlayerDespawn()
    {
        DeSpawnEffect = true;
        SpawnEffect = false;
    }
    void PlayerRespawn()
    {
        DeSpawnEffect = false;
        SpawnEffect = true;
    }
    void Update()
    {
        // If SpawnEffect is true, increment t from 0 to 1
        if (SpawnEffect && !DeSpawnEffect)
        {
            ColorBlendTime += Time.deltaTime * transitionSpeed;
            t += Time.deltaTime * transitionSpeed;
            t = Mathf.Clamp01(t); // Ensure t stays between 0 and 1
            PerformDissolveEffect(ref currentValue, targetValue);
        }
        // If DeSpawnEffect is true, decrement t from 1 to 0
        if (DeSpawnEffect && !SpawnEffect)
        {
            ColorBlendTime -= Time.deltaTime * transitionSpeed;
            t -= Time.deltaTime * transitionSpeed;
            t = Mathf.Clamp01(t); // Ensure t stays between 0 and 1
            PerformDissolveEffect(ref currentValue, targetValue);
        }
    }

    private void PerformDissolveEffect(ref float startValue, float endValue)
    {

        float colorblend = Mathf.PingPong(ColorBlendTime, 1f) * 1.99f - 1f;


        // Lerp the dissolve effect based on the value of t
        float height = Mathf.Lerp(startValue, endValue, t);
        if (height <= -8 || height >= 1)
            { DeSpawn.Invoke(); }
        else { Spawn.Invoke(); }

        //MaterialHeightValue = height;
        material.SetFloat("_Alpha", height);
        if(Player_Attacks.isNotEquipped == false)
        {
        material.SetFloat("_ColorBlend", colorblend);
        }
    }

}

