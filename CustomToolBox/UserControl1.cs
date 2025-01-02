using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MailControlLibrary1
{
    public partial class UserControl1: TextBox
    {
        private Color _bottomBorderColor = Color.Black;
        private Color _onFocusColor = Color.Blue;

        public UserControl1()
        {
            BorderStyle = BorderStyle.None;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            Controls.Add(new Label
            {
                Height = 2,
                Dock = DockStyle.Bottom,
                BackColor = _bottomBorderColor
            });
        }
        [Browsable (false)]
        public new BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }
        public Color BottomBorderColor
        {
            get { return _bottomBorderColor; }
            set 
            {
                _bottomBorderColor = value;
                Controls[0].BackColor = _bottomBorderColor;
            }
        }
        public Color BottomBorderFocusColor
        {
            get { return _onFocusColor; }
            set { _onFocusColor = value; }
        }

        private void UserControl1_Enter(object sender, EventArgs e)
        {
            Controls[0].BackColor = _onFocusColor;
        }

        private void UserControl1_Leave(object sender, EventArgs e)
        {
            Controls[0].BackColor = _bottomBorderColor;
        }
    }
}
