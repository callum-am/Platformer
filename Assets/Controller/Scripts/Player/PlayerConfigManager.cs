using UnityEngine;

public class PlayerConfigManager : MonoBehaviour
{
    private static PlayerConfigManager instance;
    public PlayerConfig Config { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Config = PlayerConfig.LoadFromFile();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnApplicationQuit()
    {
        Config?.SaveToFile();
    }

    public static PlayerConfigManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("PlayerConfigManager");
                instance = go.AddComponent<PlayerConfigManager>();
            }
            return instance;
        }
    }
}
