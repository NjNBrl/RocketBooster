using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioClip crash;
    [SerializeField] private ParticleSystem successParticle;
    [SerializeField] private ParticleSystem crashParticle;

    private Movement mv;
    private AudioSource audio;

    bool isControllale = true;
    bool isCollidable = true;

    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mv = GetComponent<Movement>();
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }
    private void RespondToDebugKeys() //remove this
    {
        if (Keyboard.current.lKey.isPressed)
        {
            NextLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
            Debug.Log("C was pressed");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isControllale || !isCollidable)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case ("Launchpad"):
                Debug.Log("Launch pad touched");
                break;
            case ("Finishpad"):
                Debug.Log("Finish pad touched");
                StartNextSequence();
                break;
            case ("Fuel"):
                Debug.Log("before speed" + Movement.thrustStrength);
                Movement.thrustStrength += 500;
                Debug.Log("after speed" + Movement.thrustStrength);
                collision.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                StartCoroutine(Delay(collision.gameObject));
                break;
            case ("Sky"):
                Debug.Log("Sky Touched");
                break;
            default:
                Debug.Log("Crashed");
                StartCrashSequence();
                break;
        }
    }
        private void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex ;
        SceneManager.LoadScene(currentScene);
    }

    private void NextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (currentScene == SceneManager.sceneCountInBuildSettings)
        {
            currentScene = 0;
        }
        SceneManager.LoadScene(currentScene);
    }
    private void StartCrashSequence()
    {
        audio.Stop();
        crashParticle.Play();
        isControllale = false;
        audio.PlayOneShot(crash);
        mv.enabled = false;
        Invoke("ReloadLevel", 2f);
    }
    private void StartNextSequence()
    {
        mv.enabled = false;
        audio.Stop();
        audio.PlayOneShot(success);
        successParticle.Play();
        isControllale = false;

        Invoke("NextLevel", 3f);
    }
    IEnumerator Delay(GameObject gameObject)
    {
        
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}

