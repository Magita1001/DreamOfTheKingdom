using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public FadePanel FadePanel;

    //当前加载的场景 即将加载的场景
    private AssetReference currentScene;
    //提前加载地图
    public AssetReference map;
    public AssetReference menu;
    public AssetReference intro;

    private Room currentRoom;


    private Vector2Int currentRoomVector;

    public ObjectEventSO afterRoomLoadedEvent;
    public ObjectEventSO UpdateRoom;

    private void Awake()
    {
        currentRoomVector = Vector2Int.one * -1;

        LoadIntro();
        //LoadMenu();
    }

    /// <summary>
    /// 在房间事件中加载监听 data由room中的广播添加
    /// </summary>
    /// <param name="data"></param>
    public async void OnLoadRoomEvent(object data)
    {
        if(data is Room)
        {
            currentRoom = (Room)data;

            //将待加载的场景赋值为RoomDataSO中的sceneToLoad
            var currentData = currentRoom.roomData;
            //获得room的Vector
            currentRoomVector = new(currentRoom.column, currentRoom.line);
            
            //print(currentData.roomType);
            currentScene = currentData.sceneToLoad;
        }

        //先卸载场景
        await UnLoadSceneTask();
        //加载房间
        await LoadSceneTask();

        afterRoomLoadedEvent.RaisEvent(currentRoom, this);
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <returns></returns>
    private async Awaitable LoadSceneTask()
    {
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        await s.Task;
        if (s.Status == AsyncOperationStatus.Succeeded)
        {
            FadePanel.FadOut(0.2f);
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    private async Awaitable UnLoadSceneTask()
    {
        FadePanel.FadIn(0.4f);
        await Awaitable.WaitForSecondsAsync(0.45f);
        await Awaitable.FromAsyncOperation(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));
    }

    /// <summary>
    /// 监听事件返回的事件函数
    /// </summary>
    public async void LoadMap()
    {
        await UnLoadSceneTask();

        if (currentRoomVector != Vector2.one * -1)
        {
            UpdateRoom.RaisEvent(currentRoomVector, this);
        }

        currentScene = map;
        await LoadSceneTask();
    }

    public async void LoadMenu()
    {
        if (currentScene != null)
        {
            await UnLoadSceneTask();
        }
        currentScene = menu;
        await LoadSceneTask();
    }

    public async void LoadIntro()
    {
        if (currentScene != null)
        {
            await UnLoadSceneTask();
        }
        currentScene = intro;
        await LoadSceneTask();
    }
}
