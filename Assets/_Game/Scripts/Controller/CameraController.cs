using UnityEngine;
using System.Collections;

namespace MSDefense
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 m_Offset;

        // drag cam
        public Camera mainCam;

        private Vector3 touchStart;
        int countTouch;
        private Vector3 direction;
        private float damping;

        private void Start()
        {

        }

        void Update()
        {
            if (GameController.Instance.gameState != GameEnum.GameState.InGame) return;
            if (Input.touchCount >= 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                zoom(difference * 0.02f);
            }
            else if(mainCam.orthographicSize > 9)
            {
                mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize - 2, 9, 15);
            }

            //else
            {
#if UNITY_EDITOR
                if (Input.GetMouseButtonDown(0))
                {
                    touchStart = mainCam.ScreenToWorldPoint(Input.mousePosition);
                }
#else
                ChangeTouch();
#endif
                if (Input.GetMouseButton(0))
                {
                    direction = touchStart - mainCam.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 newPos = transform.position + direction;
                    newPos.x = Mathf.Clamp(newPos.x, -55, 0);
                    newPos.y = Mathf.Clamp(newPos.y, 20, 82);
                    newPos.z = 0;
                    transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 50);
                    damping = 1f;
                }
                else if(damping > 0)
                {
                    Vector3 newPos = transform.position + direction;
                    newPos.x = Mathf.Clamp(newPos.x, -55, 0);
                    newPos.y = Mathf.Clamp(newPos.y, 20, 82);
                    newPos.z = 0;
                    transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 30);
                    damping -= Time.deltaTime * 0.5f;
                    direction *= damping;
                }
            }
        }
        void zoom(float increment)
        {
            mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize - increment, 9, 15);
        }
        private void ChangeTouch()
        {
            if (countTouch != Input.touchCount)
            {
                touchStart = mainCam.ScreenToWorldPoint(Input.mousePosition);
                countTouch = Input.touchCount;
            }
        }
        public void CameraShake()
        {
            StartCoroutine(Shake(0.5f, 0.3f));
            IEnumerator Shake(float duration, float magnitude)
            {
                Vector3 originalPos = new Vector3(0, 0, -72.84f); //mainCam.transform.localPosition;
                mainCam.transform.localPosition = originalPos;
                float elapsed = 0f;

                while (elapsed < duration && Time.timeScale > 0)
                {
                    float x = Random.Range(-1f, 1f) * magnitude;
                    float y = Random.Range(-1f, 1f) * magnitude;

                    mainCam.transform.localPosition = originalPos + new Vector3(x, y);

                    elapsed += Time.deltaTime;
                    yield return null;
                }
                mainCam.transform.localPosition = originalPos;
            }
        }
    }
}