using UI.Widgets;
using Utils.Disposables;

namespace General.Components.Health
{
    public class CompositeHealthComponent : HealthComponent
    {
        private ProgressBarWidget _lifeBar;
        private int _maxHp;

        private readonly CompositeDisposable _trash = new CompositeDisposable();


        private void Awake()
        {
            HpManager();
            LifeBarSearch();
        }


        private void LifeBarSearch()
        {
            var bars = GetComponentsInChildren<ProgressBarWidget>();
            foreach (var item in bars)
            {
                _lifeBar = item;
            }
        }


        private void HpManager()
        {
            var commonHp = GetComponentsInChildren<HealthComponent>();

            foreach (var member in commonHp)
            {
                _maxHp = member.Health;
                _trash.Retain(member._onChange.Subscribe(OnHpChanged));
            }
        }


        private void OnHpChanged(int hp)
        {
            var progress = (float)hp / _maxHp;
            _lifeBar.SetProgress(progress);
        }


        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}
