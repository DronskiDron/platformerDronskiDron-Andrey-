using UnityEngine;
using Player;

namespace General.Components
{
    public class MoneySendComponent : MonoBehaviour
    {
        [SerializeField] private CoinCounter _coincounter;
        [SerializeField] private int _coinCost = 1;


        public void SendMoney()
        {
            _coincounter.GetMoney(_coinCost);
        }
    }
}

