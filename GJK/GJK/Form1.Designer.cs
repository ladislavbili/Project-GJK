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
            this.save_objects_btn = new System.Windows.Forms.Button();
            this.finish_object_btn = new System.Windows.Forms.Button();
            this.create_objects_btn = new System.Windows.Forms.Button();
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 506);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button load_objects_btn;
        private System.Windows.Forms.GroupBox group_create_objects;
        private System.Windows.Forms.Button save_objects_btn;
        private System.Windows.Forms.Button finish_object_btn;
        private System.Windows.Forms.Button create_objects_btn;
    }
}

