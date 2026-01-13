using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout;

    public void UpdateMapLayoutData(object value)
    {
        Vector2Int roomVector = (Vector2Int)value;
        var currentRoom = mapLayout.mapRoomDataList.Find(r => r.colum == roomVector.x && r.line == roomVector.y);
        currentRoom.roomState = RoomState.Visited;

        //更新相邻房间的数据
        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(r => r.colum == currentRoom.colum);
        foreach (var room in sameColumnRooms)
        {
            if(room.line != roomVector.y)
                room.roomState = RoomState.Locked;
        }

        foreach (var link in currentRoom.linkTo)
        {
            var linkRoom = mapLayout.mapRoomDataList.Find(r => r.colum == link.x && r.line == link.y);
            linkRoom.roomState = RoomState.Attainable;
        }
    }
}
