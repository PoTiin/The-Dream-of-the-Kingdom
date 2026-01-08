using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;
    public AssetReference map;

    /// <summary>
    /// 在房间加载事件中监听
    /// </summary>
    /// <param name="data"></param>
    public async void OnLoadRoomEvent(object data)
    {
        if(data is RoomDataSO)
        {
            RoomDataSO currentRoom = (RoomDataSO)data;
            //Debug.Log(currentRoom.roomType);
            currentScene = currentRoom.sceneToLoad;
        }
        await UnloadSceneTask();
        await LoadSceneTask();
    }
    /// <summary>
    /// 异步操作加载场景
    /// </summary>
    /// <returns></returns>
    private async Awaitable LoadSceneTask()
    {
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);

        await s.Task;
        if(s.Status == AsyncOperationStatus.Succeeded)
        {
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    private async Awaitable UnloadSceneTask()
    {
        await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
    /// <summary>
    /// 监听返回房间的事件函数
    /// </summary>
    public async void LoadMap()
    {
        await UnloadSceneTask();
        currentScene = map;
        await LoadSceneTask();
    }
}
