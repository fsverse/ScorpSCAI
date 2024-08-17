namespace ScorpSCAI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            textBoxToTwitch = new TextBox();
            textBoxFromTwitch = new TextBox();
            button2 = new Button();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(26, 179);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(279, 87);
            textBox1.TabIndex = 0;
            textBox1.KeyDown += textBox1_KeyDown;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(26, 311);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(430, 364);
            textBox2.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(336, 179);
            button1.Name = "button1";
            button1.Size = new Size(111, 45);
            button1.TabIndex = 2;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 24F);
            label1.Location = new Point(50, 83);
            label1.Name = "label1";
            label1.Size = new Size(121, 65);
            label1.TabIndex = 3;
            label1.Text = "Wiki";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 24F);
            label2.Location = new Point(875, 108);
            label2.Name = "label2";
            label2.Size = new Size(162, 65);
            label2.TabIndex = 4;
            label2.Text = "Twitch";
            // 
            // textBoxToTwitch
            // 
            textBoxToTwitch.Location = new Point(889, 201);
            textBoxToTwitch.Multiline = true;
            textBoxToTwitch.Name = "textBoxToTwitch";
            textBoxToTwitch.Size = new Size(390, 137);
            textBoxToTwitch.TabIndex = 5;
            textBoxToTwitch.TextChanged += textBoxToTwitch_TextChanged;
            textBoxToTwitch.KeyDown += textBoxToTwitch_KeyDown;
            // 
            // textBoxFromTwitch
            // 
            textBoxFromTwitch.Location = new Point(889, 399);
            textBoxFromTwitch.Multiline = true;
            textBoxFromTwitch.Name = "textBoxFromTwitch";
            textBoxFromTwitch.Size = new Size(399, 419);
            textBoxFromTwitch.TabIndex = 6;
            // 
            // button2
            // 
            button2.Location = new Point(1331, 205);
            button2.Name = "button2";
            button2.Size = new Size(144, 56);
            button2.TabIndex = 7;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1719, 1411);
            Controls.Add(button2);
            Controls.Add(textBoxFromTwitch);
            Controls.Add(textBoxToTwitch);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;
        private Label label1;
        private Label label2;
        private TextBox textBoxToTwitch;
        private TextBox textBoxFromTwitch;
        private Button button2;
    }
}
