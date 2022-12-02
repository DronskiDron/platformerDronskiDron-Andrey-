using Player.Model;
using UnityEngine;

namespace Player
{
    public class CoinCounter : MonoBehaviour
    {
        private int _money;
        private int _moneyBalance;

        public int Money => _money;
        private GameSession _session;


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }


        public void GetMoney(int moneyFromObjects)
        {
            _session.Data.Coins= _money += moneyFromObjects;

            _moneyBalance = moneyFromObjects;
            MoneyConsoleWriter();
        }


        public void MoneyConsoleWriter()
        {
            if (_moneyBalance > 0)
            {
                Debug.Log($"Поздравляю! У Вас {_money} монет!");
            }
            else
            {
                Debug.Log($"Упс! У Вас осталось {_money} монет!");
            }
        }
    }
}
