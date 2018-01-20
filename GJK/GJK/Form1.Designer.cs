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
            this.infoBox = new System.Windows.Forms.GroupBox();
            this.closestFeaturesValueBLabel = new System.Windows.Forms.Label();
            this.closestFeaturesValueALabel = new System.Windows.Forms.Label();
            this.distanceValueLabel = new System.Windows.Forms.Label();
            this.collisionValueLabel = new System.Windows.Forms.Label();
            this.closestFeaturesLabel = new System.Windows.Forms.Label();
            this.distanceLabel = new System.Windows.Forms.Label();
            this.collisionLabel = new System.Windows.Forms.Label();
            this.group_create_objects.SuspendLayout();
            this.infoBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // load_objects_btn
            // 
            this.load_objects_btn.Location = new System.Drawing.Point(30, 19);
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
            this.group_create_objects.Size = new System.Drawing.Size(273, 102);
            this.group_create_objects.TabIndex = 1;
            this.group_create_objects.TabStop = false;
            this.group_create_objects.Text = "Create Objects";
            // 
            // save_objects_btn
            // 
            this.save_objects_btn.Enabled = false;
            this.save_objects_btn.Location = new System.Drawing.Point(30, 55);
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
            this.finish_object_btn.Location = new System.Drawing.Point(143, 55);
            this.finish_object_btn.Name = "finish_object_btn";
            this.finish_object_btn.Size = new System.Drawing.Size(94, 32);
            this.finish_object_btn.TabIndex = 2;
            this.finish_object_btn.Text = "Finish Object";
            this.finish_object_btn.UseVisualStyleBackColor = true;
            this.finish_object_btn.Click += new System.EventHandler(this.finish_object_btn_Click);
            // 
            // create_objects_btn
            // 
            this.create_objects_btn.Location = new System.Drawing.Point(143, 19);
            this.create_objects_btn.Name = "create_objects_btn";
            this.create_objects_btn.Size = new System.Drawing.Size(94, 30);
            this.create_objects_btn.TabIndex = 2;
            this.create_objects_btn.Text = "Create Objects";
            this.create_objects_btn.UseVisualStyleBackColor = true;
            this.create_objects_btn.Click += new System.EventHandler(this.create_objects_btn_Click);
            // 
            // infoBox
            // 
            this.infoBox.Controls.Add(this.closestFeaturesValueBLabel);
            this.infoBox.Controls.Add(this.closestFeaturesValueALabel);
            this.infoBox.Controls.Add(this.distanceValueLabel);
            this.infoBox.Controls.Add(this.collisionValueLabel);
            this.infoBox.Controls.Add(this.closestFeaturesLabel);
            this.infoBox.Controls.Add(this.distanceLabel);
            this.infoBox.Controls.Add(this.collisionLabel);
            this.infoBox.Location = new System.Drawing.Point(12, 120);
            this.infoBox.Name = "infoBox";
            this.infoBox.Size = new System.Drawing.Size(273, 143);
            this.infoBox.TabIndex = 2;
            this.infoBox.TabStop = false;
            this.infoBox.Text = "Information";
            // 
            // closestFeaturesValueBLabel
            // 
            this.closestFeaturesValueBLabel.AutoSize = true;
            this.closestFeaturesValueBLabel.Location = new System.Drawing.Point(140, 100);
            this.closestFeaturesValueBLabel.Name = "closestFeaturesValueBLabel";
            this.closestFeaturesValueBLabel.Size = new System.Drawing.Size(10, 13);
            this.closestFeaturesValueBLabel.TabIndex = 6;
            this.closestFeaturesValueBLabel.Text = "-";
            // 
            // closestFeaturesValueALabel
            // 
            this.closestFeaturesValueALabel.AutoSize = true;
            this.closestFeaturesValueALabel.Location = new System.Drawing.Point(140, 74);
            this.closestFeaturesValueALabel.Name = "closestFeaturesValueALabel";
            this.closestFeaturesValueALabel.Size = new System.Drawing.Size(10, 13);
            this.closestFeaturesValueALabel.TabIndex = 5;
            this.closestFeaturesValueALabel.Text = "-";
            // 
            // distanceValueLabel
            // 
            this.distanceValueLabel.AutoSize = true;
            this.distanceValueLabel.Location = new System.Drawing.Point(140, 49);
            this.distanceValueLabel.Name = "distanceValueLabel";
            this.distanceValueLabel.Size = new System.Drawing.Size(10, 13);
            this.distanceValueLabel.TabIndex = 4;
            this.distanceValueLabel.Text = "-";
            // 
            // collisionValueLabel
            // 
            this.collisionValueLabel.AutoSize = true;
            this.collisionValueLabel.Location = new System.Drawing.Point(140, 25);
            this.collisionValueLabel.Name = "collisionValueLabel";
            this.collisionValueLabel.Size = new System.Drawing.Size(10, 13);
            this.collisionValueLabel.TabIndex = 3;
            this.collisionValueLabel.Text = "-";
            // 
            // closestFeaturesLabel
            // 
            this.closestFeaturesLabel.AutoSize = true;
            this.closestFeaturesLabel.Location = new System.Drawing.Point(6, 74);
            this.closestFeaturesLabel.Name = "closestFeaturesLabel";
            this.closestFeaturesLabel.Size = new System.Drawing.Size(76, 13);
            this.closestFeaturesLabel.TabIndex = 2;
            this.closestFeaturesLabel.Text = "Closest Points:";
            // 
            // distanceLabel
            // 
            this.distanceLabel.AutoSize = true;
            this.distanceLabel.Location = new System.Drawing.Point(6, 49);
            this.distanceLabel.Name = "distanceLabel";
            this.distanceLabel.Size = new System.Drawing.Size(55, 13);
            this.distanceLabel.TabIndex = 1;
            this.distanceLabel.Text = "Distance: ";
            // 
            // collisionLabel
            // 
            this.collisionLabel.AutoSize = true;
            this.collisionLabel.Location = new System.Drawing.Point(6, 25);
            this.collisionLabel.Name = "collisionLabel";
            this.collisionLabel.Size = new System.Drawing.Size(51, 13);
            this.collisionLabel.TabIndex = 0;
            this.collisionLabel.Text = "Collision: ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1281, 630);
            this.Controls.Add(this.infoBox);
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
            this.infoBox.ResumeLayout(false);
            this.infoBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button load_objects_btn;
        private System.Windows.Forms.GroupBox group_create_objects;
        private System.Windows.Forms.Button save_objects_btn;
        private System.Windows.Forms.Button finish_object_btn;
        private System.Windows.Forms.Button create_objects_btn;
        private System.Windows.Forms.GroupBox infoBox;
        private System.Windows.Forms.Label closestFeaturesValueALabel;
        private System.Windows.Forms.Label distanceValueLabel;
        private System.Windows.Forms.Label collisionValueLabel;
        private System.Windows.Forms.Label closestFeaturesLabel;
        private System.Windows.Forms.Label distanceLabel;
        private System.Windows.Forms.Label collisionLabel;
        private System.Windows.Forms.Label closestFeaturesValueBLabel;
    }
}

