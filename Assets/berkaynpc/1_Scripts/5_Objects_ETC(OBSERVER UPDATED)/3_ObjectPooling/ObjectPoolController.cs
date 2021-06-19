using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace project_usomk
{
    public class ObjectPoolController : MonoBehaviour
    {
        [Serializable]
        public class Pool
        {
            public string tag;     //Pool Object tag    //IDEA: Tag to enum
            public GameObject prefab;      //The object to be pooled.
            public int poolSize;
        }

        #region Singleton
        public static ObjectPoolController Instance;
        private void Awake()
        {
            Instance=this;
        }
        #endregion

        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;

        private void Start()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach(Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();         //Create a new queue

                GameObject poolHolder = new GameObject(pool.prefab.name + " pool");     //Create an object as pool holder.
                poolHolder.transform.parent = transform;        //Set pool holder as transform's child object.
                poolHolder.transform.position = transform.position;     //Set position

                IPooled ipooled = pool.prefab.GetComponent<IPooled>();      
                if (ipooled != null)
                {
                    for (int i = 0; i < pool.poolSize; i++)
                    {
                        GameObject obj = Instantiate(pool.prefab, this.transform);
                        obj.GetComponent<IPooled>().myTag = pool.tag;       //Set instantiated object's tag to pool tag
                        obj.SetActive(false);
                        objectPool.Enqueue(obj);                            //Enqueue object
                        obj.transform.SetParent(poolHolder.transform);      //Set instantiated object as poolHolder's child object.
                        obj.transform.position = poolHolder.transform.position;     //Set position
                    }
                }                  
                poolDictionary.Add(pool.tag, objectPool);       //Add objectPool queue to poolDictionary
            }
        }

        /// <summary>
        /// Pull the object that matches "tag" from the pool.
        /// </summary>
        public GameObject SpawnFromPool(string tag,Vector3 position,Quaternion rotation)
        {
            if(!poolDictionary.ContainsKey(tag))
            {
                return null;
            }

            GameObject objectToSpawn= poolDictionary[tag].Dequeue();        //Dequeue object

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;           
            return objectToSpawn;
        }

        /// <summary>
        /// Return the object that matches "tag" to the pool.
        /// </summary>
        public void ReturnToPool(string tag,GameObject gameObj)
        {
            if (poolDictionary.ContainsKey(tag))
            {
                StartCoroutine(ReturnToPoolCoroutine(tag,gameObj));
            }
        }

        IEnumerator ReturnToPoolCoroutine(string tag,GameObject gameObj)
        {
            gameObj.transform.position = new Vector3(0, 100, 0);
            yield return new WaitForSeconds(0.2f);
            poolDictionary[tag].Enqueue(gameObj);
            gameObj.SetActive(false);           
        }

        /// <summary>
        /// Find similar objects in pools then spawn it.
        /// </summary>
        public GameObject FindAndSpawn(GameObject gameObj, Vector3 position, Quaternion rotation) 
        {
            IPooled ipooled = gameObj.GetComponent<IPooled>();
            if (ipooled!=null)
            {
                string getTag = ipooled.myTag;      //Get tag from object using interface (IPooled)
                return SpawnFromPool(getTag, position, rotation);       //Spawn it
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Find similar objects in pools then return to target pool.
        /// </summary>
        public void FindAndReturnPool(GameObject gameObj)
        {
            IPooled ipooled = gameObj.GetComponent<IPooled>();
            if (ipooled != null)
            {
                string getTag = ipooled.myTag;      //Get tag from object using interface (IPooled)
                ReturnToPool(getTag,gameObj);       //Return it
            }
        }
    }
}