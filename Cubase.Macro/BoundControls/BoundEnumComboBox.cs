using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cubase.Macro.BoundControls
{
    public class BoundEnumComboBox : ComboBox
    {
        private Type? _enumType;

        [Browsable(false)]
        public object? SelectedEnumValue { get; private set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type? EnumType
        {
            get => _enumType;
            set
            {
                if (value == null || !value.IsEnum)
                    return;

                _enumType = value;
                Populate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<object>? OnEnumSelected { get; set; }    

        private void Populate()
        {
            if (_enumType == null)
                return;

            Items.Clear();
            Items.AddRange(Enum.GetNames(_enumType));
        }

        public void Bind(object value)
        {
            if (_enumType == null || value.GetType() != _enumType)
                return;
            SelectedItem = value.ToString();
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            if (_enumType == null || SelectedItem == null)
                return;

            SelectedEnumValue = Enum.Parse(_enumType, SelectedItem.ToString()!);
            if (this.OnEnumSelected != null)
            {
                this.OnEnumSelected(SelectedEnumValue);
            }
        }
    }
}
