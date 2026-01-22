using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class CardDragHandler : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public GameObject arrowPrefab;
    private GameObject currentArrow;
    private Card currentCard;
    private bool canMove;

    private bool canExecute;

    private CharacterBase targetCharacter;

    private void Awake()
    {
        currentCard = GetComponent<Card>();
    }
    private void OnDisable()
    {
        canMove = false;
        canExecute = false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        switch (currentCard.cardData.cardType)
        {
            case CardType.Attack:
                currentArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                break;
            case CardType.Defense:
            case CardType.Abilities:
                canMove = true;
                break;
            default:
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canMove)
        {
            currentCard.isAnimating = true;
            Vector3 screenPos = new(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            currentCard.transform.position = worldPos;
            canExecute = worldPos.y > 1f;
        }
        else
        {
            if (eventData.pointerEnter == null) return;

            if (eventData.pointerEnter.CompareTag("Enemy"))
            {
                canExecute = true;
                targetCharacter = eventData.pointerEnter.GetComponent<CharacterBase>();
                return;
            }
            canExecute = false;
            targetCharacter = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentArrow)
        {
            Destroy(currentArrow);
        }
        if (canExecute)
        {
            currentCard.ExecuteCardEffects(currentCard.player, targetCharacter);
        }
        else
        {
            currentCard.ResetCardTransform();
            currentCard.isAnimating = false;
        }
        
    }
}
