using UnityEngine;

namespace Fox
{
    public class GameManager : MonoBehaviour
    {
        static public GameManager Instance;

        private void Awake()
        {
            if(Instance==null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}