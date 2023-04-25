
using System.Drawing;

namespace MarkdownEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.HeadingLabel = new System.Windows.Forms.PictureBox();
            this.H3_button = new System.Windows.Forms.Button();
            this.H2_button = new System.Windows.Forms.Button();
            this.H0_button = new System.Windows.Forms.Button();
            this.H1_button = new System.Windows.Forms.Button();
            this.HeadingBackpanel = new System.Windows.Forms.Button();
            this.Heading_button = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.Underline_button = new System.Windows.Forms.Button();
            this.Bold_button = new System.Windows.Forms.Button();
            this.Font_button = new System.Windows.Forms.Button();
            this.Strikethrough_button = new System.Windows.Forms.Button();
            this.Link_button = new System.Windows.Forms.Button();
            this.Italics_button = new System.Windows.Forms.Button();
            this.Image_button = new System.Windows.Forms.Button();
            this.Code_button = new System.Windows.Forms.Button();
            this.Quote_button = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeadingLabel)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer1.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splitContainer1.Panel1.Controls.Add(this.HeadingLabel);
            this.splitContainer1.Panel1.Controls.Add(this.H3_button);
            this.splitContainer1.Panel1.Controls.Add(this.H2_button);
            this.splitContainer1.Panel1.Controls.Add(this.H0_button);
            this.splitContainer1.Panel1.Controls.Add(this.H1_button);
            this.splitContainer1.Panel1.Controls.Add(this.HeadingBackpanel);
            this.splitContainer1.Panel1.Controls.Add(this.Heading_button);
            this.splitContainer1.Panel1.Controls.Add(this.richTextBox1);
            this.splitContainer1.Panel1.Controls.Add(this.Underline_button);
            this.splitContainer1.Panel1.Controls.Add(this.Bold_button);
            this.splitContainer1.Panel1.Controls.Add(this.Font_button);
            this.splitContainer1.Panel1.Controls.Add(this.Strikethrough_button);
            this.splitContainer1.Panel1.Controls.Add(this.Link_button);
            this.splitContainer1.Panel1.Controls.Add(this.Italics_button);
            this.splitContainer1.Panel1.Controls.Add(this.Image_button);
            this.splitContainer1.Panel1.Controls.Add(this.Code_button);
            this.splitContainer1.Panel1.Controls.Add(this.Quote_button);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.webBrowser1);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 384;
            this.splitContainer1.TabIndex = 0;
            // 
            // HeadingLabel
            // 
            this.HeadingLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(135)))), ((int)(((byte)(144)))));
            this.HeadingLabel.BackgroundImage = global::MarkdownEditor.Properties.Resources.H0;
            this.HeadingLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.HeadingLabel.Location = new System.Drawing.Point(259, 16);
            this.HeadingLabel.Name = "HeadingLabel";
            this.HeadingLabel.Size = new System.Drawing.Size(16, 17);
            this.HeadingLabel.TabIndex = 6;
            this.HeadingLabel.TabStop = false;
            // 
            // H3_button
            // 
            this.H3_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(135)))), ((int)(((byte)(144)))));
            this.H3_button.BackgroundImage = global::MarkdownEditor.Properties.Resources.H3_button;
            this.H3_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.H3_button.Location = new System.Drawing.Point(254, 137);
            this.H3_button.Name = "H3_button";
            this.H3_button.Size = new System.Drawing.Size(40, 33);
            this.H3_button.TabIndex = 5;
            this.H3_button.UseVisualStyleBackColor = false;
            this.H3_button.Visible = false;
            this.H3_button.Click += new System.EventHandler(this.H_button_Click);
            // 
            // H2_button
            // 
            this.H2_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(135)))), ((int)(((byte)(144)))));
            this.H2_button.BackgroundImage = global::MarkdownEditor.Properties.Resources.H2_button;
            this.H2_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.H2_button.Location = new System.Drawing.Point(254, 103);
            this.H2_button.Name = "H2_button";
            this.H2_button.Size = new System.Drawing.Size(40, 33);
            this.H2_button.TabIndex = 4;
            this.H2_button.UseVisualStyleBackColor = false;
            this.H2_button.Visible = false;
            this.H2_button.Click += new System.EventHandler(this.H_button_Click);
            // 
            // H0_button
            // 
            this.H0_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(135)))), ((int)(((byte)(144)))));
            this.H0_button.BackgroundImage = global::MarkdownEditor.Properties.Resources.H0_button;
            this.H0_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.H0_button.Location = new System.Drawing.Point(254, 35);
            this.H0_button.Name = "H0_button";
            this.H0_button.Size = new System.Drawing.Size(40, 33);
            this.H0_button.TabIndex = 2;
            this.H0_button.UseVisualStyleBackColor = false;
            this.H0_button.Visible = false;
            this.H0_button.Click += new System.EventHandler(this.H_button_Click);
            // 
            // H1_button
            // 
            this.H1_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(135)))), ((int)(((byte)(144)))));
            this.H1_button.BackgroundImage = global::MarkdownEditor.Properties.Resources.H1_button;
            this.H1_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.H1_button.Location = new System.Drawing.Point(254, 69);
            this.H1_button.Name = "H1_button";
            this.H1_button.Size = new System.Drawing.Size(40, 33);
            this.H1_button.TabIndex = 3;
            this.H1_button.UseVisualStyleBackColor = false;
            this.H1_button.Visible = false;
            this.H1_button.Click += new System.EventHandler(this.H_button_Click);
            // 
            // HeadingBackpanel
            // 
            this.HeadingBackpanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("HeadingBackpanel.BackgroundImage")));
            this.HeadingBackpanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.HeadingBackpanel.Location = new System.Drawing.Point(249, 9);
            this.HeadingBackpanel.Name = "HeadingBackpanel";
            this.HeadingBackpanel.Size = new System.Drawing.Size(50, 167);
            this.HeadingBackpanel.TabIndex = 12;
            this.HeadingBackpanel.TabStop = false;
            this.HeadingBackpanel.Visible = false;
            this.HeadingBackpanel.Click += new System.EventHandler(this.HeadingBackpanel_Click);
            // 
            // Heading_button
            // 
            this.Heading_button.BackgroundImage = global::MarkdownEditor.Properties.Resources.Heading;
            this.Heading_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Heading_button.Location = new System.Drawing.Point(250, 9);
            this.Heading_button.Name = "Heading_button";
            this.Heading_button.Size = new System.Drawing.Size(50, 30);
            this.Heading_button.TabIndex = 7;
            this.Heading_button.TabStop = false;
            this.Heading_button.Click += new System.EventHandler(this.HeadingClicked);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox1.Location = new System.Drawing.Point(0, 45);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(384, 405);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            this.richTextBox1.SelectionChanged += new System.EventHandler(this.richTextBox1_SelectionChanged);
            this.richTextBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.richTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyDown);
            // 
            // Underline_button
            // 
            this.Underline_button.BackgroundImage = global::MarkdownEditor.Properties.Resources.Underline;
            this.Underline_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Underline_button.Location = new System.Drawing.Point(110, 9);
            this.Underline_button.Name = "Underline_button";
            this.Underline_button.Size = new System.Drawing.Size(30, 30);
            this.Underline_button.TabIndex = 11;
            this.Underline_button.TabStop = false;
            this.Underline_button.Click += new System.EventHandler(this.UnderlineClicked);
            // 
            // Bold_button
            // 
            this.Bold_button.BackgroundImage = global::MarkdownEditor.Properties.Resources.Bold;
            this.Bold_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Bold_button.Location = new System.Drawing.Point(40, 9);
            this.Bold_button.Name = "Bold_button";
            this.Bold_button.Size = new System.Drawing.Size(30, 30);
            this.Bold_button.TabIndex = 3;
            this.Bold_button.TabStop = false;
            this.Bold_button.Click += new System.EventHandler(this.BoldClicked);
            // 
            // Font_button
            // 
            this.Font_button.BackgroundImage = global::MarkdownEditor.Properties.Resources.Font;
            this.Font_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Font_button.Location = new System.Drawing.Point(215, 9);
            this.Font_button.Name = "Font_button";
            this.Font_button.Size = new System.Drawing.Size(30, 30);
            this.Font_button.TabIndex = 10;
            this.Font_button.TabStop = false;
            this.Font_button.Click += new System.EventHandler(this.Font_button_Click);
            // 
            // Strikethrough_button
            // 
            this.Strikethrough_button.BackgroundImage = global::MarkdownEditor.Properties.Resources.Strikethrough;
            this.Strikethrough_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Strikethrough_button.Location = new System.Drawing.Point(5, 9);
            this.Strikethrough_button.Name = "Strikethrough_button";
            this.Strikethrough_button.Size = new System.Drawing.Size(30, 30);
            this.Strikethrough_button.TabIndex = 2;
            this.Strikethrough_button.TabStop = false;
            this.Strikethrough_button.Click += new System.EventHandler(this.StrikeClicked);
            // 
            // Link_button
            // 
            this.Link_button.BackgroundImage = global::MarkdownEditor.Properties.Resources.Link;
            this.Link_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Link_button.Location = new System.Drawing.Point(345, 9);
            this.Link_button.Name = "Link_button";
            this.Link_button.Size = new System.Drawing.Size(33, 30);
            this.Link_button.TabIndex = 9;
            this.Link_button.TabStop = false;
            this.Link_button.Click += new System.EventHandler(this.Link_button_Click);
            // 
            // Italics_button
            // 
            this.Italics_button.BackgroundImage = global::MarkdownEditor.Properties.Resources.Italics;
            this.Italics_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Italics_button.Location = new System.Drawing.Point(75, 9);
            this.Italics_button.Name = "Italics_button";
            this.Italics_button.Size = new System.Drawing.Size(30, 30);
            this.Italics_button.TabIndex = 4;
            this.Italics_button.TabStop = false;
            this.Italics_button.Click += new System.EventHandler(this.ItalicsClicked);
            // 
            // Image_button
            // 
            this.Image_button.BackgroundImage = global::MarkdownEditor.Properties.Resources.Image;
            this.Image_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Image_button.Location = new System.Drawing.Point(305, 9);
            this.Image_button.Name = "Image_button";
            this.Image_button.Size = new System.Drawing.Size(35, 30);
            this.Image_button.TabIndex = 8;
            this.Image_button.TabStop = false;
            this.Image_button.Click += new System.EventHandler(this.Image_button_Click);
            // 
            // Code_button
            // 
            this.Code_button.BackgroundImage = global::MarkdownEditor.Properties.Resources.Code;
            this.Code_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Code_button.Location = new System.Drawing.Point(145, 9);
            this.Code_button.Name = "Code_button";
            this.Code_button.Size = new System.Drawing.Size(30, 30);
            this.Code_button.TabIndex = 5;
            this.Code_button.TabStop = false;
            this.Code_button.Click += new System.EventHandler(this.CodeClicked);
            // 
            // Quote_button
            // 
            this.Quote_button.BackgroundImage = global::MarkdownEditor.Properties.Resources.Quote;
            this.Quote_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Quote_button.Location = new System.Drawing.Point(180, 9);
            this.Quote_button.Name = "Quote_button";
            this.Quote_button.Size = new System.Drawing.Size(30, 30);
            this.Quote_button.TabIndex = 6;
            this.Quote_button.TabStop = false;
            this.Quote_button.Click += new System.EventHandler(this.QuoteClicked);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(412, 450);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Url = new System.Uri("http://6", System.UriKind.Absolute);
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Editor";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HeadingLabel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button Font_button;
        private System.Windows.Forms.Button Link_button;
        private System.Windows.Forms.Button Image_button;
        private System.Windows.Forms.Button Heading_button;
        private System.Windows.Forms.Button Quote_button;
        private System.Windows.Forms.Button Code_button;
        private System.Windows.Forms.Button Italics_button;
        private System.Windows.Forms.Button Bold_button;
        private System.Windows.Forms.Button Strikethrough_button;
        private System.Windows.Forms.Button Underline_button;
        private System.Windows.Forms.Button H0_button;
        private System.Windows.Forms.Button H3_button;
        private System.Windows.Forms.Button H2_button;
        private System.Windows.Forms.Button H1_button;
        private System.Windows.Forms.PictureBox HeadingLabel;
        private System.Windows.Forms.Button HeadingBackpanel;
        private System.Windows.Forms.FontDialog fontDialog1;
    }
}