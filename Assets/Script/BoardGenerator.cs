using UnityEngine;
using System.Collections.Generic;
using TMPro; // ดึงระบบ TextMeshPro มาใช้

public class BoardGenerator : MonoBehaviour
{
    [Header("Floating Text System")]
    public GameObject floatingTextPrefab;
    public Transform canvasTransform; // เอาไว้บอกว่าให้ตัวหนังสือไปโชว์ใน Canvas ไหน
    public GameObject clearEffectPrefab;

    // สร้างตัวแปร Instance เพื่อให้โค้ดอื่น (เช่น บล็อก) วิ่งมาถามข้อมูลตารางได้ง่ายๆ
    public static BoardGenerator Instance;

    public GameObject tilePrefab;
    public int width = 16;
    public int height = 16;
    public float offset = 1.1f;

    // ความจำของบอร์ด: เก็บข้อมูลว่าช่อง (X,Y) มีบล็อกอะไรวางอยู่บ้าง
    public Transform[,] grid;

    void Awake()
    {
        Instance = this; // ตั้งค่าตัวเองเป็นศูนย์กลาง
        grid = new Transform[width, height]; // สร้างตารางความจำขนาด 8x8
    }

    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float posX = (x - (width / 2f) + 0.5f) * offset;
                float posY = (y - (height / 2f) + 0.5f) * offset;
                Vector2 spawnPos = new Vector2(posX, posY);

                GameObject newTile = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
                newTile.transform.parent = this.transform;
                newTile.name = $"Tile {x},{y}";
            }
        }
    }

    // ฟังก์ชันสำคัญ! แปลงตำแหน่งจุดในจอให้เป็นเลขช่องตาราง
    public Vector2Int GetGridPos(Vector3 pos)
    {
        int x = Mathf.RoundToInt((pos.x / offset) + (width / 2f) - 0.5f);
        int y = Mathf.RoundToInt((pos.y / offset) + (height / 2f) - 0.5f);
        return new Vector2Int(x, y);
    }

    // ---------------------------------------------------------
    // ฟังก์ชันตรวจสอบแถวและแนวตั้งที่เต็ม
    // ---------------------------------------------------------
    public void CheckForMatches()
    {
        List<int> rowsToClear = new List<int>();
        List<int> colsToClear = new List<int>();

        // 1. เช็กแนวนอน (Rows)
        for (int y = 0; y < height; y++)
        {
            bool isRowFull = true;
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] == null) // ถ้าเจอช่องว่างแม้แต่ช่องเดียว ถือว่าแถวไม่เต็ม
                {
                    isRowFull = false;
                    break;
                }
            }
            if (isRowFull) rowsToClear.Add(y); // ถ้าเต็ม จำเลขแถวไว้
        }

        // 2. เช็กแนวตั้ง (Columns)
        for (int x = 0; x < width; x++)
        {
            bool isColFull = true;
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    isColFull = false;
                    break;
                }
            }
            if (isColFull) colsToClear.Add(x); // ถ้าเต็ม จำเลขแนวตั้งไว้
        }

        // 3. สั่งทำลายบล็อกในแถว/แนวตั้งที่จดจำไว้
        foreach (int y in rowsToClear) ClearRow(y);
        foreach (int x in colsToClear) ClearColumn(x);

        // ---------------------------------------------------
        // ระบบคอมโบ และเสกข้อความเด้ง
        // ---------------------------------------------------
        int totalLinesCleared = rowsToClear.Count + colsToClear.Count;
        if (totalLinesCleared > 0)
        {
            int baseScore = 100; // คะแนนต่อ 1 แถว
            int comboMultiplier = totalLinesCleared; // ระบบคอมโบ x1, x2, x3...

            // คำนวณคะแนนรวม (เช่น 2 แถว = (100 * 2) * 2 = 400 คะแนน)
            int finalScore = (baseScore * totalLinesCleared) * comboMultiplier;

            // 1. บวกคะแนนและเล่นเสียง
            ScoreManager.Instance.AddScore(finalScore);
            AudioManager.Instance.PlaySound(AudioManager.Instance.clearSound);

            // 2. เสกตัวหนังสือเด้งกลางหน้าจอ (Canvas)
            if (floatingTextPrefab != null && canvasTransform != null)
            {
                GameObject popup = Instantiate(floatingTextPrefab, canvasTransform);

                string message = "+" + finalScore;
                if (totalLinesCleared > 1)
                {
                    message += "\nCOMBO x" + totalLinesCleared + "!";
                }

                // สั่งให้ TextMeshPro เปลี่ยนข้อความ
                popup.GetComponent<TextMeshProUGUI>().text = message;
            }
        }
    } 

    // ฟังก์ชันย่อย: ทำลายบล็อกแนวนอน
    void ClearRow(int y)    
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                if (clearEffectPrefab != null)
                {
                    Instantiate(clearEffectPrefab, grid[x, y].transform.position, Quaternion.identity);
                }
                Destroy(grid[x, y].gameObject); // ทำลายวัตถุในเกม
                grid[x, y] = null;               // ลบความจำในตารางให้กลับมาว่าง
            }
        }
    }

    // ฟังก์ชันย่อย: ทำลายบล็อกแนวตั้ง
    void ClearColumn(int x)
    {
        for (int y = 0; y < height; y++)
        {
            if (grid[x, y] != null)
            {
                if (clearEffectPrefab != null)
                {
                    Instantiate(clearEffectPrefab, grid[x, y].transform.position, Quaternion.identity);
                }
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }

    // ---------------------------------------------------------
    // ระบบตรวจสอบ Game Over
    // ---------------------------------------------------------

    // ฟังก์ชันนี้จะถูกเรียกเพื่อเช็กว่า "บล็อกชิ้นนี้" วางบนกระดานได้ไหม
    public bool CanShapeFitAnywhere(GameObject shape)
    {
        // จำลองการวางตั้งแต่ช่อง X:0, Y:0 ไปจนถึงช่องสุดท้าย
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (CheckShapeAtPosition(shape, x, y))
                {
                    return true; // เจอที่ว่างแล้ว! รอดตาย ลุยต่อได้
                }
            }
        }
        return false; // ลองทุกช่องแล้ว วางไม่ได้เลยสักช่อง
    }

    // ฟังก์ชันย่อย: จำลองวางบล็อกลงในช่อง (gridX, gridY) ที่กำหนด
    bool CheckShapeAtPosition(GameObject shape, int gridX, int gridY)
    {
        Transform referenceBlock = shape.transform.GetChild(0); // ยึดบล็อกลูกก้อนแรกเป็นหลัก เหมือนตอน Snap

        foreach (Transform child in shape.transform)
        {
            // หาว่าบล็อกลูกแต่ละก้อน ห่างจากก้อนแรกกี่ช่องตาราง
            int offsetX = Mathf.RoundToInt((child.localPosition.x - referenceBlock.localPosition.x) / offset);
            int offsetY = Mathf.RoundToInt((child.localPosition.y - referenceBlock.localPosition.y) / offset);

            int targetX = gridX + offsetX;
            int targetY = gridY + offsetY;

            // 1. เช็กว่าล้นกรอบตาราง 8x8 ไหม
            if (targetX < 0 || targetX >= width || targetY < 0 || targetY >= height) return false;

            // 2. เช็กว่าช่องนั้นมีบล็อกอื่นขวางอยู่ไหม
            if (grid[targetX, targetY] != null) return false;
        }

        return true; // ไม่ล้น และ ไม่ทับ = วางได้ชัวร์!
    }
}