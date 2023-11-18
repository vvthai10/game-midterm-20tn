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
        originalSprite1 = image1.sprite;
        originalSprite2 = image2.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseClick.Instance.PlayMouseHover();
        image1.sprite = hoverSprite1;
        image2.sprite = hoverSprite2;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image1.sprite = originalSprite1;
        image2.sprite = originalSprite2;
    }
}
