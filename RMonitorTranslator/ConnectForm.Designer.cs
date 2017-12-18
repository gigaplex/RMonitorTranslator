namespace RMonitorTranslator
{
    partial class ConnectForm
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
            this.serverLabel = new System.Windows.Forms.Label();
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.comPortComboBox = new System.Windows.Forms.ComboBox();
            this.comPortLabel = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(12, 16);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(41, 13);
            this.serverLabel.TabIndex = 0;
            this.serverLabel.Text = "Server:";
            // 
            // serverTextBox
            // 
            this.serverTextBox.Location = new System.Drawing.Point(105, 13);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(167, 20);
            this.serverTextBox.TabIndex = 1;
            // 
            // comPortComboBox
            // 
            this.comPortComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comPortComboBox.FormattingEnabled = true;
            this.comPortComboBox.Location = new System.Drawing.Point(105, 40);
            this.comPortComboBox.Name = "comPortComboBox";
            this.comPortComboBox.Size = new System.Drawing.Size(167, 21);
            this.comPortComboBox.TabIndex = 2;
            // 
            // comPortLabel
            // 
            this.comPortLabel.AutoSize = true;
            this.comPortLabel.Location = new System.Drawing.Point(12, 43);
            this.comPortLabel.Name = "comPortLabel";
            this.comPortLabel.Size = new System.Drawing.Size(56, 13);
            this.comPortLabel.TabIndex = 3;
            this.comPortLabel.Text = "COM Port:";
            // 
            // connectButton
            // 
            this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.connectButton.Location = new System.Drawing.Point(197, 67);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 4;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // ConnectForm
            // 
            this.AcceptButton = this.connectButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 102);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.comPortLabel);
            this.Controls.Add(this.comPortComboBox);
            this.Controls.Add(this.serverTextBox);
            this.Controls.Add(this.serverLabel);
            this.Name = "ConnectForm";
            this.Text = "Connection Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.TextBox serverTextBox;
        private System.Windows.Forms.ComboBox comPortComboBox;
        private System.Windows.Forms.Label comPortLabel;
        private System.Windows.Forms.Button connectButton;
    }
}