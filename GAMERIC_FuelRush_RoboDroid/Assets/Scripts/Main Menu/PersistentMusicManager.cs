using UnityEngine;

public class PersistentMusicManager : MonoBehaviour
{
    // Static variable for the Singleton object:
    public static PersistentMusicManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // NOTE: 
            // - We want to keep the first and original instance at all cost!
            // - If an instance already exist, delete the second, third, fourth, ..., instance!
            Destroy(gameObject);
        }
    }
}
