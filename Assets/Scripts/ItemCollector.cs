using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelScore
{
    public int levelScenes;
    public int highestBananaCount;
}

[System.Serializable]
public class GameScore
{
    public List<LevelScore> levelScores = new List<LevelScore>();
}

public class ItemCollector : MonoBehaviour
{
    private int banana = 0;
    private int highestBanana = 0;
    private int level = 0;

    [SerializeField] private Text points;
    [SerializeField] private Text highestpoints;
    [SerializeField] private Text levelMap;
    [SerializeField] private AudioSource collectionSoundEffect;

    private string filePath;

    private void Start()
    {
        level = SceneManager.GetActiveScene().buildIndex;
        filePath = Path.Combine(Application.persistentDataPath, "SaveFile.json");
        LoadScoreData();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Banana"))
        {
            collectionSoundEffect.Play();
            Destroy(collision.gameObject);
            banana++;
            Debug.Log("Banana: " + banana);
            points.text = "Points: " + banana;
            UpdateScoreData();
            highestpoints.text = "highest points: " + highestBanana;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 normal = collision.contacts[0].normal;

            // Nếu va chạm từ phía trên
            if (normal == Vector2.up)
            {
                collectionSoundEffect.Play();
                Destroy(collision.gameObject);
                banana++;
                Debug.Log("Banana: " + banana);
                points.text = "Points: " + banana;
                UpdateScoreData();
                highestpoints.text = "highest points: " + highestBanana;
            }
        }
    }

    private void UpdateScoreData()
    {
        // Kiểm tra xem có điểm nào được lưu cho cấp độ hiện tại không
        if (banana > highestBanana)
        {
            highestBanana = banana;
            SaveScoreData();
        }
    }

    private void SaveScoreData()
    {
        GameScore gameScore = new GameScore();

        // Đọc dữ liệu từ tệp JSON hiện có
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            gameScore = JsonUtility.FromJson<GameScore>(jsonData);
        }

        // Kiểm tra xem dữ liệu cho cấp độ hiện tại đã tồn tại trong danh sách hay chưa
        bool found = false;
        foreach (var score in gameScore.levelScores)
        {
            if (score.levelScenes == level)
            {
                // Nếu tồn tại, cập nhật điểm cao nhất
                score.highestBananaCount = highestBanana;
                found = true;
                break;
            }
        }

        // Nếu không tìm thấy dữ liệu cho cấp độ hiện tại, thêm mới vào danh sách
        if (!found)
        {
            LevelScore newScore = new LevelScore();
            newScore.levelScenes = level;
            newScore.highestBananaCount = highestBanana;
            gameScore.levelScores.Add(newScore);
        }

        // Lưu dữ liệu vào tệp JSON
        string jsonDataUpdated = JsonUtility.ToJson(gameScore);
        File.WriteAllText(filePath, jsonDataUpdated);
    }

    private void LoadScoreData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            GameScore gameScore = JsonUtility.FromJson<GameScore>(jsonData);
            foreach (var score in gameScore.levelScores)
            {
                if (score.levelScenes == level)
                {
                    highestBanana = score.highestBananaCount;
                    highestpoints.text = "highest points: " + highestBanana;
                    levelMap.text = "level: " + level;
                    break;
                }
            }
        }
    }
}
