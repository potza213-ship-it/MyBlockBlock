using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 50f; // ความเร็วในการลอยขึ้น
    public float destroyTime = 1.5f; // เวลาที่จะโชว์ก่อนหายไป

    private TextMeshProUGUI textMesh;
    private Color textColor;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textColor = textMesh.color;

        // สั่งให้ทำลายตัวเองทิ้งเมื่อครบกำหนดเวลา (จะได้ไม่รกเครื่อง)
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // 1. ทำให้ลอยขึ้นด้านบน
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // 2. ทำให้ค่อยๆ จางหายไป (Fade out)
        textColor.a -= (1f / destroyTime) * Time.deltaTime;
        textMesh.color = textColor;
    }
}   