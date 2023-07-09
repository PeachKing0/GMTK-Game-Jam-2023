using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Make a struct that can be seen in the editor that links a level to a music track
[System.Serializable]
public struct LevelTrack
{
    public string Track; //Purely for debugging purposes (changes the elements name in the list in the inspector)
    public string LevelName;
    public bool hasPlayed;
    public AudioClip Music;
    public bool loop;
};

public class MusicPlayer : MonoBehaviour
{
    //Make sure only one exists
    public static MusicPlayer Instance;

    //Array of tracks and the levels they go with
    public LevelTrack[] levelTracks;

    private AudioSource myAud;

    //Check that there is only one MusicPlayer
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        myAud = GetComponent<AudioSource>();
        //Play first track
        myAud.Stop();
        FindRightTrack();
    }

    // Update is called once per frame
    void Update()
    {
        if (!myAud.isPlaying) FindRightTrack();
    }

    private void OnLevelWasLoaded(int level)
    {
        //Reset the "queue" and stop playing (prevent bugs)
        for (int i = 0; i < levelTracks.Length; i++)
            levelTracks[i].hasPlayed = false;
        myAud.Stop(); //Generates an error when swapping scenes???? Ignore said error

        FindRightTrack();
    }

    //This should check if the correct music is playing on any given level, and allow levels that share a track to keep playing rather than start and stop
    private void FindRightTrack()
    {
        foreach (LevelTrack LT in levelTracks)
        {
            if (SceneManager.GetActiveScene().name == LT.LevelName && !LT.hasPlayed)
            {
                myAud.clip = LT.Music;
                myAud.loop = LT.loop;
                //CHANGE THIS LINE EVENTUALLY | SHOULD NOT ALWAYS BE INDEX 0
                levelTracks[0].hasPlayed = true;
                myAud.Play();
                break; //heh
            }
        }
    }

    //Used for volume slider
    public void ChangeVolume(Slider slider)
    {
        myAud.volume = slider.value;
    }

    //Used for toggle
    public void ToggleMusic(Toggle toggle)
    {
        myAud.mute = !toggle.isOn; //the "!" is janky, prevents weird reversal bug
    }
}
