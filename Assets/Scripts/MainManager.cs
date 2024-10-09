using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    public NameScript nameScript;

    private SaveData save;
    
    private bool m_Started = false;
    private int m_Points;
    private int highScore = 0;
    private string playerName;
    private bool m_GameOver = false;


    // Start is called before the first frame update
    private void Awake()
    {
        save = new SaveData();
        string path = Application.persistentDataPath + "/savefile.json";
        GameObject nameObject = GameObject.Find("NameKeeper");
        nameScript = nameObject.GetComponent<NameScript>();
        Debug.Log(nameScript.getName());
        if(File.Exists(path))
        {
            save.LoadScore();
        }
    }

    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            save.LoadScore();
            HighScoreText.text = "Best Score: " + save.getName() + ": " + save.getScore();
        }
        
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        Debug.Log("Score: " + save.highScore);
        Debug.Log("Name: " + nameScript.getName());
        if (m_Points > save.getScore())
        {
            save.highScore = m_Points;
            save.topPlayer = nameScript.getName();//change the way you save the name

            highScore = m_Points;
            
            save.SaveScore();
            
        }
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore = 0;
        public string topPlayer = "";

        public void SaveScore()
        {
            SaveData data = new SaveData();
            data.highScore = highScore;
            Debug.Log("saved " + data.highScore + " as high score");
            data.topPlayer = topPlayer;
            Debug.Log("saved " + data.topPlayer + " as name");


            string json = JsonUtility.ToJson(data);

            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }

        public void LoadScore()
        {
            string path = Application.persistentDataPath + "/savefile.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                highScore = data.highScore;
                Debug.Log("loaded " + highScore + " as high score");
                topPlayer = data.topPlayer;
                Debug.Log("loaded " + topPlayer + " as name");
            }
        }

        public string getName()
        {
            return topPlayer;
        }

        public int getScore()
        {
            return highScore;
        }
    }
}

