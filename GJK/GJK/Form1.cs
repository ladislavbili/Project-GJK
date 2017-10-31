using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using CanvasPoint = System.Drawing.Point;

namespace GJK {
    public partial class Form1 : Form {

        public const string objects_file = "objects.txt";
        public List<Polyline> objects = new List<Polyline>();
        public bool creating = false;
        public bool moving = false;
        public Polyline moving_obj;
        CanvasPoint last_move_position;

        public class Polyline {
            public List<CanvasPoint> points;
            public Color color;
            public bool finished;

            public Polyline(List<CanvasPoint> p, Color c, bool f) {
                points = p;
                color = c;
                finished = f;
            }
            public Polyline() {
                points = new List<CanvasPoint>();
                color = Color.Black;
                finished = false;
            }

            public void Draw(Graphics g) {
                if (finished) {
                    g.DrawPolygon(new Pen(color, 3), points.ToArray());
                    g.FillPolygon(new SolidBrush(Color.Yellow), points.ToArray());
                }
                else {
                    if (points.Count > 1) {
                        foreach (CanvasPoint point in points) {
                            g.DrawLines(new Pen(color, 3), points.ToArray());
                        }
                    }
                }
                foreach (CanvasPoint point in points) {
                    g.FillEllipse(new SolidBrush(Color.Red), new Rectangle(point.X - 5, point.Y - 5, 10, 10));
                }
            }

            public void Move(int x, int y) {
                for (int i = 0; i < points.Count(); i++) {
                    CanvasPoint new_point = new CanvasPoint(points[i].X - x, points[i].Y - y);
                    points[i] = new_point;
                }
            }

            public void Add(CanvasPoint p) {
                points.Add(p);
            }
        }

        public Form1() {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e) {
            if (!File.Exists(objects_file) || new FileInfo(objects_file).Length == 0) {
                load_objects_btn.Enabled = false;
            }
        }

        private void load_objects_btn_Click(object sender, EventArgs e) {  // TODO test
            create_objects_btn.Enabled = false;
            load_objects_btn.Enabled = false;
            var lines = File.ReadAllLines(objects_file);
            objects.Add(new Polyline());
            objects.Last().finished = true;
            foreach (var line in lines) { // iterate over positions
                if (line == String.Empty) { // Go to second object
                    if (objects.Count == 2) {
                        break;
                    }
                    objects.Add(new Polyline());
                    objects.Last().finished = true;
                }
                else { // add position to polyline
                    var values = line.Split(' ').Select(Int32.Parse).ToList();
                    Polyline polyline = objects.Last();
                    polyline.Add(new CanvasPoint(values.First(), values.Last()));
                }
            }
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e) {
            foreach (Polyline polyline in objects) {
                polyline.Draw(e.Graphics);
            }
        }

        private void create_objects_btn_Click(object sender, EventArgs e) {
            create_objects_btn.Enabled = false;
            load_objects_btn.Enabled = false;
            finish_object_btn.Enabled = true;
            creating = true;
        }

        private void Form1_Click(object sender, EventArgs e) {
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e) {
            if (creating) {
                if (objects.Count == 0) {
                    objects.Add(new Polyline());
                }
                Polyline obj = objects.Last();
                obj.Add(new CanvasPoint(e.X, e.Y));
                Invalidate();
            }
        }

        private void finish_object_btn_Click(object sender, EventArgs e) {
            objects.Last().finished = true;
            if (objects.Count == 2) {
                creating = false;
                finish_object_btn.Enabled = false;
            }
            else {
                objects.Add(new Polyline());
            }
            save_objects_btn.Enabled = true;
            Invalidate();
        }

        private void save_objects_btn_Click(object sender, EventArgs e) {
            save_objects_btn.Enabled = false;
            using (FileStream fs = File.Open(objects_file, FileMode.Create)) {
                foreach (Polyline polyline in objects) {
                    string data = "";
                    foreach (CanvasPoint point in polyline.points) {
                        data += point.X + " " + point.Y + Environment.NewLine;
                    }
                    data += Environment.NewLine;
                    byte[] info = new UTF8Encoding(true).GetBytes(data);
                    fs.Write(info, 0, info.Length);
                }
            }
        }

        public static bool IsPointInPolygon(PointF[] polygon, PointF testPoint) {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++) {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y) {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X) {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e) {
            moving = false;
            moving_obj = null;
            foreach (Polyline polyline in objects) {
                if (IsPointInPolygon(Array.ConvertAll(polyline.points.ToArray(), item => (PointF)item), new PointF(e.X, e.Y))) {
                    moving_obj = polyline;
                    moving = true;
                    last_move_position = new CanvasPoint(e.X, e.Y);
                }
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e) {
            moving = false;
            moving_obj = null;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e) {
            if (moving) {
                int x_diff = last_move_position.X - e.X;
                int y_diff = last_move_position.Y - e.Y;
                last_move_position.X = e.X;
                last_move_position.Y = e.Y;
                moving_obj.Move(x_diff, y_diff);
                Invalidate();
            }
        }

        public Vector ProximityGJK(Polyline A, Polyline B, Simplex W) {
            return new Vector();
        }

        public Vector ClosestPoint(Simplex W) {  // TODO test
            Vector d = new Vector();
            if (W.count >= 2) {
                d = W.B.vec - W.A.vec;
            }
            double n = 0;
            if (W.count == 3) {
                n = (W.B.vec - W.A.vec) * (W.C.vec - W.A.vec);
            }
            switch (W.count) {
                case 0:
                    return new Vector(0, 0);
                case 1:
                    return W.A.vec;
                case 2:
                    return W.A.vec - (((d * W.A.vec) / (d * d)) * d);
                case 3:
                    return ((n * W.A.vec) / (n * n)) * n;
            }
            return new Vector(0, 0);
        }
    }

    public class Simplex {
        public SimplexVertex A;
        public SimplexVertex B;
        public SimplexVertex C;
        public int count;
    }

    public class SimplexVertex {
        public Vector vec;
        public int index;
    }
}
