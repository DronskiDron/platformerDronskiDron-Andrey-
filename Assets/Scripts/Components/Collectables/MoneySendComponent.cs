using UnityEngine;
using Creatures;

namespace General.Components.Collectables
{
    public class MoneySendComponent : MonoBehaviour
    {
        [SerializeField] private int _coinCost = 1;


        public void SendMoney(GameObject go)
        {
            var player = go.GetComponent<CoinCounter>();
            player.GetMoney(_coinCost);
        }
    }
}

