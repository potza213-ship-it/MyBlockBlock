using UnityEngine;
using UnityEngine.SceneManagement; // จำเป็นสำหรับการโหลดฉาก

public class GameMenuManager : MonoBehaviour
{
    // ฟังก์ชันสำหรับสลับกลับไปหน้าเมนู
    public void GoToMainMenu()
    {
        // พิมพ์ชื่อฉากเมนูของคุณให้เป๊ะๆ (ที่เราตั้งไว้คือ MainMenu)
        SceneManager.LoadScene("MainMenu");
    }

    // ฟังก์ชันสำหรับปุ่ม "เล่นใหม่" (Restart)
    public void RestartGame()
    {
        // โหลดฉากปัจจุบันซ้ำอีกรอบ เพื่อเริ่มใหม่ทั้งหมด
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}