namespace @try
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.code = new System.Windows.Forms.TextBox();
            this.token = new System.Windows.Forms.TextBox();
            this.Parse = new System.Windows.Forms.TextBox();
            this.Expression = new System.Windows.Forms.TextBox();
            this.assignment = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(596, 349);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // code
            // 
            this.code.Location = new System.Drawing.Point(523, 83);
            this.code.Multiline = true;
            this.code.Name = "code";
            this.code.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.code.Size = new System.Drawing.Size(218, 246);
            this.code.TabIndex = 1;
            this.code.TextChanged += new System.EventHandler(this.code_TextChanged);
            // 
            // token
            // 
            this.token.Location = new System.Drawing.Point(3, 12);
            this.token.Multiline = true;
            this.token.Name = "token";
            this.token.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.token.Size = new System.Drawing.Size(434, 153);
            this.token.TabIndex = 2;
            // 
            // Parse
            // 
            this.Parse.Location = new System.Drawing.Point(3, 377);
            this.Parse.Multiline = true;
            this.Parse.Name = "Parse";
            this.Parse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Parse.Size = new System.Drawing.Size(434, 61);
            this.Parse.TabIndex = 3;
            this.Parse.TextChanged += new System.EventHandler(this.assignment_TextChanged);
            // 
            // Expression
            // 
            this.Expression.Location = new System.Drawing.Point(3, 202);
            this.Expression.Multiline = true;
            this.Expression.Name = "Expression";
            this.Expression.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Expression.Size = new System.Drawing.Size(434, 61);
            this.Expression.TabIndex = 4;
            this.Expression.TextChanged += new System.EventHandler(this.Expression_TextChanged);
            // 
            // assignment
            // 
            this.assignment.Location = new System.Drawing.Point(3, 286);
            this.assignment.Multiline = true;
            this.assignment.Name = "assignment";
            this.assignment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.assignment.Size = new System.Drawing.Size(434, 61);
            this.assignment.TabIndex = 5;
            this.assignment.TextChanged += new System.EventHandler(this.assignment_TextChanged_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.assignment);
            this.Controls.Add(this.Expression);
            this.Controls.Add(this.Parse);
            this.Controls.Add(this.token);
            this.Controls.Add(this.code);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox code;
        private System.Windows.Forms.TextBox token;
        private System.Windows.Forms.TextBox Parse;
        private System.Windows.Forms.TextBox Expression;
        private System.Windows.Forms.TextBox assignment;
    }
}

