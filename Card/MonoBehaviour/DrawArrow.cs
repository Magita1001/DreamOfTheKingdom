using UnityEngine;

public class DrawArrow : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Vector3 mousePos;

    public int pointCount;    //点的数量
    public float arcModifier; //描绘贝塞尔曲线的形状

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

        SetArrowPosition();
    }

    public void SetArrowPosition()
    {
        Vector3 cardPosition = transform.position; //卡牌位置
        Vector3 direction = mousePos - cardPosition; //卡牌到鼠标的向量
        Vector3 normalizedDirection = direction.normalized; //归一化

        //计算垂直于卡牌到鼠标方向的向量
        Vector3 perpendicular = new Vector3(-normalizedDirection.y, normalizedDirection.x, normalizedDirection.z);

        //设置控制点的偏移量
        Vector3 offset = perpendicular * arcModifier; //可以通过调整这个值来改变曲线的形状

        Vector3 controlPoint = (cardPosition + mousePos) / 2 + offset; //控制点

        lineRenderer.positionCount = pointCount; //设置 LineRenderer 的点的数量

        for (int i = 0; i < pointCount; i++)
        {
            float t = i / (float)(pointCount - 1);
            Vector3 point = CalculateQuadraticBezieePoint(t, cardPosition, controlPoint, mousePos);
            lineRenderer.SetPosition(i, point);
        }
    }

    //计算二次贝塞尔曲线点
    private Vector3 CalculateQuadraticBezieePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0; //第一项
        p += 2 * u * t * p1; //第二项
        p += tt * p2;        //第三项

        return p;
    }
}
