using Player.Model;
using UnityEngine;

namespace Player
{
    public class CoinCounter : MonoBehaviour
    {
        private int _moneyBalance;

        private GameSession _session;


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }


        public void GetMoney(int moneyFromObjects)
        {
            _session.Data.Coins += moneyFromObjects;

            _moneyBalance = moneyFromObjects;
            MoneyConsoleWriter();
        }


        public void MoneyConsoleWriter()
        {
            if (_moneyBalance > 0)
            {
                Debug.Log($"Поздравляю! У Вас {_session.Data.Coins} монет!");
            }
            else
            {
                Debug.Log($"Упс! У Вас осталось {_session.Data.Coins} монет!");
            }
        }
    }
}
