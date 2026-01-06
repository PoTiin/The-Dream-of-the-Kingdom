using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public MapConfigSO mapConfig;
    public Room roomPrefab;
    private float screenHeight;
    private float screenWidth;

    private float columnWidth;

    private Vector3 generatePoint;

    public float border;

    private List<Room> rooms = new();

    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;
        columnWidth = screenWidth / (mapConfig.roomBlueprints.Count + 1);
    }
    private void Start()
    {
        CreateMap();
    }

    public void CreateMap()
    {
        for (int col = 0; col < mapConfig.roomBlueprints.Count; col++)
        {
            var blueprint = mapConfig.roomBlueprints[col];
            var amount = Random.Range(blueprint.min, blueprint.max);

            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);
            generatePoint = new Vector3(-screenWidth / 2 + border + columnWidth * col, startHeight, 0);

            var newPosition = generatePoint;

            

            var roomGapY = screenHeight / (amount + 1);
            //循环当前列的所有房间数量生成房间
            for (int i = 0; i < amount; i++)
            {
                if (col == mapConfig.roomBlueprints.Count - 1)
                {
                    newPosition.x = screenWidth / 2 - border * 2;
                }else if (col != 0)
                {
                    newPosition.x = generatePoint.x + Random.Range(-border / 2, border / 2);
                }
                newPosition.y = startHeight - roomGapY * i;
                var room = Instantiate(roomPrefab, newPosition,Quaternion.identity,transform);
                rooms.Add(room);
            }
        }
    }

    //重新生成地图
    [ContextMenu(itemName: "ReGenerateRoom")]
    public void ReGenerateRoom()
    {
        foreach (var room in rooms)
        {
            Destroy(room.gameObject);
        }
        rooms.Clear();
        CreateMap();
    }
}
