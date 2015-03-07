using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace numericTextBox
{
        public partial class NumericTextBox : TextBox
    {
        public delegate void InvalidUserEntryEvent(object sender, KeyPressEventArgs e);
        public event InvalidUserEntryEvent InvalidUserEntry;
        
        public NumericTextBox()
        {
            InitializeComponent();
        }

        public NumericTextBox(IContainer container)
        {
            //Container.Add(this);
            InitializeComponent();
        }

        protected virtual void OnInvalidUserEntry(KeyPressEventArgs e)
        {
            if (InvalidUserEntry != null)
                InvalidUserEntry(this, e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public int ValueNumber
        {
            get
            {
                return int.Parse(Text);
            }
            set
            {
                Text = value.ToString();
            }

        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            int asciiInteger = Convert.ToInt32(e.KeyChar);
            if (asciiInteger == 00 || asciiInteger == 45 || asciiInteger > 47 && asciiInteger <= 57)         
            {
                e.Handled = false;
                return;
            }
            if (asciiInteger == 8)
            {
                e.Handled = false;
                return;
            }
            e.Handled = true;
            OnInvalidUserEntry(e);
        }

            [Localizable(false)]
            public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                try
                {
                    int.Parse(value);
                    base.Text = value;
                    return;
                }
                catch
                {         
                }
                if (value == null)
                {
                    base.Text = value;
                }
            }
        }
    }
}
