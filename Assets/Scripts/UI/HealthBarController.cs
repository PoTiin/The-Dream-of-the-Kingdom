using NUnit.Framework.Internal;
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

    private void Awake()
    {
        currentCharacter = GetComponent<CharacterBase>();
        


    }
    private void Start()
    {
        InitHealthBar();
    }

    private void MoveToWorldPosition(VisualElement element,Vector3 worldPosition,Vector2 size)
    {
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel,worldPosition,size,Camera.main);
        element.transform.position = rect.position;
        //element.style.translate = rect.position;
    }
    [ContextMenu("Get UI Position")]
    public void InitHealthBar()
    {
        healthBarDocument = GetComponent<UIDocument>();
        healthBar = healthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");
        healthBar.highValue = currentCharacter.MaxHP;
        MoveToWorldPosition(healthBar, healthBarTransform.position, Vector2.zero);

        defenseElement = healthBar.Q<VisualElement>("Defense");
        defenseAmountLabel = defenseElement.Q<Label>("DefenseAmount");

        defenseElement.style.display = DisplayStyle.None;
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
            healthBar.title = $"{currentCharacter.CurrentHP}/{currentCharacter.MaxHP}";
            healthBar.value = currentCharacter.CurrentHP;

            healthBar.RemoveFromClassList("highHealth");
            healthBar.RemoveFromClassList("mediumHealth");
            healthBar.RemoveFromClassList("lowHealth");

            var percentage = (float)currentCharacter.CurrentHP / (float)currentCharacter.MaxHP;
            if(percentage < 0.3f)
            {
                healthBar.AddToClassList("lowHealth");
            }else if (percentage < 0.6f)
            {
                healthBar.AddToClassList("mediumHealth");
            }else
            {
                healthBar.AddToClassList("highHealth");
            }
            //·ÀÓùÊôÐÔ¸üÐÂ
            defenseElement.style.display = currentCharacter.defense.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            defenseAmountLabel.text = currentCharacter.defense.currentValue.ToString();
        }
    }
}
