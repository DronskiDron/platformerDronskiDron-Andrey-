using UnityEngine;

namespace Player
{
    public class CoinCounter : MonoBehaviour
    {
        private float _money;
        private float _moneyBalance;

        public float Money => _money;


        public void GetMoney(float moneyFromObjects)
        {
            _money += moneyFromObjects;

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
