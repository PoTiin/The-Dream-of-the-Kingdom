using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("地图配置表")]
    public MapConfigSO mapConfig;
    [Header("地图布局")]
    public MapLayoutSO mapLayout;
    [Header("预制体")]
    public Room roomPrefab;
    public LineRenderer linePrefab;
    private float screenHeight;
    private float screenWidth;

    private float columnWidth;

    private Vector3 generatePoint;

    public float border;

    private List<Room> rooms = new();
    private List<LineRenderer> lines = new();

    public List<RoomDataSO> roomDataList = new();

    private Dictionary<RoomType, RoomDataSO> roomDataDict = new();

    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;
        columnWidth = screenWidth / (mapConfig.roomBlueprints.Count + 1);
        foreach (var item in roomDataList)
        {
            roomDataDict.Add(item.roomType, item);
        }
    }
    //private void Start()
    //{
    //    CreateMap();
    //}
    private void OnEnable()
    {
        if (mapLayout.mapRoomDataList.Count > 0) LoadMap();
        else CreateMap();
    }
    public void CreateMap()
    {
        //创建前一列房间列表
        List<Room> previousColumnRooms = new();
        for (int col = 0; col < mapConfig.roomBlueprints.Count; col++)
        {
            var blueprint = mapConfig.roomBlueprints[col];
            var amount = Random.Range(blueprint.min, blueprint.max);

            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);
            generatePoint = new Vector3(-screenWidth / 2 + border + columnWidth * col, startHeight, 0);

            var newPosition = generatePoint;

            var roomGapY = screenHeight / (amount + 1);

            //创建当期房间列表
            List<Room> currentColumnRooms = new();

            //循环当前列的所有房间数量生成房间
            for (int i = 0; i < amount; i++)
            {
                //判断为最后一列，Boss房间
                if (col == mapConfig.roomBlueprints.Count - 1)
                {
                    newPosition.x = screenWidth / 2 - border * 2;
                }else if (col != 0)
                {
                    newPosition.x = generatePoint.x + Random.Range(-border / 2, border / 2);
                }
                newPosition.y = startHeight - roomGapY * i;
                var room = Instantiate(roomPrefab, newPosition,Quaternion.identity,transform);
                RoomType newType = GetRandomRoomType(mapConfig.roomBlueprints[col].roomType);
                //设置只有第一列房间可以进入其他房间
                if(col == 0)
                {
                    room.roomState = RoomState.Attainable;
                }
                else
                {
                    room.roomState = RoomState.Locked;
                }

                room.SetupRoom(col, i, GetRoomData(newType));
                rooms.Add(room);
                currentColumnRooms.Add(room);
            }

            //判断当前列是否为第一列，如果不是则连接到上一列
            if (previousColumnRooms.Count > 0)
            {
                //创建两个列表的房间连线
                CreateConnections(previousColumnRooms, currentColumnRooms);
            }

            previousColumnRooms = currentColumnRooms;
        }

        SaveMap();
    }

    private void CreateConnections(List<Room> column1, List<Room> column2)
    {
        HashSet<Room> connectedColumn2Rooms = new();
        foreach (var room in column1)
        {
            var targetRoom = ConnectToRandomRoom(room, column2,false);
            connectedColumn2Rooms.Add(targetRoom);
        }
        //检查col2中所有房间都有连接
        foreach (var room in column2)
        {
            if (!connectedColumn2Rooms.Contains(room))
            {
                ConnectToRandomRoom(room, column1,true);
            }
        }
    }

    private Room ConnectToRandomRoom(Room room, List<Room> column2,bool check)
    {
        Room targetRoom = column2[Random.Range(0, column2.Count)];
        if (check)
        {
            targetRoom.lineTo.Add(new(room.column, room.line));
            //创建房间之间的连线
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, room.transform.position);
            line.SetPosition(1, targetRoom.transform.position);
            lines.Add(line);
        }
        else
        {
            room.lineTo.Add(new(targetRoom.column, targetRoom.line));
            //创建房间之间的连线
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(1, room.transform.position);
            line.SetPosition(0, targetRoom.transform.position);
            lines.Add(line);
        }
        
        return targetRoom;
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
        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }
        lines.Clear();
        CreateMap();
    }

    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDict[roomType];
    }

    private RoomType GetRandomRoomType(RoomType flags)
    {
        string[] options = flags.ToString().Split(',');
        string randomOption = options[Random.Range(0, options.Length)];

        RoomType roomType = (RoomType)System.Enum.Parse(typeof(RoomType) ,randomOption);

        return roomType;
    }

    private void SaveMap()
    {
        mapLayout.mapRoomDataList = new();

        //添加所有已经生成的房间
        for (int i = 0; i < rooms.Count; i++)
        {
            var room = new MapRoomData()
            {
                posX = rooms[i].transform.position.x,
                posY = rooms[i].transform.position.y,
                colum = rooms[i].column,
                line = rooms[i].line,
                roomData = rooms[i].roomData,
                roomState = rooms[i].roomState,
                linkTo = rooms[i].lineTo
            };

            mapLayout.mapRoomDataList.Add(room);
            
        }
        mapLayout.linePositionList = new();
        //添加所有连线
        for (int i = 0; i < lines.Count; i++)
        {
            var line = new LinePosition()
            {
                startPos = lines[i].GetPosition(0),
                endPos = lines[i].GetPosition(1)
            };

            mapLayout.linePositionList.Add(line);

        }
    }

    private void LoadMap()
    {
        //读取房间数据生成房间
        for (int i = 0; i < mapLayout.mapRoomDataList.Count; i++)
        {
            var newPos = new Vector3(mapLayout.mapRoomDataList[i].posX, mapLayout.mapRoomDataList[i].posY, 0);
            var newRoom = Instantiate(roomPrefab, newPos, Quaternion.identity, transform);
            newRoom.roomState = mapLayout.mapRoomDataList[i].roomState;
            newRoom.SetupRoom(mapLayout.mapRoomDataList[i].colum, mapLayout.mapRoomDataList[i].line, mapLayout.mapRoomDataList[i].roomData);
            newRoom.lineTo = mapLayout.mapRoomDataList[i].linkTo;
            rooms.Add(newRoom);
        }
        //读取连线
        for (int i = 0; i < mapLayout.linePositionList.Count; i++)
        {
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, mapLayout.linePositionList[i].startPos);
            line.SetPosition(1, mapLayout.linePositionList[i].endPos);
            lines.Add(line);
        }
    }
}
