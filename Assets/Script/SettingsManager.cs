using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [Header("ลาก SettingsPanel มาใส่ช่องนี้")]
    public GameObject settingsPanel;

    void Start()
    {
        // เริ่มเกมมา ให้ซ่อนหน้าต่างตั้งค่าไว้ก่อน
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    // ฟังก์ชันสำหรับกดปุ่มเปิด
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    // ฟังก์ชันสำหรับกดปุ่มปิด
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
}