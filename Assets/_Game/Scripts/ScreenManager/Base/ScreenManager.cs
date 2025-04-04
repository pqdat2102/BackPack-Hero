using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GamePlugins
{
    public class ScreenManager : MonoBehaviour
    {
        public Canvas canvas;
        public Transform parent;
        public BaseScreen currentScreen;
        [SerializeField] string pathResScreenPrefabs;
        public BaseScreen[] prefabs;

        public Stack<BaseScreen> screenStacks = new Stack<BaseScreen>();
        public List<BaseScreen> cacheScreen = new List<BaseScreen>();
        private List<BaseScreen> ExistScreen = new List<BaseScreen>();

        private int defaultSortingOrder;

        private static ScreenManager mInstance;

        public static ScreenManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = LoadResource<ScreenManager>("Prefabs/ScreenManager");
                }
                return mInstance;
            }
        }

        public static ScreenManager CheckInstance
        {
            get
            {
                return mInstance;
            }
        }

        private void Awake()
        {
            if (mInstance == null)
            {
                mInstance = this;
                defaultSortingOrder = canvas.sortingOrder;
                getExixtScreen();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (this != Instance) Destroy(gameObject);
            }
        }

        private void getExixtScreen()
        {
            ExistScreen.Clear();
            int idxstartEnable = -1;
            bool isacmorethanone = false;
            for (int i = 0; i < parent.childCount; i++)
            {
                BaseScreen bs = parent.GetChild(i).GetComponent<BaseScreen>();
                if (bs != null)
                {
                    if (bs.gameObject.activeInHierarchy)
                    {
                        if (idxstartEnable == -1)
                        {
                            idxstartEnable = i;
                        }
                        else
                        {
                            isacmorethanone = true;
                        }
                    }
                    ExistScreen.Add(bs);
                }
            }
            if (isacmorethanone)
            {
                for (int i = (idxstartEnable + 1); i < parent.childCount; i++)
                {
                    parent.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        public T CheckExistScreen<T>() where T : BaseScreen
        {
            BaseScreen gameObject;
            for (int i = 0; i < ExistScreen.Count; i++)
            {
                if (IsOfType<T>(ExistScreen[i]))
                {
                    gameObject = ExistScreen[i];
                    ExistScreen.RemoveAt(i);
                    return gameObject as T;
                }
            }
            for (int i = 0; i < cacheScreen.Count; i++)
            {
                if (IsOfType<T>(cacheScreen[i]))
                {
                    gameObject = cacheScreen[i];
                    cacheScreen.RemoveAt(i);
                    return gameObject as T;
                }
            }
            return null;
        }

        public T CheckInstanceScreen<T>() where T : BaseScreen
        {
            GameObject gameObject = null;
            for (int i = 0; i < prefabs.Length; i++)
            {
                if (IsOfType<T>(prefabs[i]))
                {
                    gameObject = Instantiate(prefabs[i].gameObject, parent);
                    break;
                }
            }
            if (gameObject == null)
            {
                if (pathResScreenPrefabs != null && pathResScreenPrefabs.Length > 1)
                {
                    gameObject = (GameObject)Instantiate(Resources.Load(pathResScreenPrefabs + "\\" + typeof(T).ToString()), parent);
                }
            }
            if (gameObject != null)
            {
                gameObject.transform.localPosition = new Vector3(0, 0, 0);
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                RectTransform rct = gameObject.GetComponent<RectTransform>();
                if (rct != null)
                {
                    rct.anchorMin = new Vector2(0, 0);
                    rct.anchorMax = new Vector2(1, 1);
                    rct.anchoredPosition = new Vector2(0, 0);
                    rct.anchoredPosition3D = new Vector3(0, 0, 0);
                    rct.sizeDelta = new Vector2(0, 0);
                }
            }
            return gameObject.GetComponent<T>();
        }

        public bool Contains<T>()
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                if (IsOfType<T>(prefabs[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsOfType<T>(object value)
        {
            return value is T;
        }

        public bool CheckStack(Type type)
        {
            return screenStacks.Count > 0 && screenStacks.Peek().GetType() == type;
        }

        public static T LoadResource<T>(string name)
        {
            GameObject gameObject = (GameObject)Instantiate(Resources.Load(name));
            gameObject.name = $"[{name}]";
            DontDestroyOnLoad(gameObject);
            return gameObject.GetComponent<T>();
        }

        public void showPopup()
        {
            if (currentScreen != null)
            {
                currentScreen.onDisableControl();
            }
        }

        public void hidePopup()
        {
            if (currentScreen != null)
            {
                currentScreen.onEnableControl();
            }
        }

        public void SetSortingOrder(int order)
        {
            canvas.sortingOrder = order;
        }

        public void ResetOrder()
        {
            canvas.sortingOrder = defaultSortingOrder;
        }
    }
}
