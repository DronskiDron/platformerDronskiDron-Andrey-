using UnityEngine;
using Player;

namespace General.Components
{
    public class MoneySendComponent : MonoBehaviour
    {
        [SerializeField] private CoinCounter _coincounter;
        [SerializeField] private float _coinCost = 1f;


        public void SendMoney()
        {
            _coincounter.GetMoney(_coinCost);
        }
    }
}

