
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
            var resources = Properties.Resources.ResourceManager;
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.backgroundPanel = new System.Windows.Forms.Panel();
            this.Underline_button = new System.Windows.Forms.Button();
            this.Font_button = new System.Windows.Forms.Button();
            this.Link_button = new System.Windows.Forms.Button();
            this.Image_button = new System.Windows.Forms.Button();
            this.Heading_button = new System.Windows.Forms.Button();
            this.Quote_button = new System.Windows.Forms.Button();
            this.Code_button = new System.Windows.Forms.Button();
            this.Italics_button = new System.Windows.Forms.Button();
            this.Bold_button = new System.Windows.Forms.Button();
            this.Strikethrough_button = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.backgroundPanel.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.backgroundPanel);
            this.splitContainer1.Panel1.Controls.Add(this.richTextBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.webBrowser1);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 384;
            this.splitContainer1.TabIndex = 0;
            // 
            // backgroundPanel
            // 
            this.backgroundPanel.BackColor = System.Drawing.Color.LightGray;
            this.backgroundPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Background")));
            this.backgroundPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.backgroundPanel.Controls.Add(this.Underline_button);
            this.backgroundPanel.Controls.Add(this.Font_button);
            this.backgroundPanel.Controls.Add(this.Link_button);
            this.backgroundPanel.Controls.Add(this.Image_button);
            this.backgroundPanel.Controls.Add(this.Heading_button);
            this.backgroundPanel.Controls.Add(this.Quote_button);
            this.backgroundPanel.Controls.Add(this.Code_button);
            this.backgroundPanel.Controls.Add(this.Italics_button);
            this.backgroundPanel.Controls.Add(this.Bold_button);
            this.backgroundPanel.Controls.Add(this.Strikethrough_button);
            this.backgroundPanel.Location = new System.Drawing.Point(3, 0);
            this.backgroundPanel.Name = "backgroundPanel";
            this.backgroundPanel.Size = new System.Drawing.Size(378, 48);
            this.backgroundPanel.TabIndex = 1;
            // 
            // Underline_button
            // 
            this.Underline_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Underline")));
            this.Underline_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Underline_button.Location = new System.Drawing.Point(107, 9);
            this.Underline_button.Name = "Underline_button";
            this.Underline_button.Size = new System.Drawing.Size(30, 30);
            this.Underline_button.TabIndex = 11;
            this.Underline_button.TabStop = false;
            this.Underline_button.Click += new System.EventHandler(this.FormattingClicked);
            // 
            // Font_button
            // 
            this.Font_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Font")));
            this.Font_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Font_button.Location = new System.Drawing.Point(212, 9);
            this.Font_button.Name = "Font_button";
            this.Font_button.Size = new System.Drawing.Size(30, 30);
            this.Font_button.TabIndex = 10;
            this.Font_button.TabStop = false;
            this.Font_button.Click += new System.EventHandler(this.FormattingClicked);
            // 
            // Link_button
            // 
            this.Link_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Link")));
            this.Link_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Link_button.Location = new System.Drawing.Point(342, 9);
            this.Link_button.Name = "Link_button";
            this.Link_button.Size = new System.Drawing.Size(33, 30);
            this.Link_button.TabIndex = 9;
            this.Link_button.TabStop = false;
            this.Link_button.Click += new System.EventHandler(this.FormattingClicked);
            // 
            // Image_button
            // 
            this.Image_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Image")));
            this.Image_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Image_button.Location = new System.Drawing.Point(302, 9);
            this.Image_button.Name = "Image_button";
            this.Image_button.Size = new System.Drawing.Size(35, 30);
            this.Image_button.TabIndex = 8;
            this.Image_button.TabStop = false;
            this.Image_button.Click += new System.EventHandler(this.FormattingClicked);
            // 
            // Heading_button
            // 
            this.Heading_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Heading")));
            this.Heading_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Heading_button.Location = new System.Drawing.Point(247, 9);
            this.Heading_button.Name = "Heading_button";
            this.Heading_button.Size = new System.Drawing.Size(50, 30);
            this.Heading_button.TabIndex = 7;
            this.Heading_button.TabStop = false;
            this.Heading_button.Click += new System.EventHandler(this.FormattingClicked);
            // 
            // Quote_button
            // 
            this.Quote_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Quote")));
            this.Quote_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Quote_button.Location = new System.Drawing.Point(177, 9);
            this.Quote_button.Name = "Quote_button";
            this.Quote_button.Size = new System.Drawing.Size(30, 30);
            this.Quote_button.TabIndex = 6;
            this.Quote_button.TabStop = false;
            this.Quote_button.Click += new System.EventHandler(this.FormattingClicked);
            // 
            // Code_button
            // 
            this.Code_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Code")));
            this.Code_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Code_button.Location = new System.Drawing.Point(142, 9);
            this.Code_button.Name = "Code_button";
            this.Code_button.Size = new System.Drawing.Size(30, 30);
            this.Code_button.TabIndex = 5;
            this.Code_button.TabStop = false;
            this.Code_button.Click += new System.EventHandler(this.FormattingClicked);
            // 
            // Italics_button
            // 
            this.Italics_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Italics")));
            this.Italics_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Italics_button.Location = new System.Drawing.Point(72, 9);
            this.Italics_button.Name = "Italics_button";
            this.Italics_button.Size = new System.Drawing.Size(30, 30);
            this.Italics_button.TabIndex = 4;
            this.Italics_button.TabStop = false;
            this.Italics_button.Click += new System.EventHandler(this.FormattingClicked);
            // 
            // Bold_button
            // 
            this.Bold_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Bold")));
            this.Bold_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Bold_button.Location = new System.Drawing.Point(37, 9);
            this.Bold_button.Name = "Bold_button";
            this.Bold_button.Size = new System.Drawing.Size(30, 30);
            this.Bold_button.TabIndex = 3;
            this.Bold_button.TabStop = false;
            this.Bold_button.Click += new System.EventHandler(this.FormattingClicked);
            // 
            // Strikethrough_button
            // 
            this.Strikethrough_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Strikethrough")));
            this.Strikethrough_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Strikethrough_button.Location = new System.Drawing.Point(2, 9);
            this.Strikethrough_button.Name = "Strikethrough_button";
            this.Strikethrough_button.Size = new System.Drawing.Size(30, 30);
            this.Strikethrough_button.TabIndex = 2;
            this.Strikethrough_button.TabStop = false;
            this.Strikethrough_button.Click += new System.EventHandler(this.FormattingClicked);
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
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(412, 450);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.backgroundPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel backgroundPanel;
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
    }
}

