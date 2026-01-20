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
        }
    }
}
