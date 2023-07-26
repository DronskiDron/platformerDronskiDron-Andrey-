using System;

namespace Creatures.Model.Data.Properties
{
    public class StringProperty : ObservableProperty<string>
    {
        public StringProperty(string defaultValue) : base(defaultValue)
        {
        }

        protected override string Read(string defaultValue)
        {
            return _value;
        }

        protected override void Write(string value)
        {
            _value = value;
        }
    }
}
