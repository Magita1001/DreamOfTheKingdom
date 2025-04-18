using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class FadePanel : MonoBehaviour
{
    public VisualElement backGround;

    private void Awake()
    {
        backGround = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("BackGround");
    }

    public void FadIn(float duration)
    {
        DOVirtual.Float(0, 1, duration, value =>
        {
            backGround.style.opacity = value;
        }).SetEase(Ease.InQuad);
    }

    public void FadOut(float duration)
    {
        DOVirtual.Float(1, 0, duration, value =>
        {
            backGround.style.opacity = value;
        }).SetEase(Ease.InQuad);
    }
}
