using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // ศูนย์กลางให้โค้ดอื่นเรียกใช้

    public AudioSource audioSource; // ลำโพง

    // ช่องสำหรับใส่ไฟล์เสียงต่างๆ
    public AudioClip pickUpSound;
    public AudioClip placeSound;
    public AudioClip clearSound;
    public AudioClip gameOverSound;

    void Awake()
    {
        Instance = this;
    }

    // ฟังก์ชันสำหรับสั่งเล่นเสียง
    public void PlaySound(AudioClip clip)
    {
        // เช็กว่ามีไฟล์เสียงใส่ไว้ไหม ถ้ามีให้เล่นเสียงนั้น 1 ครั้งทับซ้อนกันได้
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}