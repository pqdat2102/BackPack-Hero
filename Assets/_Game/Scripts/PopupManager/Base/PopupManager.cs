using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlugins
{
    public class PopupManager : MonoBehaviour
    {
        public Camera popupCamera;

        public Canvas canvas;
        public Transform parent;
        public GameObject transparent;
        //public bool isCache = false;

        public bool usingDefaultTransparent = true;
        [SerializeField] string pathResPopupPrefabs;
        public bool isShowOverPop = false;
        public BasePopup[] prefabs;
        private Transform mTransparentTrans;
        public Stack<BasePopup> popupStacks = new Stack<BasePopup>();
        public Stack<BasePopup> popupQueueShow = new Stack<BasePopup>();
        public List<BasePopup> cachePopup = new List<BasePopup>();
        [SerializeField] private Transform waitingPopup;

        private int defaultSortingOrder;
        private static PopupManager mInstance;

        public static PopupManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = LoadResource<PopupManager>("Popup/PopupManager");
                }
                return mInstance;
            }
        }

        public static PopupManager CheckInstance
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
                mTransparentTrans = transparent.transform;
                defaultSortingOrder = canvas.sortingOrder;
                // popupCamera.gameObject.SetActive(false);
                popupCamera.orthographicSize = Camera.main.orthographicSize;
                getExixtPopup();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (this != mInstance)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void getExixtPopup()
        {
            cachePopup.Clear();
            for (int i = 0; i < parent.childCount; i++)
            {
                BasePopup bs = parent.GetChild(i).GetComponent<BasePopup>();
                if (bs != null)
                {
                    bs.gameObject.SetActive(false);
                    cachePopup.Add(bs);
                }
            }
        }

        public static T CreateNewInstance<T>() where T : BasePopup
        {
            return Instance.CheckInstancePopupPrebab<T>();
        }

        public T showNewPopup<T>(Action showCompletedCallback = null, Action hideCompletedCallback = null, bool overlay = true, bool isRenderCamera = false) where T : BasePopup
        {
            T ob = CheckInstancePopupPrebab<T>();
            if (ob != null)
            {
                ob.Show(showCompletedCallback, hideCompletedCallback, overlay, isRenderCamera);
            }
            return ob;
        }

        public T showPopup<T>(Action showCompletedCallback = null, Action hideCompletedCallback = null, bool overlay = true, bool isRenderCamera = false) where T : BasePopup
        {
            T gameObject = null;
            foreach (var item in popupStacks)
            {
                if (IsOfType<T>(item))
                {
                    gameObject = item as T;
                    break;
                }
            }
            if (gameObject == null)
            {
                return showNewPopup<T>();
            }
            else
            {
                gameObject.Show(showCompletedCallback, hideCompletedCallback, overlay, isRenderCamera);
                return gameObject;
            }
        }
        public T hideNewPopup<T>() where T : BasePopup
        {
            T ob = CheckInstancePopupPrebab<T>();
            if (ob != null)
            {
                ob.Hide();
            }
            return ob;
        }
        public T hidePopup<T>() where T : BasePopup
        {
            T gameObject = null;
            foreach (var item in popupStacks)
            {
                if (IsOfType<T>(item))
                {
                    gameObject = item as T;
                    break;
                }
            }
            if (gameObject == null)
            {
                return hideNewPopup<T>();
            }
            else
            {
                gameObject.Hide();
                return gameObject;
            }
        }

        public void onShowPopup(BasePopup popup)
        {
            ChangeTransparentOrder(popup.mTransform, true);
        }

        public void onHidePopup(BasePopup popup)
        {
            ChangeTransparentOrder(popup.mTransform, false);
        }

        public T CheckInstancePopupPrebab<T>() where T : BasePopup
        {
            GameObject gameObject = null;
            for (int i = 0; i < cachePopup.Count; i++)
            {
                if (IsOfType<T>(cachePopup[i]))
                {
                    gameObject = cachePopup[i].gameObject;
                    break;
                }
            }
            if (gameObject == null)
            {
                for (int i = 0; i < prefabs.Length; i++)
                {
                    if (IsOfType<T>(prefabs[i]))
                    {
                        gameObject = Instantiate(prefabs[i].gameObject, parent);
                        break;
                    }
                }
            }
            if (gameObject == null)
            {
                if (pathResPopupPrefabs != null && pathResPopupPrefabs.Length > 1)
                {
                    string pathre = pathResPopupPrefabs + "\\" + typeof(T).ToString();
                    gameObject = (GameObject)Instantiate(Resources.Load(pathre), parent);
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


        private void ChangeTransparentOrder(Transform topPopupTransform, bool active)
        {
            if (active)
            {
                mTransparentTrans.SetSiblingIndex(topPopupTransform.GetSiblingIndex() - 1);
                transparent.SetActive(true && usingDefaultTransparent);
            }
            else if (popupStacks.Count > 1)
            {
                mTransparentTrans.SetSiblingIndex(topPopupTransform.GetSiblingIndex() - 2);
            }
            else
            {
                transparent.SetActive(value: false);
            }
        }

        public void ForceHideAll()
        {
            while (popupStacks.Count > 0)
            {
                BasePopup basePopup = popupStacks.Pop();
                basePopup.Hide();
            }
        }

        public bool SequenceHidePopup(Action cb = null)
        {
            if (popupStacks.Count > 0)
            {
                popupStacks.Peek().Hide(cb);
            }
            else
            {
                transparent.SetActive(value: false);
            }
            return popupStacks.Count > 0;
        }

        public bool CheckStack(Type type)
        {
            return popupStacks.Count > 0 && popupStacks.Peek().GetType() == type;
        }

        private static T LoadResource<T>(string name)
        {
            GameObject gameObject = (GameObject)Instantiate(Resources.Load(name));
            gameObject.name = $"[{name}]";
            DontDestroyOnLoad(gameObject);
            return gameObject.GetComponent<T>();
        }

        public void SetSortingOrder(int order)
        {
            canvas.sortingOrder = order;
        }

        public void ResetOrder()
        {
            canvas.sortingOrder = defaultSortingOrder;
        }

        public static void ActiveCamera()
        {
            Instance.popupCamera.gameObject.SetActive(true);
            Instance.canvas.worldCamera = Instance.popupCamera;
        }

        public static void DisableCamera()
        {
            Instance.popupCamera.gameObject.SetActive(false);
            Instance.canvas.worldCamera = null;
        }
        public void ShowWaiting(bool isEnable) 
        {
            waitingPopup.gameObject.SetActive(isEnable);
        }
    }
}
