using System;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerCharControler : MonoBehaviour
{
        [Header("References")] [Tooltip("Reference to the main camera used for the player")]
        public Camera PlayerCamera;

        //[Tooltip("Audio source for footsteps, jump, etc...")]
        //public AudioSource AudioSource;

        [Header("General")] [Tooltip("Force applied downward when in the air")]
        public float GravityDownForce = 20f;

        //[Tooltip("Physic layers checked to consider the player grounded")]
        //public LayerMask GroundCheckLayers = -1;

        //[Tooltip("distance from the bottom of the character controller capsule to test for grounded")]
        //public float GroundCheckDistance = 1.1f;

        [Header("Movement")] [Tooltip("Max movement speed when grounded (when not sprinting)")]
        public float MaxSpeedOnGround = 3f;

        [Tooltip(
            "Sharpness for the movement when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite")]
        public float MovementSharpnessOnGround = 10;

        [Tooltip("Max movement speed when crouching")] [Range(0, 1)]
        public float MaxSpeedCrouchedRatio = 0.5f;

        [Tooltip("Max movement speed when not grounded")]
        public float MaxSpeedInAir = 1f;

        [Tooltip("Acceleration speed when in the air")]
        public float AccelerationSpeedInAir = 25f;

        [Tooltip("Multiplicator for the sprint speed (based on grounded speed)")]
        public float SprintSpeedModifier = 2f;

        [Tooltip("Height at which the player dies instantly when falling off the map")]
        public float KillHeight = -50f;
        
        [Header("Jump")] [Tooltip("Force applied upward when jumping")]
        public float JumpForce = 9f;
        
        //[Tooltip("Speed of crouching transitions")]
        //public float CrouchingSharpness = 10f;
        

        //[Tooltip("Sound played when jumping")] public AudioClip JumpSfx;
        //[Tooltip("Sound played when landing")] public AudioClip LandSfx;

    private Vector3 m_CharacterVelocity;
    public Vector3 m_ExternalVelocity;
    private float m_LastTimeJumped;
    private float _camXRotation = 0f;
    private CharacterController m_CharacterController;
    private PlayerInputHandler m_PlayerInputHandler;

    private bool m_wasChrouching = false;
    //private bool isGrounded;
    //private bool wasGrounded;
    //Settings:
    private float _sensitivity;
    //FOV
    void Start()
    {
        //import settings
        _sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        //get Components on same GameObject
        m_CharacterController = GetComponent<CharacterController>();
        m_PlayerInputHandler = GetComponent<PlayerInputHandler>();
        Cursor.lockState = CursorLockMode.Locked;
    }   

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < KillHeight)
        {
            Teleport(new Vector3(0f,2f,0f));
            Debug.LogWarning("dead");
        }
        ManageView();
        ManageMovement();
    }

    void ManageView()
    {
        //ca. 4 work because lost and lookF was locally defined
        //Vertical: locked all 3 axis in unity
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * _sensitivity * 250 * Time.deltaTime);
        
        //horizontal
        _camXRotation += Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sensitivity * -250;
        _camXRotation = Mathf.Clamp(_camXRotation, -89f, 89f);
        PlayerCamera.transform.localEulerAngles = new Vector3(_camXRotation, 0, 0);
    }

    void Teleport(Vector3 position)
    {
        m_CharacterController.enabled = false;
        transform.SetPositionAndRotation(position, transform.rotation);
        m_CharacterController.enabled = true;
    }
    void ManageMovement()
    {

        if (m_CharacterController.isGrounded)
        {
            //handle different speedmultipliers
            float speedMultiplier = 1f;
            if (m_PlayerInputHandler.IsSprinting())
            {
                speedMultiplier = SprintSpeedModifier;
            }
            else if (m_PlayerInputHandler.IsCrouching())
            {
                speedMultiplier = MaxSpeedCrouchedRatio;
            }
            if (m_PlayerInputHandler.IsCrouching() && m_PlayerInputHandler.IsSprinting())
            {
                speedMultiplier = MaxSpeedCrouchedRatio + SprintSpeedModifier * 0.5f;
            }
            //handle the camera height when crouching
            if (!m_wasChrouching && m_PlayerInputHandler.IsCrouching())
            {
                PlayerCamera.transform.position -= new Vector3(0f, 0.25f, 0f);
            }
            if (m_wasChrouching && !m_PlayerInputHandler.IsCrouching())
            {
                PlayerCamera.transform.position += new Vector3(0f, 0.25f, 0f);
            }
            
            //get jump input
            if (m_PlayerInputHandler.IsJumping())
            {
                m_CharacterVelocity += JumpForce * Vector3.up;
                m_LastTimeJumped = Time.time;       
            }
            
            //smooth the movement
            Vector3 targetVelocity = m_PlayerInputHandler.GetMovementInput() * (MaxSpeedOnGround * speedMultiplier);
            m_CharacterVelocity = Vector3.Lerp(m_CharacterVelocity, targetVelocity,
                MovementSharpnessOnGround * Time.deltaTime);
            //works beautiful!
            //Add External Velocity sources to m_CharVel
            m_CharacterVelocity += m_ExternalVelocity;
            m_ExternalVelocity = Vector3.zero;
        }else // if not grounded
        {
            Vector3 targetVelocity = m_PlayerInputHandler.GetMovementInput() * MaxSpeedInAir;
            /*m_CharacterVelocity = Vector3.Lerp(m_CharacterVelocity, targetVelocity,
                MovementSharpnessOnGround * Time.deltaTime);*/
            m_CharacterVelocity += Vector3.down * (GravityDownForce * Time.deltaTime);
            // limit air speed to a maximum, but only horizontally
            float verticalVelocity = m_CharacterVelocity.y;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(m_CharacterVelocity, Vector3.up);
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, MaxSpeedInAir);
            m_CharacterVelocity = horizontalVelocity + (Vector3.up * (verticalVelocity));
            
            //gravity
            
        }
        m_CharacterController.Move(m_CharacterVelocity * MaxSpeedOnGround);
        //m_CharacterVelocity *= 0.9f;
        
        
        //handle was ... variables
        m_wasChrouching = m_PlayerInputHandler.IsCrouching();
    }

    /*private void GroundCheck()
    {
        wasGrounded = isGrounded;
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, GroundCheckDistance, GroundCheckLayers);
    }*/

}