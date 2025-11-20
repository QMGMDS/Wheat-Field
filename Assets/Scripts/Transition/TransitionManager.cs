using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MFarm.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [SceneName] public string startSceneName = string.Empty;
        private CanvasGroup fadeCanvasGroup;

        private bool isFade; //是否进人渐入渐出动画

        private IEnumerator Start()
        {
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
            yield return LoadSceneSetActive(startSceneName);
            EventHandler.CallAfterSceneloadedEvent();
        }

        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }


        //切换场景事件触发
        private void OnTransitionEvent(string sceneToGo, Vector3 positionToGo)
        {
            if(!isFade)
                StartCoroutine(Transition(sceneToGo,positionToGo));
        }

        /// <summary>
        /// 场景切换
        /// </summary>
        /// <param name="sceneName">目标场景</param>
        /// <param name="targetPosition">目标位置</param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName,Vector3 targetPosition)
        {
            EventHandler.CallBeforeSceneUnloadEvent();
            
            yield return Fade(1); //场景变黑

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()); //卸载当前被激活的场景

            yield return LoadSceneSetActive(sceneName); //加载目标场景
            //这里不用StartCoroutine()的原因是LoadSceneSetActive()这个函数方法返回的已经是一个接口

            //移动人物坐标
            EventHandler.CallMoveToPosition(targetPosition);

            EventHandler.CallAfterSceneloadedEvent();

            yield return Fade(0); //场景显示出来
        }

        /// <summary>
        /// 加载场景并设置为激活
        /// </summary>
        /// <param name="sceneName">要加载的场景名字</param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);

            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1); //获得新加载出来的场景编号
            SceneManager.SetActiveScene(newScene);
        }




        /// <summary>
        /// 淡入淡出场景
        /// </summary>
        /// <param name="targetAlpha">1是黑，0是透明</param>
        /// <returns></returns>
        private IEnumerator Fade(float targetAlpha)
        {
            isFade = true;

            fadeCanvasGroup.blocksRaycasts = true; //鼠标射线遮挡，鼠标无法互动场景中的物体

            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.fadeDuration; //Mathf.Abs()取绝对值

            //Mathf.Approximately()比较函数，比较两个数是否相等，返回布尔值Approximately表示近似比较
            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                //Mathf.MoveTowards()趋近函数，让fadeCanvasGroup.alpha以speed * Time.deltaTime的速度趋近targetAlpha
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }

            fadeCanvasGroup.blocksRaycasts = false;
            isFade = false;
        }
    }

}
