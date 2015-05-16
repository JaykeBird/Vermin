using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    private bool paused = false;

    private GUIManager gui;
    public Animator pauseAnimator; // this is where we go to do the pausing and the animating and the yeah

    private int pauseCooldown = 0;

    public bool Paused {
        get {
            return paused;
        }
    }

    void Awake()
    {
        // Instantiates the GUIManager.
        gui = FindObjectOfType(typeof(GUIManager)) as GUIManager;
    }

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseCooldown == 0)
        {
            PauseGame();

            if (!paused)
            {
                //PauseGame();
                //gui.Pause();

                pauseAnimator.SetBool("Paused", false);
            }
            else
            {
                //PauseGame();
                //gui.Unpause();

                pauseAnimator.SetBool("Paused", true);
            }

            pauseCooldown = 40;
        }

        if (pauseCooldown > 0)
        {
            pauseCooldown--;
        }
    }

    void PauseGame()
    {
        paused = !paused;
    }

    public static GameManager Instance {
        get {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
}
