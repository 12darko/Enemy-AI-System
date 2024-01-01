using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool
{
    private PoolableObject _prefab;
    private int _size;
    private List<PoolableObject> _availableObjectsPool;

    private ObjectPool(PoolableObject Prefab, int Size)
    {
        this._prefab = Prefab;
        this._size = Size;
        _availableObjectsPool = new List<PoolableObject>(Size);
    }

    public static ObjectPool CreateInstance(PoolableObject Prefab, int Size , Vector3 pos)
    {
        ObjectPool pool = new ObjectPool(Prefab, Size);

        GameObject poolGameObject = new GameObject(Prefab + " Pool");
        pool.CreateObjects(poolGameObject ,pos);

        return pool;
    }

    private void CreateObjects(GameObject parent, Vector3 pos)
    {
        for (int i = 0; i < _size; i++)
        {
            PoolableObject poolableObject = GameObject.Instantiate(_prefab, pos, Quaternion.identity, parent.transform);
            poolableObject.Parent = this;
            poolableObject.gameObject.SetActive(false); // PoolableObject handles re-adding the object to the AvailableObjects
        }
    }

    public PoolableObject GetObject()
    {
        PoolableObject instance = _availableObjectsPool[0];

        _availableObjectsPool.RemoveAt(0);

        instance.gameObject.SetActive(true);

        return instance;
    }

    public void ReturnObjectToPool(PoolableObject Object)
    {
        _availableObjectsPool.Add(Object);
    }
}