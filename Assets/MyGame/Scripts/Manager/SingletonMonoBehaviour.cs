using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T instance;
    private bool isInited = false;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Object.FindObjectOfType<T>();

                if (instance == null)
                {
                    Debug.LogError($"No {typeof(T).Name} Singleton Instance.");
                }
            }

            return instance;
        }
    }

    public static bool HasInstance
    {
        get
        {
            return (instance != null);
        }
    }

    public bool IsInited { get => isInited; }

    protected virtual void Awake()
    {
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = (T)this;
            DontDestroyOnLoad(this);
            return true;
        }
        else if (instance == this)
        {
            DontDestroyOnLoad(this);
            return true;
        }

        Object.Destroy(this);
        return false;
    }

    public void Init(bool force = false)
    {
        if (this.isInited && !force)
            return;
        OnInited();
        this.isInited = false;
    }

    public virtual void OnInited()
    {

    }

}

