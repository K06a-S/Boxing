using UnityEngine;
using UnityEngine.SceneManagement;

namespace Boxing
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        private void Awake()
        {
            var boxers = GetComponentsInChildren<Damageable>();
            foreach (Damageable boxer in boxers)
            {
                boxer.OnDefeat += GameOver;
            }
            _mainCanvas.gameObject.SetActive(false);
        }

        private void GameOver(DamageInfo damageInfo)
        {
            _mainCanvas.gameObject.SetActive(true);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}