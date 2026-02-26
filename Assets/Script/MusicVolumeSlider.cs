using UnityEngine;
using UnityEngine.UI; // ต้องมีบรรทัดนี้ถึงจะใช้ Slider ได้

public class MusicVolumeSlider : MonoBehaviour
{
    [Header("ใส่ Audio Source ที่ใช้เล่นเพลงตรงนี้")]
    public AudioSource musicSource;

    private Slider volumeSlider;

    void Start()
    {
        volumeSlider = GetComponent<Slider>();

        // โหลดค่าความดังที่เคยปรับไว้ (ถ้าไม่มีให้เริ่มที่ความดัง 0.5 หรือ 50%)
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        // เซ็ตค่าให้ทั้งเพลงและสไลเดอร์ตรงกัน
        if (musicSource != null)
        {
            musicSource.volume = savedVolume;
        }
        volumeSlider.value = savedVolume;

        // สั่งให้เวลาเลื่อนแถบ ให้ไปเรียกฟังก์ชัน UpdateVolume อัตโนมัติ
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
    }

    void UpdateVolume(float val)
    {
        if (musicSource != null)
        {
            musicSource.volume = val; // ปรับความดังเพลงตามแถบเลื่อน
        }

        // แอบจำค่าความดังนี้ลงเครื่องไว้ เปิดเกมรอบหน้าจะได้ดังเท่าเดิม!
        PlayerPrefs.SetFloat("MusicVolume", val);
        PlayerPrefs.Save();
    }
}