using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myeongjin
{
    public class CCameraManager : Singleton<CCameraManager>
    {
        [SerializeField] GameObject dollyCart;
        [SerializeField] GameObject openingCamera;

        private void Start()
        {
            dollyCart.SetActive(false);
            StartCoroutine(CUIManager.Instance.FadeOut(3.0f, SetDollyTrackActivate, OpeningEnd));
        }
        private void SetDollyTrackActivate()
        {
            dollyCart.SetActive(true);
        }
        private void OpeningEnd()
        {
            openingCamera.SetActive(false);
        }
    }
}