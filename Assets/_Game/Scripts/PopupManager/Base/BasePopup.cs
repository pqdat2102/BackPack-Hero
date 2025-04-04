using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlugins
{
    [RequireComponent(typeof(Animation))]
    public class BasePopup : MonoBehaviour
    {
        [HideInInspector]
        public Animation popAnimation;

        public AnimationClip showAnimationClip;

        public AnimationClip hideAnimationClip;
        public bool isCache = false;

        protected bool isShowed;

        private int mSortOrder;

        public bool isQueueShow { get; private set; }

        public Transform mTransform { get; private set; }

        private Stack<BasePopup> refStacks;

        private Action hideCompletedCallback;

        private Action showCompletedCallback;
        private bool isCallOncreate = false;

        public virtual void onCreatePopup()
        {

        }

        public virtual void onShowPopup()
        {

        }

        public virtual void onHidePopup()
        {

        }

        public virtual void onDestroyPopup()
        {

        }

        public virtual void Awake()
        {
            popAnimation = GetComponent<Animation>();
            mTransform = base.transform;
            mSortOrder = mTransform.GetSiblingIndex();
            refStacks = PopupManager.Instance.popupStacks;
            if (popAnimation != null && showAnimationClip != null && hideAnimationClip != null)
            {
                popAnimation.AddClip(showAnimationClip, showAnimationClip.name);
                popAnimation.AddClip(hideAnimationClip, hideAnimationClip.name);
            }
            else
            {
                // BPDebug.LogMessage("Chưa gán Animator hoặc showAnimationClip, hideAnimationClip  cho popup " + GetType().ToString(), error: true);
            }
            isQueueShow = false;
            callOnCreate();
        }

        private void callOnCreate()
        {
            if (!isCallOncreate)
            {
                isCallOncreate = true;
                onCreatePopup();
            }
        }

        public void Show(Action showCompletedCallback = null, Action hideCompletedCallback = null, bool overlay = true, bool isRenderCamera = false)
        {
            if (refStacks == null)
            {
                popAnimation = GetComponent<Animation>();
                mTransform = base.transform;
                mSortOrder = mTransform.GetSiblingIndex();
                refStacks = PopupManager.Instance.popupStacks;
            }
            if (ScreenManager.CheckInstance && refStacks.Count == 0)
            {
                ScreenManager.CheckInstance.showPopup();
            }
            // if (isRenderCamera) PopupManager.ActiveCamera();
            // else PopupManager.DisableCamera();
            callOnCreate();
            isQueueShow = false;
            if (isShowed)
            {
                Reshow();
                int num = refStacks.Peek().SortOrder();
                if (refStacks.Count > 1 && SortOrder() != num)
                {
                    MoveElementToTopStack(ref refStacks, SortOrder());
                }
                return;
            }
            this.showCompletedCallback = showCompletedCallback;
            this.hideCompletedCallback = hideCompletedCallback;
            float waitTime = 0f;
            isShowed = true;
            if (!overlay && refStacks.Count > 0)
            {
                ForceHideAllCurrent(ref waitTime);
            }
            if (!PopupManager.Instance.isShowOverPop && refStacks.Count > 0)
            {
                refStacks.Peek().gameObject.SetActive(false);
            }
            if (!refStacks.Contains(this))
            {
                checkListCache();
                refStacks.Push(this);
            }
            if (refStacks.Count > 0)
            {
                ChangeSortOrder(refStacks.Peek().SortOrder() + 1);
            }
            gameObject.SetActive(true);
            if (waitTime != 0f)
            {
                //AdsProcessCB.Instance().Enqueue(() =>
                //{
                    AnimateShow();
                //}, waitTime);
                //Invoke("AnimateShow", waitTime);
            }
            else
            {
                AnimateShow();
            }
            onShowPopup();
        }

        private void Reshow()
        {
            if (popAnimation != null && showAnimationClip != null)
            {
                popAnimation.Play(showAnimationClip.name);
                float animationClipDuration = GetAnimationClipDuration(showAnimationClip);
                //AdsProcessCB.Instance().Enqueue(() =>
                //{
                    OnShowFinish();
                //}, animationClipDuration);
                // Invoke("OnShowFinish", animationClipDuration);
            }
            PopupManager.Instance.onShowPopup(this);
        }

        private void AnimateShow()
        {
            gameObject.SetActive(true);
            if (popAnimation != null && showAnimationClip != null)
            {
                float animationClipDuration = GetAnimationClipDuration(showAnimationClip);
                //AdsProcessCB.Instance().Enqueue(() =>
                //{
                    OnShowFinish();
                //}, animationClipDuration + .1f);
                // Invoke("OnShowFinish", animationClipDuration + .1f);
                popAnimation.Play(showAnimationClip.name);
            }
            else
            {
                OnShowFinish();
            }
            PopupManager.Instance.onShowPopup(this);
        }

        public virtual void OnShowFinish()
        {
            if (showCompletedCallback != null)
            {
                showCompletedCallback();
            }
        }

        public void Hide(Action hideCompletedCallback = null, bool isRenderCamera = false)
        {
            this.hideCompletedCallback = hideCompletedCallback;
            // if (isRenderCamera) PopupManager.ActiveCamera();
            // else PopupManager.DisableCamera();
            if (isShowed)
            {
                isShowed = false;
                AnimateHide();
            }
            onHidePopup();
        }

        public virtual void OnCloseClick()
        {
            Hide();
        }

        private void AnimateHide()
        {
            PopupManager.Instance.onHidePopup(this);
            if (popAnimation != null && hideAnimationClip != null)
            {
                popAnimation.Play(hideAnimationClip.name);
                float animationClipDuration = GetAnimationClipDuration(hideAnimationClip);
                if (Time.timeScale != 0)
                {
                    //AdsProcessCB.Instance().Enqueue(() =>
                    //{
                        DoHide();
                    //}, animationClipDuration);
                    // Invoke("DoHide", animationClipDuration);
                }
                else
                {
                    DoHide();
                }
            }
            else
            {
                DoHide();
            }
        }

        private void DoHide()
        {
            onDestroyPopup();
            if (refStacks.Contains(this))
            {
                List<BasePopup> ltmp = new List<BasePopup>();
                while (refStacks.Count > 0)
                {
                    BasePopup ob = refStacks.Pop();
                    if (ob.Equals(this))
                    {
                        break;
                    }
                    else
                    {
                        ltmp.Add(ob);
                    }
                }
                for (int i = (ltmp.Count - 1); i >= 0; i--)
                {
                    refStacks.Push(ltmp[i]);
                }
                ltmp.Clear();
            }

            if (this.isCache)
            {
                base.gameObject.SetActive(false);
                if (!PopupManager.Instance.cachePopup.Contains(this))
                {
                    PopupManager.Instance.cachePopup.Add(this);
                }
                else
                {

                }
            }
            else
            {
                if (gameObject.activeSelf)
                {
                    Destroy(base.gameObject);
                }
            }

            hideCompletedCallback?.Invoke();
            PopupManager.Instance.ResetOrder();


            if (refStacks.Count == 0)
            {
                if (ScreenManager.CheckInstance)
                {
                    ScreenManager.CheckInstance.hidePopup();
                }
            }
            else
            {
                refStacks.Peek().gameObject.SetActive(true);
                ChangeSortOrder(refStacks.Peek().SortOrder() + 1);
                PopupManager.Instance.onShowPopup(refStacks.Peek());
            }
        }

        public int SortOrder()
        {
            return mSortOrder;
        }

        public void ChangeSortOrder(int newSortOrder = -1)
        {
            if (newSortOrder != -1)
            {
                mTransform.SetSiblingIndex(newSortOrder);
                mSortOrder = newSortOrder;
            }
        }

        private void ForceHideAllCurrent(ref float waitTime)
        {
            while (refStacks.Count > 0)
            {
                BasePopup basePopup = refStacks.Pop();
                waitTime += basePopup.GetAnimationClipDuration(basePopup.hideAnimationClip);
                basePopup.Hide();
            }
        }

        private float GetAnimationClipDuration(AnimationClip clip)
        {
            if (popAnimation != null && clip != null)
            {
                return popAnimation.GetClip(clip.name).length;
            }

            return 0f;
        }

        private void MoveElementToTopStack(ref Stack<BasePopup> stack, int order)
        {
            Stack<BasePopup> stack2 = new Stack<BasePopup>();
            BasePopup basePopup = null;
            int num = 0;
            while (refStacks.Count > 0)
            {
                BasePopup basePopup2 = refStacks.Pop();
                if (basePopup2.SortOrder() != order)
                {
                    stack2.Push(basePopup2);
                    num = basePopup2.SortOrder();
                }
                else
                {
                    basePopup = basePopup2;
                }
            }
            while (stack2.Count > 0)
            {
                BasePopup basePopup3 = stack2.Pop();
                basePopup3.ChangeSortOrder(num++);
                stack.Push(basePopup3);
                if (!PopupManager.Instance.isShowOverPop)
                {
                    basePopup3.gameObject.SetActive(false);
                }
            }
            if (basePopup != null)
            {
                basePopup.gameObject.SetActive(true);
                basePopup.ChangeSortOrder(num);
                stack.Push(basePopup);
            }
        }
        private void checkListCache()
        {
            PopupManager.Instance.cachePopup.Remove(this);
        }
    }
}
