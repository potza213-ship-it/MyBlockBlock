using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // [เพิ่มตรงนี้!] ต้องใส่เพื่อใช้งานตัวหนังสือ TextMeshPro

public class MainMenuManager : MonoBehaviour
{
    // [เพิ่มตรงนี้!] ช่องสำหรับใส่ตัวหนังสือ High Score
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        // ทันทีที่เปิดหน้าเมนูขึ้นมา ให้ไปดึงค่า "HighScore" ที่เซฟไว้ในเครื่อง
        // (ถ้าเพิ่งโหลดเกมมาเล่นครั้งแรก ยังไม่มีคะแนน มันจะตั้งค่าเริ่มต้นให้เป็น 0)
        int bestScore = PlayerPrefs.GetInt("HighScore", 0);

        // เอาคะแนนที่ดึงมาได้ ไปอัปเดตใส่ตัวหนังสือบนหน้าจอ
        if (highScoreText != null)
        {
            highScoreText.text = "HIGH SCORE: " + bestScore;
        }
    }

    // ฟังก์ชันเริ่มเกม (ของเดิมของคุณ)
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}