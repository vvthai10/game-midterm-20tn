using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image image1;
    public Image image2;

    public Sprite hoverSprite1;
    public Sprite hoverSprite2;

    private Sprite originalSprite1;
    private Sprite originalSprite2;

    private void Start()
    {
        // Lưu trữ hình ảnh gốc của hai UI Image con
        originalSprite1 = image1.sprite;
        originalSprite2 = image2.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Thay đổi hình ảnh của hai UI Image con khi hover chuột vào button
        image1.sprite = hoverSprite1;
        image2.sprite = hoverSprite2;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Khôi phục hình ảnh gốc của hai UI Image con khi chuột rời khỏi button
        image1.sprite = originalSprite1;
        image2.sprite = originalSprite2;
    }
}
