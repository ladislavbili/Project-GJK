namespace GJK {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.load_objects_btn = new System.Windows.Forms.Button();
            this.group_create_objects = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.save_objects_btn = new System.Windows.Forms.Button();
            this.finish_object_btn = new System.Windows.Forms.Button();
            this.create_objects_btn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.group_create_objects.SuspendLayout();
            this.SuspendLayout();
            // 
            // load_objects_btn
            // 
            this.load_objects_btn.Location = new System.Drawing.Point(6, 19);
            this.load_objects_btn.Name = "load_objects_btn";
            this.load_objects_btn.Size = new System.Drawing.Size(92, 30);
            this.load_objects_btn.TabIndex = 0;
            this.load_objects_btn.Text = "Load From File";
            this.load_objects_btn.UseVisualStyleBackColor = true;
            this.load_objects_btn.Click += new System.EventHandler(this.load_objects_btn_Click);
            // 
            // group_create_objects
            // 
            this.group_create_objects.Controls.Add(this.label2);
            this.group_create_objects.Controls.Add(this.button1);
            this.group_create_objects.Controls.Add(this.label1);
            this.group_create_objects.Controls.Add(this.save_objects_btn);
            this.group_create_objects.Controls.Add(this.finish_object_btn);
            this.group_create_objects.Controls.Add(this.create_objects_btn);
            this.group_create_objects.Controls.Add(this.load_objects_btn);
            this.group_create_objects.Location = new System.Drawing.Point(12, 12);
            this.group_create_objects.Name = "group_create_objects";
            this.group_create_objects.Size = new System.Drawing.Size(228, 137);
            this.group_create_objects.TabIndex = 1;
            this.group_create_objects.TabStop = false;
            this.group_create_objects.Text = "Create Objects";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "X: ";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(6, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 32);
            this.button1.TabIndex = 3;
            this.button1.Text = "Test GJK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Y: ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // save_objects_btn
            // 
            this.save_objects_btn.Enabled = false;
            this.save_objects_btn.Location = new System.Drawing.Point(115, 93);
            this.save_objects_btn.Name = "save_objects_btn";
            this.save_objects_btn.Size = new System.Drawing.Size(94, 32);
            this.save_objects_btn.TabIndex = 2;
            this.save_objects_btn.Text = "Save To File";
            this.save_objects_btn.UseVisualStyleBackColor = true;
            this.save_objects_btn.Click += new System.EventHandler(this.save_objects_btn_Click);
            // 
            // finish_object_btn
            // 
            this.finish_object_btn.Enabled = false;
            this.finish_object_btn.Location = new System.Drawing.Point(115, 55);
            this.finish_object_btn.Name = "finish_object_btn";
            this.finish_object_btn.Size = new System.Drawing.Size(94, 32);
            this.finish_object_btn.TabIndex = 2;
            this.finish_object_btn.Text = "Finish Object";
            this.finish_object_btn.UseVisualStyleBackColor = true;
            this.finish_object_btn.Click += new System.EventHandler(this.finish_object_btn_Click);
            // 
            // create_objects_btn
            // 
            this.create_objects_btn.Location = new System.Drawing.Point(115, 19);
            this.create_objects_btn.Name = "create_objects_btn";
            this.create_objects_btn.Size = new System.Drawing.Size(94, 30);
            this.create_objects_btn.TabIndex = 2;
            this.create_objects_btn.Text = "Create Objects";
            this.create_objects_btn.UseVisualStyleBackColor = true;
            this.create_objects_btn.Click += new System.EventHandler(this.create_objects_btn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "All:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1281, 630);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.group_create_objects);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Click += new System.EventHandler(this.Form1_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.group_create_objects.ResumeLayout(false);
            this.group_create_objects.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button load_objects_btn;
        private System.Windows.Forms.GroupBox group_create_objects;
        private System.Windows.Forms.Button save_objects_btn;
        private System.Windows.Forms.Button finish_object_btn;
        private System.Windows.Forms.Button create_objects_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

