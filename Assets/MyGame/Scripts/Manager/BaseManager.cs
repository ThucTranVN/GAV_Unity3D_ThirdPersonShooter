using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
///
public class BaseManager<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool AlreadyCreated = false;
    public bool IsCanDestroy = false;

    private bool isInited = false;

    private static T _instance;
    private static object _lock = new object();
    private static bool applicationIsQuitting = false;
    public bool IsInited { get => isInited; }

    public static bool HasInstance
    {
        get
        {
            return (Instance != null);
        }
    }

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                //					Debug.LogWarning ("[Singleton] Instance '" + typeof(T) +
                //					"' already destroyed on application quit." +
                //					" Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        //							Debug.LogError ("[Singleton] Something went really wrong " +
                        //							" - there should never be more than 1 singleton!" +
                        //							" Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
#if UNITY_EDITOR
                        singleton.name = typeof(T).ToString();
#endif
                        DontDestroyOnLoad(singleton);

                        //							Debug.Log ("[Singleton] An instance of " + typeof(T) +
                        //							" is needed in the scene, so '" + singleton +
                        //							"' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        //Debug.Log("[Singleton] Using instance already created: " +
                        //_instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
    }

    public static bool ApplicationIsQuitting { get => applicationIsQuitting; }

    public virtual void Awake()
    {
        if (AlreadyCreated)
        {
            if (FindObjectsOfType(typeof(T)).Length > 1)
            {
                Debug.LogWarning("[Singleton] Instance already created: " +
                _instance.gameObject.name);
                DestroyImmediate(this.gameObject);
                return;
            }

            _instance = (T)FindObjectOfType(typeof(T));
#if UNITY_EDITOR
            this.gameObject.name = typeof(T).ToString();
#endif
            if (this.transform.parent != null)
            {
                this.transform.parent = null;
            }
            Init(this.IsCanDestroy, true);
        }
    }

    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public virtual void OnDestroy()
    {
        _instance = null;
        //applicationIsQuitting = true;
    }

    public virtual void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    public void Init(bool canDestroy = false, bool forceInit = false)
    {
        if (isInited && !forceInit)
            return;
        this.IsCanDestroy = canDestroy;
        OnInited();
        if (this.IsCanDestroy)
        {
            RemoveDontDestroy();
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
        isInited = true;
    }

    /// <summary>
    ///  Init only 1 time
    /// </summary>
    public virtual void OnInited()
    {

    }

    public void RemoveDontDestroy()
    {
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
    }

}