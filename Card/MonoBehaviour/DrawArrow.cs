using UnityEngine;

public class DrawArrow : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Vector3 mousePos;

    public int pointCount;    //�������
    public float arcModifier; //��汴�������ߵ���״

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
        Vector3 cardPosition = transform.position; //����λ��
        Vector3 direction = mousePos - cardPosition; //���Ƶ���������
        Vector3 normalizedDirection = direction.normalized; //��һ��

        //���㴹ֱ�ڿ��Ƶ���귽�������
        Vector3 perpendicular = new Vector3(-normalizedDirection.y, normalizedDirection.x, normalizedDirection.z);

        //���ÿ��Ƶ��ƫ����
        Vector3 offset = perpendicular * arcModifier; //����ͨ���������ֵ���ı����ߵ���״

        Vector3 controlPoint = (cardPosition + mousePos) / 2 + offset; //���Ƶ�

        lineRenderer.positionCount = pointCount; //���� LineRenderer �ĵ������

        for (int i = 0; i < pointCount; i++)
        {
            float t = i / (float)(pointCount - 1);
            Vector3 point = CalculateQuadraticBezieePoint(t, cardPosition, controlPoint, mousePos);
            lineRenderer.SetPosition(i, point);
        }
    }

    //������α��������ߵ�
    private Vector3 CalculateQuadraticBezieePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0; //��һ��
        p += 2 * u * t * p1; //�ڶ���
        p += tt * p2;        //������

        return p;
    }
}
