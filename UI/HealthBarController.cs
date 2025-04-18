using NUnit.Framework.Internal;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    private CharacterBase currentCharacter;

    [Header("Elements")]
    public Transform healthBarTransform;
    private UIDocument healthBarDocument;
    private ProgressBar healthBar;

    private VisualElement defenseElement;
    private Label defenseAmountLabel;

    private VisualElement buffElement;
    private Label buffRound;

    [Header("BuffͼƬ")]
    public Sprite buff;
    public Sprite debuff;

    private Enemy enemy;
    private VisualElement intentSprite;
    private Label intentAmount;

    private void Awake()
    {
        currentCharacter = GetComponent<CharacterBase>();
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        InitHealthBar();
    }

    private void MoveToWorldPosition(VisualElement element, Vector3 worldPosition, Vector2 size)
    {
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPosition, size, Camera.main);
        element.transform.position = rect.position;
    }

    [ContextMenu("����λ��")]
    public void InitHealthBar()
    {
        healthBarDocument = GetComponent<UIDocument>();
        healthBar = healthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");

        healthBar.highValue = currentCharacter.MaxHp;
        MoveToWorldPosition(healthBar, healthBarTransform.position, Vector2.zero);


        defenseElement = healthBar.Q<VisualElement>("Defense");
        defenseAmountLabel = defenseElement.Q<Label>("DefenseAmount");
        defenseElement.style.display = DisplayStyle.None;

        buffElement = healthBar.Q<VisualElement>("Buff");
        buffRound = buffElement.Q<Label>("BuffRound");
        buffElement.style.display = DisplayStyle.None;

        intentSprite = healthBar.Q<VisualElement>("intent");
        intentAmount = healthBar.Q<Label>("intentAmount");
        intentSprite.style.display = DisplayStyle.None;
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (currentCharacter.isDead)
        {
            healthBar.style.display = DisplayStyle.None;
            return;
        }

        if (healthBar != null)
        {
            healthBar.title = $"{currentCharacter.CurrentHP}/{currentCharacter.MaxHp}";

            healthBar.value = currentCharacter.CurrentHP;

            healthBar.RemoveFromClassList("highHealh");
            healthBar.RemoveFromClassList("mediumHealh");
            healthBar.RemoveFromClassList("lowHealh");

            var percentage = (float)currentCharacter.CurrentHP / (float)currentCharacter.MaxHp;

            if (percentage < 0.3f)
            {
                healthBar.AddToClassList("lowHealh");
            }
            else if (percentage < 0.6f)
            {
                healthBar.AddToClassList("mediumHealh");
            }
            else
            {
                healthBar.AddToClassList("highHealh");
            }

            //�������Ը���
            defenseElement.style.display = currentCharacter.defense.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            defenseAmountLabel.text = currentCharacter.defense.currentValue.ToString();

            //buff�غϸ���
            buffElement.style.display = currentCharacter.buffRound.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            buffRound.text = currentCharacter.buffRound.currentValue.ToString();
            buffElement.style.backgroundImage = currentCharacter.baseStrength > 1f ? new StyleBackground(buff) : new StyleBackground(debuff);
        }
    }


    /// <summary>
    /// ����һغϿ�ʼʱ���� �¼�����
    /// </summary>
    public void SetIntentElement()
    {
        intentSprite.style.display = DisplayStyle.Flex;

        intentSprite.style.backgroundImage = new StyleBackground(enemy.currentAction.intentSprite);

        //�ж��Ƿ��ǹ���
        var value = enemy.currentAction.effect.value;
        if (enemy.currentAction.effect.GetType() == typeof(DamageEffect))
        {
            value = (int)math.round(enemy.currentAction.effect.value * enemy.baseStrength);
        }

        intentAmount.text = value.ToString();
    }

    /// <summary>
    /// �¼����� ���˻غϽ���֮��������Ϊ��
    /// </summary>
    public void HideIntentElement()
    {
        intentSprite.style.display = DisplayStyle.None;
    }
}
