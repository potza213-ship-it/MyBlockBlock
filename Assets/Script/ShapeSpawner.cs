using UnityEngine;
using UnityEngine.SceneManagement;

public class ShapeSpawner : MonoBehaviour
{
    public Color[] shapeColors;
    public static ShapeSpawner Instance; // ตั้งเป็นศูนย์กลางให้โค้ดอื่นเรียกใช้ง่ายๆ

    public GameObject[] shapePrefabs; // กล่องสำหรับใส่ Prefab รูปทรงต่างๆ
    public Transform[] spawnPoints;   // กล่องสำหรับใส่จุดเกิด 3 จุด
    public GameObject gameOverPanel; // ช่องสำหรับใส่หน้าต่าง Game Over

    private int shapesRemaining = 0;  // ตัวนับว่าเหลือบล็อกกี่ชิ้น

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnShapes(); // เริ่มเกมปุ๊บ สุ่มบล็อกให้เลย
    }

    public void SpawnShapes()
    {
        // วนลูปตามจำนวนจุดเกิด (3 จุด)
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int randomIndex = Random.Range(0, shapePrefabs.Length);
            GameObject newShape = Instantiate(shapePrefabs[randomIndex], spawnPoints[i].position, Quaternion.identity);
            newShape.transform.parent = spawnPoints[i];
            newShape.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
            // ---------------------------------------------------------
            // [เพิ่มโค้ดส่วนนี้เข้าไปครับ!] ระบบสุ่มสีบล็อก
            // 1. เช็กก่อนว่าเราได้ใส่สีเตรียมไว้ใน Inspector หรือยัง
            if (shapeColors != null && shapeColors.Length > 0)
            {
                // 2. สุ่มเลือกสีมา 1 สีจากหน้า Inspector
                Color randomColor = shapeColors[Random.Range(0, shapeColors.Length)];

                // 3. ค้นหาชิ้นส่วนลูกๆ ทั้งหมด (บล็อกสี่เหลี่ยมเล็กๆ) แล้วสั่งเปลี่ยนสี
                SpriteRenderer[] renderers = newShape.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sr in renderers)
                {
                    sr.color = randomColor;
                }
            }
            // ---------------------------------------------------------
        }

        shapesRemaining = spawnPoints.Length;
        CheckGameOver();
    }

    // ฟังก์ชันนี้จะถูกเรียกตอนที่ผู้เล่นวางบล็อกลงกระดานสำเร็จ
    public void ShapePlaced()
    {
        shapesRemaining--;

        if (shapesRemaining <= 0)
        {
            SpawnShapes(); // ถ้าบล็อกหมด ให้เสกใหม่ (และใน SpawnShapes จะมี CheckGameOver อยู่แล้ว)
        }
        else
        {
            // [เพิ่มตรงนี้!] ถ้าบล็อกยังเหลือ แต่เพิ่งวางไป ก็ต้องเช็กด้วยว่าบล็อกที่เหลือมีที่ให้ลงไหม!
            CheckGameOver();
        }
    }

    // ---------------------------------------------------------
    // ฟังก์ชันตรวจสอบ Game Over
    // ---------------------------------------------------------
    public void CheckGameOver()
    {
        bool canPlaceAny = false;

        // วนลูปเช็กบล็อกทุกชิ้นที่ยังเหลืออยู่ในจุด Spawn
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint.childCount > 0) // ถ้าจุดนี้ยังมีบล็อกอยู่
            {
                GameObject shape = spawnPoint.GetChild(0).gameObject;
                if (BoardGenerator.Instance.CanShapeFitAnywhere(shape))
                {   
                    canPlaceAny = true;
                    break; // เจอที่วางแค่ 1 ที่ก็ถือว่ายังเล่นต่อได้ ออกจากลูปเลย
                }
            }
        }

        if (!canPlaceAny)
        {
            // เกมโอเวอร์! 
            // เกมโอเวอร์! โชว์หน้าต่าง UI ขึ้นมาเลย
            gameOverPanel.SetActive(true);
            AudioManager.Instance.PlaySound(AudioManager.Instance.gameOverSound);
        }
    }
    public void RestartGame()
    {
        // สั่งโหลด Scene ปัจจุบันใหม่ทั้งหมด (เริ่มเกมใหม่)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}