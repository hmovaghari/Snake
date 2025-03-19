namespace Snake.Desktop
{
    partial class frmGame
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGame));
            this.lblBoardGameBottom = new System.Windows.Forms.Label();
            this.pnlGameGround = new System.Windows.Forms.Panel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.lblBoardGametop = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblBoardGameBottom
            // 
            this.lblBoardGameBottom.AutoSize = true;
            this.lblBoardGameBottom.BackColor = System.Drawing.Color.White;
            this.lblBoardGameBottom.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBoardGameBottom.ForeColor = System.Drawing.Color.White;
            this.lblBoardGameBottom.Location = new System.Drawing.Point(18, 332);
            this.lblBoardGameBottom.Name = "lblBoardGameBottom";
            this.lblBoardGameBottom.Size = new System.Drawing.Size(560, 15);
            this.lblBoardGameBottom.TabIndex = 5;
            this.lblBoardGameBottom.Text = "███████████████████████████████████████████████████████████████████████████████";
            // 
            // pnlGameGround
            // 
            this.pnlGameGround.BackColor = System.Drawing.Color.Black;
            this.pnlGameGround.Location = new System.Drawing.Point(31, 95);
            this.pnlGameGround.Name = "pnlGameGround";
            this.pnlGameGround.Size = new System.Drawing.Size(532, 243);
            this.pnlGameGround.TabIndex = 6;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // lblBoardGametop
            // 
            this.lblBoardGametop.AutoSize = true;
            this.lblBoardGametop.BackColor = System.Drawing.Color.White;
            this.lblBoardGametop.ForeColor = System.Drawing.Color.White;
            this.lblBoardGametop.Location = new System.Drawing.Point(18, 82);
            this.lblBoardGametop.Name = "lblBoardGametop";
            this.lblBoardGametop.Size = new System.Drawing.Size(557, 13);
            this.lblBoardGametop.TabIndex = 8;
            this.lblBoardGametop.Text = "███████████████████████████████████████████████████████\r\n";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(18, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 52);
            this.label1.TabIndex = 9;
            this.label1.Text = "█\r\n█\r\n█\r\n█";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(560, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 52);
            this.label4.TabIndex = 10;
            this.label4.Text = "█\r\n█\r\n█\r\n█";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(18, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(557, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "█▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(252, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Welcome to Snake!";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Black;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(230, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(143, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Programming by HMovaghari";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(18, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 247);
            this.label2.TabIndex = 14;
            this.label2.Text = "█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(560, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 247);
            this.label3.TabIndex = 15;
            this.label3.Text = "█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.White;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(561, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 325);
            this.label8.TabIndex = 16;
            this.label8.Text = "█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█\r\n█";
            // 
            // frmGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(597, 373);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblBoardGameBottom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblBoardGametop);
            this.Controls.Add(this.pnlGameGround);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(613, 412);
            this.MinimumSize = new System.Drawing.Size(613, 412);
            this.Name = "frmGame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Snake";
            this.Load += new System.EventHandler(this.frmGame_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmGame_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblBoardGameBottom;
        private System.Windows.Forms.Panel pnlGameGround;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lblBoardGametop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
    }
}

