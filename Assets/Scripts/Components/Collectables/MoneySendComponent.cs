using UnityEngine;
using Creatures;

namespace General.Components.Collectables
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

