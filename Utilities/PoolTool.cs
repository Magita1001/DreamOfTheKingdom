using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[DefaultExecutionOrder(-100)]//代码的执行顺序 值越小越靠前
public class PoolTool : MonoBehaviour
{
    public GameObject objPerfab;
    private ObjectPool<GameObject> pool;
    private void Awake()
    {        
        //初始化对象池
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(objPerfab, this.transform),  //被调用时 可用对象不足时
            actionOnGet: (obj) => obj.SetActive(true), //获得方法、获得时
            actionOnRelease: (obj) => obj.SetActive(false),//回收时、释放时
            actionOnDestroy: (obj) => Destroy(obj), //销毁时
            collectionCheck: false,
            defaultCapacity: 10, //默认大小
            maxSize: 30 //最大数量
            );

        PreFillPool(7);
    }

    private void PreFillPool(int count)
    {
        var preFillArray = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            preFillArray[i] = pool.Get();
        }

        foreach (var item in preFillArray)
        {
            pool.Release(item);
        }
    }

    public GameObject GetObjectFromPool()
    {
        return pool.Get();
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        pool.Release(obj);
    } 
}

public class poolTool:MonoBehaviour
{
    public GameObject objPerfab;
    public List<GameObject> pool = new List<GameObject>();
    int maxSize;

    public poolTool(int createSize, int maxSize)
    {
        this.maxSize = maxSize;
        PreFillPool(createSize);
    }

    /// <summary>
    /// 预先填充指定数量的对象到对象池中
    /// </summary>
    /// <param name="createSize"></param>
    public void PreFillPool(int createSize)
    {
        if (pool.Count == 0 || (pool.Count + createSize) <= maxSize)
        {
            for (int i = 0; i < createSize; i++)
            {
                var obj = Instantiate(objPerfab, this.transform);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }
        else
        {
            Debug.Log("PreFillPool时对象池溢出");
        }
    }

    /// <summary>
    /// 从对象池中获得对象
    /// </summary>
    /// <returns></returns>
    public GameObject Get()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        if (pool.Count < maxSize)
        {
            var obj = Instantiate(objPerfab, this.transform);
            obj.SetActive(true);
            pool.Add(objPerfab);
            return obj;
        }    
        
        Debug.Log("对象池已满");
        return null;
    }

    /// <summary>
    /// 归还对象到池中
    /// </summary>
    /// <param name="obj">要归还的对象</param>
    public void Release(GameObject obj)
    {
        if (pool.Contains(obj))
        {
            obj.SetActive(false);
        }
        else
        {
            Debug.Log("对象不在池中");
        }        
    }

}
