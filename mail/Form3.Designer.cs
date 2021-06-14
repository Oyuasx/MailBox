
namespace mail
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button4 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button7 = new mail.UserControl2();
            this.button5 = new mail.UserControl2();
            this.button6 = new mail.UserControl2();
            this.button2 = new mail.UserControl2();
            this.button3 = new mail.UserControl2();
            this.button1 = new mail.UserControl2();
            this.textBox1 = new MailControlLibrary1.UserControl1();
            this.button8 = new mail.UserControl2();
            this.button10 = new mail.UserControl2();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox2
            // 
            this.listBox2.BackColor = System.Drawing.Color.SkyBlue;
            this.listBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.listBox2.ForeColor = System.Drawing.Color.Black;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 16;
            this.listBox2.Location = new System.Drawing.Point(798, 218);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(414, 592);
            this.listBox2.TabIndex = 13;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox2.BackColor = System.Drawing.Color.Gray;
            this.pictureBox2.Location = new System.Drawing.Point(-10, -1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(1236, 30);
            this.pictureBox2.TabIndex = 17;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseDown);
            this.pictureBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseMove);
            this.pictureBox2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseUp);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Gray;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button4.Location = new System.Drawing.Point(1196, -1);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(30, 30);
            this.button4.TabIndex = 18;
            this.button4.Tag = "";
            this.button4.Text = "X";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 48);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(554, 105);
            this.pictureBox1.TabIndex = 19;
            this.pictureBox1.TabStop = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.SkyBlue;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(12, 169);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(771, 712);
            this.richTextBox1.TabIndex = 20;
            this.richTextBox1.Text = "";
            this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.checkBox1.Location = new System.Drawing.Point(789, 818);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(76, 17);
            this.checkBox1.TabIndex = 21;
            this.checkBox1.Text = "OnlyText";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(1095, 819);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "label1";
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.SkyBlue;
            this.button9.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button9.FlatAppearance.BorderSize = 3;
            this.button9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DeepSkyBlue;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button9.ForeColor = System.Drawing.Color.Navy;
            this.button9.Location = new System.Drawing.Point(789, 841);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(91, 42);
            this.button9.TabIndex = 34;
            this.button9.Text = "Ekleri İndir";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.DarkRed;
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Location = new System.Drawing.Point(880, 841);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(18, 42);
            this.pictureBox3.TabIndex = 35;
            this.pictureBox3.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(1095, 191);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "label2";
            // 
            // button7
            // 
            this.button7.BorderColor = System.Drawing.Color.Black;
            this.button7.ButtonColor = System.Drawing.Color.SkyBlue;
            this.button7.FlatAppearance.BorderSize = 0;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button7.Location = new System.Drawing.Point(1030, 138);
            this.button7.Name = "button7";
            this.button7.OnHoverBorderColor = System.Drawing.Color.Navy;
            this.button7.OnHoverButtonColor = System.Drawing.Color.DeepSkyBlue;
            this.button7.OnHoverTextColor = System.Drawing.Color.Navy;
            this.button7.Size = new System.Drawing.Size(186, 39);
            this.button7.TabIndex = 45;
            this.button7.Text = "Gönderilen Mesajlar";
            this.button7.TextColor = System.Drawing.Color.Navy;
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button5
            // 
            this.button5.BorderColor = System.Drawing.Color.Black;
            this.button5.ButtonColor = System.Drawing.Color.SkyBlue;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button5.Location = new System.Drawing.Point(1030, 48);
            this.button5.Name = "button5";
            this.button5.OnHoverBorderColor = System.Drawing.Color.Navy;
            this.button5.OnHoverButtonColor = System.Drawing.Color.DeepSkyBlue;
            this.button5.OnHoverTextColor = System.Drawing.Color.Navy;
            this.button5.Size = new System.Drawing.Size(186, 39);
            this.button5.TabIndex = 44;
            this.button5.Text = "Gelen Mesajlar";
            this.button5.TextColor = System.Drawing.Color.Navy;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.BorderColor = System.Drawing.Color.Black;
            this.button6.ButtonColor = System.Drawing.Color.SkyBlue;
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button6.Location = new System.Drawing.Point(904, 823);
            this.button6.Name = "button6";
            this.button6.OnHoverBorderColor = System.Drawing.Color.Navy;
            this.button6.OnHoverButtonColor = System.Drawing.Color.DeepSkyBlue;
            this.button6.OnHoverTextColor = System.Drawing.Color.Navy;
            this.button6.Size = new System.Drawing.Size(91, 28);
            this.button6.TabIndex = 41;
            this.button6.Text = "Güncelle";
            this.button6.TextColor = System.Drawing.Color.Navy;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button2
            // 
            this.button2.BorderColor = System.Drawing.Color.Black;
            this.button2.ButtonColor = System.Drawing.Color.SkyBlue;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button2.Location = new System.Drawing.Point(997, 823);
            this.button2.Name = "button2";
            this.button2.OnHoverBorderColor = System.Drawing.Color.Navy;
            this.button2.OnHoverButtonColor = System.Drawing.Color.DeepSkyBlue;
            this.button2.OnHoverTextColor = System.Drawing.Color.Navy;
            this.button2.Size = new System.Drawing.Size(91, 28);
            this.button2.TabIndex = 40;
            this.button2.Text = "Oku";
            this.button2.TextColor = System.Drawing.Color.Navy;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BorderColor = System.Drawing.Color.Black;
            this.button3.ButtonColor = System.Drawing.Color.SkyBlue;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button3.Location = new System.Drawing.Point(997, 857);
            this.button3.Name = "button3";
            this.button3.OnHoverBorderColor = System.Drawing.Color.Navy;
            this.button3.OnHoverButtonColor = System.Drawing.Color.DeepSkyBlue;
            this.button3.OnHoverTextColor = System.Drawing.Color.Navy;
            this.button3.Size = new System.Drawing.Size(91, 28);
            this.button3.TabIndex = 39;
            this.button3.Text = "Sil";
            this.button3.TextColor = System.Drawing.Color.Navy;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.BorderColor = System.Drawing.Color.Black;
            this.button1.ButtonColor = System.Drawing.Color.SkyBlue;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button1.Location = new System.Drawing.Point(1098, 838);
            this.button1.Name = "button1";
            this.button1.OnHoverBorderColor = System.Drawing.Color.Navy;
            this.button1.OnHoverButtonColor = System.Drawing.Color.DeepSkyBlue;
            this.button1.OnHoverTextColor = System.Drawing.Color.Navy;
            this.button1.Size = new System.Drawing.Size(118, 48);
            this.button1.TabIndex = 38;
            this.button1.Text = "Mesaj Gönder";
            this.button1.TextColor = System.Drawing.Color.Navy;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.BottomBorderColor = System.Drawing.Color.Black;
            this.textBox1.BottomBorderFocusColor = System.Drawing.Color.Blue;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.textBox1.Location = new System.Drawing.Point(798, 183);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(280, 22);
            this.textBox1.TabIndex = 32;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button8
            // 
            this.button8.BorderColor = System.Drawing.Color.Black;
            this.button8.ButtonColor = System.Drawing.Color.SkyBlue;
            this.button8.FlatAppearance.BorderSize = 0;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button8.Location = new System.Drawing.Point(1030, 93);
            this.button8.Name = "button8";
            this.button8.OnHoverBorderColor = System.Drawing.Color.Navy;
            this.button8.OnHoverButtonColor = System.Drawing.Color.DeepSkyBlue;
            this.button8.OnHoverTextColor = System.Drawing.Color.Navy;
            this.button8.Size = new System.Drawing.Size(186, 39);
            this.button8.TabIndex = 46;
            this.button8.Text = "Çöp Kutusu";
            this.button8.TextColor = System.Drawing.Color.Navy;
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button10
            // 
            this.button10.BorderColor = System.Drawing.Color.Black;
            this.button10.ButtonColor = System.Drawing.Color.SkyBlue;
            this.button10.FlatAppearance.BorderSize = 0;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button10.Location = new System.Drawing.Point(904, 858);
            this.button10.Name = "button10";
            this.button10.OnHoverBorderColor = System.Drawing.Color.Navy;
            this.button10.OnHoverButtonColor = System.Drawing.Color.DeepSkyBlue;
            this.button10.OnHoverTextColor = System.Drawing.Color.Navy;
            this.button10.Size = new System.Drawing.Size(91, 28);
            this.button10.TabIndex = 47;
            this.button10.Text = "Sil-İptal";
            this.button10.TextColor = System.Drawing.Color.Navy;
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ClientSize = new System.Drawing.Size(1225, 896);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.listBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form3";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private MailControlLibrary1.UserControl1 textBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label2;
        private UserControl2 button1;
        private UserControl2 button3;
        private UserControl2 button2;
        private UserControl2 button6;
        private UserControl2 button5;
        private UserControl2 button7;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private UserControl2 button8;
        private UserControl2 button10;
    }
}