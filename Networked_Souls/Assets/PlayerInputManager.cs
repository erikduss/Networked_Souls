using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    PlayerControls playerControls;

    [SerializeField] Vector2 movementInput;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        //when the scene changes, run this logic.
        SceneManager.activeSceneChanged += OnSceneChange;

        instance.enabled = false;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        //If we are loading into our world scene, enable our player controls.
        if(newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
        }
        //otherwise we must be in the main menu, disable player controls.
        //This is so our player cant move around if we enter things like a character creation menu etc.
        else
        {
            instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        //if we destroy this object, unsubscribe from this event
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
}
