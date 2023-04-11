using System;
using UnityEngine;
using Utils.Disposables;

namespace Creatures.Model.Data.Properties
{
    [Serializable]
    public abstract class ObservableProperty<TPropertyType>
    {
        [SerializeField] protected TPropertyType _value;

        private TPropertyType _stored;
        private TPropertyType _defaultValue;

        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);
        public event OnPropertyChanged OnChanged;

        public TPropertyType Value
        {
            get => _stored;
            set
            {
                var isEquals = _stored.Equals(value);
                if (isEquals) return;

                var oldValue = _stored;
                Write(value);
                _stored = _value = value;

                OnChanged?.Invoke(value, oldValue);
            }
        }

        public ObservableProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        }


        public IDisposable Subscribe(OnPropertyChanged call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }


        public IDisposable SubscribeAndInvoke(OnPropertyChanged call)
        {
            OnChanged += call;
            var dispose = new ActionDisposable(() => OnChanged -= call);
            call(_value, _value);
            return dispose;
        }


        protected void Init()
        {
            _stored = _value = Read(_defaultValue);
        }


        protected abstract void Write(TPropertyType value);


        protected abstract TPropertyType Read(TPropertyType defaultValue);


        public void Validate()
        {
            if (!_stored.Equals(_value))
                Value = _value;
        }
    }
}
