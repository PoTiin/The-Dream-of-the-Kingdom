using UnityEngine;

public class Room : MonoBehaviour
{
    public int column;
    public int line;
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState roomState;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        SetupRoom(0, 0, roomData);
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        Debug.Log($"点击到了: {hit.collider.gameObject.name}");
        //        Debug.Log($"点击位置: {hit.point}");
        //    }
        //    else
        //    {
        //        Debug.Log("什么都没点到");
        //    }
        //}
    }

    void OnMouseDown()
    {
        Debug.Log($"OnMouseDown被调用: {gameObject.name}");
    }
    /// <summary>
    /// 外部创建房间时调用配置房间
    /// </summary>
    /// <param name="column"></param>
    /// <param name="line"></param>
    /// <param name="roomData"></param>
    public void SetupRoom(int column,int line,RoomDataSO roomData)
    {
        this.column = column;
        this.line = line;
        this.roomData = roomData;
        spriteRenderer.sprite = roomData.roomIcon;
    }
}
