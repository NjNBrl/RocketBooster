using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private AudioClip ignition;

    [SerializeField] private AudioClip landing;
    
    [SerializeField] private float rotationLimit;

    [SerializeField] private ParticleSystem mainjet;
    [SerializeField] private ParticleSystem rightJet;

    [SerializeField] private ParticleSystem leftJet;



    private float timeCount;
    private float thrust;

    private float moveInput;
    private float rotationInput;


    private AudioSource audio;

    private PlayerInput input;
    private Rigidbody rb;
    private float moveSpeed = 100f;

    public static float thrustStrength ;


    void Awake()
    {
        input = new PlayerInput();
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        int scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        Debug.Log(scene);
        switch (scene)
        {
            case 0: thrustStrength = 1000f; break;
            case 1: thrustStrength = 2000f; break;
            case 2: thrustStrength = 3000f; break;
            default: thrustStrength = 1000f; break;
        }
    }

    void Start()
    {
        audio.Stop();
    }
    private void OnEnable()
    {
        input.Enable();
        input.Player.Rotation.performed += ctx => rotationInput = ctx.ReadValue<float>();
        input.Player.Movement.performed +=ctx => moveInput = ctx.ReadValue<float>();
        input.Player.Thrust.performed += ctx => thrust = ctx.ReadValue<float>();

        input.Player.Movement.canceled += ctx => moveInput = 0f;
        input.Player.Thrust.canceled += ctx => thrust = 0f;
        input.Player.Rotation.canceled += ctx => rotationInput = 0f;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    // One-shot input reads belong in Update — NOT FixedUpdate
    void Update()
    {
        if (input.Player.Thrust.IsPressed()) 
        {
            timeCount = Time.time;
            thrust = 1f;

        } // just set the value here
    }

    void FixedUpdate()
    {
        HandleInput();
        HandleRotation();
    }

    private void HandleInput()
    {
        // Thrust takes priority — apply it and skip the normal dampening this tick
        if (thrust > 0f)
        {
            mainjet.Play();
            rb.AddRelativeForce(Vector3.up* thrustStrength * Time.fixedDeltaTime);
            if (!audio.isPlaying)
            {
               audio.PlayOneShot(mainEngine);

            }
            return;
        }
        else if (thrust ==0f)
        {
            audio.Stop();
            mainjet.Stop();
        }

        Vector3 vel = moveInput * moveSpeed * Time.fixedDeltaTime * Vector3.right; 

        rb.linearVelocity = new Vector3(vel.x, rb.linearVelocity.y, 0);
    


    }
    private void HandleRotation()
    {
        rb.freezeRotation = true;
        if (rotationInput > 0) 
        {
            rightJet.Stop();
            leftJet.Play();
            transform.Rotate(Vector3.forward); 
        }
        else if (rotationInput < 0)
        {
            leftJet.Stop();
            rightJet.Play();
            transform.Rotate(Vector3.back);
        }
        else
        {
            leftJet.Stop();
            rightJet.Stop();
        }
        rb.freezeRotation = false;
  
    }

    
}