using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameWork
{
    public class GameSceneManager : SingletonManager<GameSceneManager>
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public async UniTask LoadSceneAsync(string sceneName)
        {
            Debug.Log("GameSceneManager::LoadSceneAsync");
            await SceneManager.LoadSceneAsync(sceneName).ToUniTask(progress: Progress.Create<float> (x => Debug.Log(x)));
        }
    }   
}
