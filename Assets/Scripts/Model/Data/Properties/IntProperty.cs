using System;

namespace Creatures.Model.Data.Properties
{
    [Serializable]
    public class IntProperty : ObservableProperty<int>
    {
        public IntProperty(int defaultValue) : base(defaultValue)
        {
        }

        protected override int Read(int defaultValue)
        {
            return _value;
        }

        protected override void Write(int value)
        {
            _value = value;
        }
    }
}
