using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 offset;

    void Start()
    {
        startPosition = transform.position;
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMousePos();
        transform.localScale = Vector3.one;
        AudioManager.Instance.PlaySound(AudioManager.Instance.pickUpSound);
    }

    void OnMouseDrag()
    {
        transform.position = GetMousePos() + offset;
    }

    void OnMouseUp()
    {
        // เมื่อปล่อยเมาส์ ให้เช็กว่าวางได้ไหม?
        if (CanPlaceShape())
        {
            PlaceShape(); // ถกต้อง -> วางเลย!
        }
        else
        {
            transform.position = startPosition; // ผิดกติกา -> เด้งกลับ
            transform.localScale = new Vector3(0.6f, 0.6f, 1f);
        }
    }

    // ฟังก์ชันตรวจสอบว่าวางได้ไหม
    bool CanPlaceShape()
    {
        // วนลูปเช็กชิ้นส่วนสี่เหลี่ยมเล็กๆ "ทุกชิ้น" ในรูปทรงนี้
        foreach (Transform child in transform)
        {
            // หาว่าบล็อกเล็กชิ้นนี้ อยู่ตรงกับช่องไหนของตาราง (X,Y)
            Vector2Int gridPos = BoardGenerator.Instance.GetGridPos(child.position);

            // 1. เช็กว่าล้นกรอบตาราง 8x8 ไหม
            if (gridPos.x < 0 || gridPos.x >= BoardGenerator.Instance.width ||
                gridPos.y < 0 || gridPos.y >= BoardGenerator.Instance.height)
            {
                return false; // ล้นกรอบ วางไม่ได้
            }

            // 2. เช็กว่าช่องนั้นมีบล็อกอื่นวางอยู่ก่อนแล้วหรือเปล่า
            if (BoardGenerator.Instance.grid[gridPos.x, gridPos.y] != null)
            {
                return false; // ทับซ้อน วางไม่ได้
            }
        }
        return true; // ผ่านทุกเงื่อนไข แปลว่าวางได้ 100%
    }

    // ฟังก์ชันดูดเข้าตาราง (Snap) และจำข้อมูล
    void PlaceShape()
    {
        // 1. ดึงบล็อกลูกก้อนแรกมาเป็นจุดอ้างอิงในการ Snap
        Transform referenceBlock = transform.GetChild(0);

        // 2. หาว่าบล็อกลูกก้อนแรกนี้ ควรจะไปตกที่ช่องตาราง (X,Y) ไหน
        Vector2Int targetGridPos = BoardGenerator.Instance.GetGridPos(referenceBlock.position);

        // 3. คำนวณหาตำแหน่ง (World Position) ที่แท้จริงกึ่งกลางของช่องตารางนั้น
        float targetX = (targetGridPos.x - (BoardGenerator.Instance.width / 2f) + 0.5f) * BoardGenerator.Instance.offset;
        float targetY = (targetGridPos.y - (BoardGenerator.Instance.height / 2f) + 0.5f) * BoardGenerator.Instance.offset;
        Vector3 exactTargetPos = new Vector3(targetX, targetY, 0);

        // 4. หาความห่างระหว่างตำแหน่งปัจจุบัน กับ ตำแหน่งเป้าหมาย แล้วสั่งให้ตัวแม่ขยับไปตามนั้น!
        Vector3 moveOffset = exactTargetPos - referenceBlock.position;
        transform.position += moveOffset; // ขยับตัวแม่ บล็อกลูกทุกตัวจะตามไปลงล็อกพอดีเป๊ะ
        transform.SetParent(BoardGenerator.Instance.transform);

        // ---------------------------------------------------
        // โค้ดส่วนที่เหลือยังคงเหมือนเดิมครับ (บันทึกและล้างข้อมูล)

        foreach (Transform child in transform)
        {
            Vector2Int gridPos = BoardGenerator.Instance.GetGridPos(child.position);
            BoardGenerator.Instance.grid[gridPos.x, gridPos.y] = child;
            AudioManager.Instance.PlaySound(AudioManager.Instance.placeSound);
        }
        
        Destroy(GetComponent<DragAndDrop>());
        Destroy(GetComponent<BoxCollider2D>());
        BoardGenerator.Instance.CheckForMatches();
        ShapeSpawner.Instance.ShapePlaced();
    }

    Vector3 GetMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}