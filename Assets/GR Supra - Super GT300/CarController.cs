using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class CarController : MonoBehaviour
{
    IAA_Player action;
    [SerializeField] Transform TFR, TFL, TRR, TRL;
    [SerializeField] WheelCollider WC_FR, WC_FL, WC_RR, WC_RL;
    [SerializeField] float SteerAngle = 30f;  // Changed to float for better precision
    [SerializeField] float Speed ;     // Changed to float
    [SerializeField] float BreakTorque ; // Changed to float
    [SerializeField] bool ControllingCar;
    [SerializeField] GameObject PlayerCamera, vehical_camera , MainPlayer, PlayerFollow;
    [SerializeField] GameObject[] Player;
    Rigidbody rb;
    [SerializeField] float RotationPower;
    [SerializeField] Light[] Lights; 
    private void Awake()
    {
        action = new IAA_Player();
        rb = GetComponent<Rigidbody>(); 
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Combine the "E" and "R" key logic as they both do the same thing
            if (Input.GetKeyDown(KeyCode.E))
            {               
                SwitchToCarControl();
            }
        }
    }
    
    private void SwitchToPlayerControl()
    {
        ControllingCar = false;
        foreach (GameObject player in Player)
        {
            WC_FL.motorTorque = 0;
            WC_FR.motorTorque = 0;
            WC_RL.motorTorque = 0;
            WC_RR.motorTorque = 0;
            player.SetActive(true);
        }
        MainPlayer.transform.position = PlayerFollow.transform.position;
        PlayerCamera.SetActive(true);
        vehical_camera.SetActive(false);
        LightColor(0,0);
    }

    private void LightColor(uint value1, uint value2)
    {
        Lights[0].intensity = value1;
        Lights[1].intensity = value1;
        Lights[2].intensity = value2;
        Lights[3].intensity = value2;
        Lights[4].intensity = value2;
    }

    private void SwitchToCarControl()
    {
        ControllingCar = true;
        foreach (GameObject player in Player) 
        {
            player.SetActive(false); 
        }
        PlayerCamera.SetActive(false);
        vehical_camera.SetActive(true);
        LightColor(1000,50);
    }

    private void ApplyBrakes()
    {
        float brakeValue = action.CarControls.Brakes.ReadValue<float>();
        WC_FR.brakeTorque = BreakTorque * brakeValue;
        WC_FL.brakeTorque = BreakTorque * brakeValue;
        WC_RR.brakeTorque = BreakTorque * brakeValue;
        WC_RL.brakeTorque = BreakTorque * brakeValue;
    }

    // Update is called once per frame
    void Update()
    {
   
        if (ControllingCar)
        {
            //rb.AddForce(new Vector3(transform.position.x, transform.position.y+5, transform.position.z));

            CastRay();
            ApplyBrakes();
            Vector2 movementValue = action.CarControls.Move.ReadValue<Vector2>();
            BackAndForthMovement(movementValue);
            LeftAndRightMovement(movementValue);
            if (Input.GetKeyDown(KeyCode.R))
            {
                SwitchToPlayerControl();
            }
        }
    }

    void CastRay()
    {
        Ray ray1 = new Ray(transform.position, transform.up); // Up direction
        Ray ray2 = new Ray(transform.position, transform.right); // Right direction
        Ray ray3 = new Ray(transform.position, -transform.right); // Left direction

        RaycastHit hittop;
        RaycastHit hitleft;
        RaycastHit hitright;

        bool hitTop = Physics.Raycast(ray1, out hittop, 3f);
        bool hitLeft = Physics.Raycast(ray2, out hitleft, 3f);
        bool hitRight = Physics.Raycast(ray3, out hitright, 3f);

        if (hitTop || hitLeft || hitRight)
        {
            // Check if any of the hits are valid (not null)
            if (hittop.collider != null || hitleft.collider != null || hitright.collider != null)
            {
                rb.AddTorque(Vector3.up * RotationPower); // Apply torque if any ray hits an object
            }
        }
    }

    private void BackAndForthMovement(Vector2 value)
    {
        // Apply motor torque to all wheels
        WC_FR.motorTorque = value.y * Speed;
        WC_FL.motorTorque = value.y * Speed;
        WC_RR.motorTorque = value.y * Speed;
        WC_RL.motorTorque = value.y * Speed;
        UpdateWheelRotation();
    }

    private void UpdateWheelRotation()
    {
        // Update the visual wheel transforms based on wheel collider's current rotation.
        UpdateWheelPosition(WC_FR, TFR);
        UpdateWheelPosition(WC_FL, TFL);
        UpdateWheelPosition(WC_RR, TRR);
        UpdateWheelPosition(WC_RL, TRL);
    }

    private void UpdateWheelPosition(WheelCollider wheelCollider, Transform wheelTransform)
    {
        // WheelCollider provides position and rotation info via GetWorldPose
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);

        // Apply the position and rotation of the wheel collider to the visual wheel transform
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void LeftAndRightMovement(Vector2 value)
    {
        // Apply steering to front wheels
        WC_FL.steerAngle = value.x * SteerAngle;
        WC_FR.steerAngle = value.x * SteerAngle;
    }
}
