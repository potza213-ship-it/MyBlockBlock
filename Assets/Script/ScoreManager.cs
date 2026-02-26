using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText; // ช่องนี้เอาไว้ใส่คะแนนปัจจุบันตอนเล่นปกติ

    private int score = 0;
    private int highScore = 0; // ตัวแปรนี้เอาไว้แอบจำสถิติเงียบๆ หลังบ้าน

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // แอบดึงสถิติเก่าจากเครื่องมาเตรียมไว้ (ไม่โชว์ให้ใครเห็น)
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreText();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();

        // แอบเช็กเงียบๆ ว่าคะแนนปัจจุบัน แซงสถิติเก่าไปหรือยัง?
        if (score > highScore)
        {
            highScore = score; // ถ้าแซงแล้ว ก็อัปเดตสถิติใหม่

            // บันทึกลงเครื่องทันที! (เซฟแบบนี้รับรองไม่หาย)
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score;
        }
    }
}