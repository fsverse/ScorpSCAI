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
            label3 = new Label();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            button3 = new Button();
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
            textBox1.MouseDown += textBox1_MouseDown;
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
            label1.Location = new Point(26, 83);
            label1.Name = "label1";
            label1.Size = new Size(355, 65);
            label1.TabIndex = 3;
            label1.Text = "Wiki via Ollama";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 24F);
            label2.Location = new Point(655, 83);
            label2.Name = "label2";
            label2.Size = new Size(162, 65);
            label2.TabIndex = 4;
            label2.Text = "Twitch";
            // 
            // textBoxToTwitch
            // 
            textBoxToTwitch.Location = new Point(669, 179);
            textBoxToTwitch.Multiline = true;
            textBoxToTwitch.Name = "textBoxToTwitch";
            textBoxToTwitch.Size = new Size(279, 87);
            textBoxToTwitch.TabIndex = 5;
            textBoxToTwitch.TextChanged += textBoxToTwitch_TextChanged;
            textBoxToTwitch.KeyDown += textBoxToTwitch_KeyDown;
            textBoxToTwitch.MouseDown += textBoxToTwitch_MouseDown;
            // 
            // textBoxFromTwitch
            // 
            textBoxFromTwitch.Location = new Point(669, 311);
            textBoxFromTwitch.Multiline = true;
            textBoxFromTwitch.Name = "textBoxFromTwitch";
            textBoxFromTwitch.Size = new Size(399, 364);
            textBoxFromTwitch.TabIndex = 6;
            // 
            // button2
            // 
            button2.Location = new Point(968, 179);
            button2.Name = "button2";
            button2.Size = new Size(100, 45);
            button2.TabIndex = 7;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 24F);
            label3.Location = new Point(1229, 83);
            label3.Name = "label3";
            label3.Size = new Size(288, 65);
            label3.TabIndex = 8;
            label3.Text = "Ollama Only";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(1238, 179);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(279, 87);
            textBox3.TabIndex = 9;
            textBox3.KeyDown += textBox3_KeyDown;
            textBox3.MouseDown += textBox3_MouseDown;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(1238, 311);
            textBox4.Multiline = true;
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(430, 364);
            textBox4.TabIndex = 10;
            // 
            // button3
            // 
            button3.Location = new Point(1557, 179);
            button3.Name = "button3";
            button3.Size = new Size(111, 45);
            button3.TabIndex = 11;
            button3.Text = "button3";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1821, 877);
            Controls.Add(button3);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(label3);
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
        private Label label3;
        private TextBox textBox3;
        private TextBox textBox4;
        private Button button3;
    }
}
