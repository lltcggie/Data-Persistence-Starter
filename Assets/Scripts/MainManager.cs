using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private string BestScoreName;
    private int BestScore = 0;
    private string UserName = "";

    public void SetInitData(string userName)
    {
        UserName = userName;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 名前が未設定だったらUserとしておく
        if (UserName.Length == 0)
        {
            UserName = "User";
        }

        // セーブロード
        LoadSaveData();

        SetBestScoreText();

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
    }

    void LoadSaveData()
    {
        var saveData = SaveDataUtils.Load();
        if (saveData != null)
        {
            BestScoreName = saveData.BestScoreName;
            BestScore = saveData.BestScore;
        }
    }

    void SetBestScoreText()
    {
        BestScoreText.text = string.Format("Best Score : {0} : {1}", BestScoreName, BestScore);
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
                SceneManager.sceneLoaded += SceneLoaded;
                SceneManager.LoadScene("menu");
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

        UpdateBestScore();
    }

    void UpdateBestScore()
    {
        if (BestScore < m_Points)
        {
            BestScore = m_Points;
            BestScoreName = UserName;

            SaveSaveData();
        }
    }

    void SaveSaveData()
    {
        SaveData saveData = new SaveData();
        saveData.BestScore = BestScore;
        saveData.BestScoreName = BestScoreName;

        SaveDataUtils.Save(saveData);
    }

    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        // mainシーンのセットアップ
        if (nextScene.name == "menu")
        {
            var menuUIManager = GameObject.Find("Canvas").GetComponent<MenuUIManager>();
            menuUIManager.SetInitData(UserName);
        }

        SceneManager.sceneLoaded -= SceneLoaded;
    }
}
