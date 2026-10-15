
using UnityEngine;
using UnityEngine.InputSystem;

public class Quit : MonoBehaviour
{
    private void Start()
    {

    }
    private void Update()
    {

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("We quitting!");
            Application.Quit();
        }
    }
}
