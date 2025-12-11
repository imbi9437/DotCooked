using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Generic;
using Interface;
using Photon.Pun;
using UnityEngine;

namespace Manager
{
    public class GameManager : PunMonoSingleton<GameManager>
    {
        public float InitializeDelay = 2f;
        private List<MonoBehaviour> initializables;

        [SerializeField] private Player Player;

        private void Start()
        {
            initializables = GetComponents<MonoBehaviour>().Where(s => s is IInitializable).ToList();
            CheckManagerInitialize().Forget();
        }
        
        private async UniTaskVoid CheckManagerInitialize()
        {
            foreach (IInitializable initializable in initializables)
            {
                initializable.Initialize();
                await UniTask.WaitUntil(() => initializable.IsCompleteInitialize);
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(InitializeDelay));
            
            EventManager.Instance.OnCompleteManagerInitialize?.Invoke();
        }

        public Player CreatePlayer(Vector2 pos, bool isMine, int id)
        {
            var player = Instantiate(Player, pos, Quaternion.identity);
            if (isMine) player.InitEvent();
            if (id != 0) player.photonView.ViewID = id;

            return player;
        }
    }
}
