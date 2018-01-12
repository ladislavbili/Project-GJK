using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;
using System.Windows;
using System.Windows.Forms;
using CanvasPoint = System.Drawing.PointF;

namespace GJK {
    public partial class Form1 : Form {

        /// <summary>
        /// Path to file containing definitions of objects as lines of pairs of points
        /// </summary>
        public const string objects_file = "objects.txt";
        /// <summary>
        /// List of interacitive objects in form of polylines created from objects defined in <see cref="objects_file"/>
        /// </summary>
        public List<Polyline> objects = new List<Polyline>();
        /// <summary>
        /// If true, then app is in state of creating new objects using click events
        /// </summary>
        public bool creating = false;
        /// <summary>
        /// If true, then app is in state of moving clicked object
        /// </summary>
        public bool moving = false;
        /// <summary>
        /// Currently clicked objects.
        /// </summary>
        public Polyline moving_obj;
        /// <summary>
        /// last position of objects' point, used for moving of objects
        /// </summary>
        CanvasPoint last_move_position;


        /// <summary>
        /// If true, then app is in state of rotating objects using click events
        /// </summary>
        public bool rotating = false;
        /// <summary>
        /// Currently clicked objects.
        /// </summary>
        public Polyline rotating_obj;
        /// <summary>
        /// Last position of objects' point, used for rotation of objects
        /// </summary>
        CanvasPoint last_rotate_click;

        /// <summary>
        /// Class defining objects in form of list of points
        /// </summary>
        public class Polyline {
            /// <summary>
            /// List of points that comprise the objects
            /// </summary>
            public List<CanvasPoint> points;
            /// <summary>
            /// Color used to fill objects if it finised <see cref="finished"/>
            /// </summary>
            public Color color;
            /// <summary>
            /// Defines wheter the creation of object is finished, if yes, then last point will be connected with first when drawn.
            /// </summary>
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

            /// <summary>
            /// Draws object onto canvas. Filled with <see cref="color"/> and with last and first point connected if <see cref="finished"/> 
            /// </summary>
            /// <param name="g"></param>
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
                    g.FillEllipse(new SolidBrush(Color.Red), new Rectangle((int)Math.Round(point.X) - 5, (int)Math.Round(point.Y) - 5, 10, 10));
                }
            }

            /// <summary>
            /// Moves each point of object by <paramref name="x"/> and <paramref name="y"/>
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public void Move(int x, int y) {
                for (int i = 0; i < points.Count(); i++) {
                    CanvasPoint new_point = new CanvasPoint(points[i].X - x, points[i].Y - y);
                    points[i] = new_point;
                }
            }

            /// <summary>
            /// Finds center of object.
            /// </summary>
            /// <returns>Center of object.</returns>
            public CanvasPoint GetCenter() {
                int totalX = 0;
                int totalY = 0;
                foreach (CanvasPoint p in points) {
                    totalX += (int)Math.Round(p.X);
                    totalY += (int)Math.Round(p.Y);
                }
                int centerX = totalX / points.Count;
                int centerY = totalY / points.Count;
                return new CanvasPoint(centerX, centerY);
            }

            /// <summary>
            /// Rotates object by <paramref name="angle"/> (moves each point).
            /// </summary>
            /// <param name="angle"></param>
            public void Rotate(double angle) {
                Matrix myMatrix = new Matrix();
                myMatrix.RotateAt((float)angle, GetCenter());
                CanvasPoint[] p = points.ToArray();
                myMatrix.TransformPoints(p);
                points = p.ToList();
            }

            /// <summary>
            /// Adds new point to unfinished object.
            /// </summary>
            /// <param name="p"></param>
            public void Add(CanvasPoint p) {
                points.Add(p);
            }
        }

        public Form1() {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Upon form load event: checks whether objects are available to be loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e) {
            if (!File.Exists(objects_file) || new FileInfo(objects_file).Length == 0) {
                load_objects_btn.Enabled = false;
            }
        }

        /// <summary>
        /// When <see cref="load_objects_btn"/> is clicked, loads objects present in <see cref="objects_file"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void load_objects_btn_Click(object sender, EventArgs e) {  // TODO test
            create_objects_btn.Enabled = false;
            load_objects_btn.Enabled = false;
            button1.Enabled = true;
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
                    var values = line.Split(' ').Select(Double.Parse).ToList();
                    Polyline polyline = objects.Last();
                    polyline.Add(new CanvasPoint((float)values.First(), (float)values.Last()));
                }
            }
            Invalidate();
        }

        /// <summary>
        /// Draws all objects onto canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e) {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            foreach (Polyline polyline in objects) {
                polyline.Draw(e.Graphics);
            }
            if (objects.Count == 2 && objects[0].finished == true && objects[1].finished == true) { // TODO spojit najblizsie body
                CanvasPoint a = objects[0].points[0];
                CanvasPoint b = objects[1].points[0];
                button1.Enabled = true;
                connectPoints(e.Graphics, a, b);
            }
        }

        /// <summary>
        /// Changes state to object creation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void create_objects_btn_Click(object sender, EventArgs e) {
            create_objects_btn.Enabled = false;
            load_objects_btn.Enabled = false;
            finish_object_btn.Enabled = false;
            creating = true;
        }

        private void Form1_Click(object sender, EventArgs e) {
        }


        /// <summary>
        /// If in creating mode: adds new point to current object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseClick(object sender, MouseEventArgs e) {
            if (creating) {
                if (objects.Count == 0) {
                    objects.Add(new Polyline());
                }
                Polyline obj = objects.Last();
                obj.Add(new CanvasPoint(e.X, e.Y));
                if (obj.points.Count > 2) {
                    finish_object_btn.Enabled = true;
                }
                else {
                    finish_object_btn.Enabled = false;
                }
                Invalidate();
            }
        }

        /// <summary>
        /// When <see cref="finish_object_btn"/> is clicked: sets current object and as finished and redraws canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void finish_object_btn_Click(object sender, EventArgs e) {
            objects.Last().finished = true;
            if (objects.Count == 2) {
                creating = false;

                save_objects_btn.Enabled = true;
            }
            else {
                objects.Add(new Polyline());
            }
            Invalidate();
            finish_object_btn.Enabled = false;
        }


        /// <summary>
        /// When <see cref="save_objects_btn"/> is clicked: saves created objects to <see cref="objects_file"/> in form of lines of pairs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        /// <summary>
        /// Checks whether <paramref name="testPoint"/> is in <paramref name="polygon"/>.
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="testPoint"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Prepares for moving or rotating events based on mouse button that was used.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseDown(object sender, MouseEventArgs e) {
            moving = false;
            moving_obj = null;
            rotating = false;
            rotating_obj = null;
            foreach (Polyline polyline in objects) {
                if (IsPointInPolygon(Array.ConvertAll(polyline.points.ToArray(), item => (PointF)item), new PointF(e.X, e.Y))) {
                    if (e.Button == MouseButtons.Left) {
                        moving_obj = polyline;
                        moving = true;
                        last_move_position = new CanvasPoint(e.X, e.Y);
                    }
                    else if (e.Button == MouseButtons.Right) {
                        rotating_obj = polyline;
                        rotating = true;
                        last_rotate_click = new CanvasPoint(e.X, e.Y);
                    }

                }
            }
        }

        /// <summary>
        /// Finishes move of rotation event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseUp(object sender, MouseEventArgs e) {
            if (moving) {
                moving = false;
                moving_obj = null;
            }
            else if (rotating) {
                rotating = false;
                rotating_obj = null;
            }

        }

        /// <summary>
        /// Moves or rotates clicked object, based on mouse button that is being pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseMove(object sender, MouseEventArgs e) {
            if (moving) {
                int x_diff = (int)Math.Round(last_move_position.X) - e.X;
                int y_diff = (int)Math.Round(last_move_position.Y) - e.Y;
                last_move_position.X = e.X;
                last_move_position.Y = e.Y;
                moving_obj.Move(x_diff, y_diff);
            }
            else if (rotating) {
                CanvasPoint sp = new CanvasPoint(last_rotate_click.X, last_rotate_click.Y);
                CanvasPoint mp = new CanvasPoint(rotating_obj.GetCenter().X, rotating_obj.GetCenter().Y);
                CanvasPoint p = new CanvasPoint(e.X, e.Y);

                double sAngle = Math.Atan2((sp.Y - mp.Y), (sp.X - mp.X));
                double pAngle = Math.Atan2((p.Y - mp.Y), (p.X - mp.X));

                last_rotate_click.X = e.X;
                last_rotate_click.Y = e.Y;

                double angle = (pAngle - sAngle) * 180 / Math.PI; ;
                rotating_obj.Rotate(angle);
            }
            Invalidate();
        }

        /// <summary>
        /// Draws dotted line between <paramref name="a"/> and <paramref name="b"/>, used for drawing of closest points.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void connectPoints(Graphics g, CanvasPoint a, CanvasPoint b) {
            Pen p = new Pen(Color.Green, 3);
            p.DashStyle = DashStyle.Dash;
            g.DrawLine(p, a, b);
            g.FillEllipse(new SolidBrush(Color.Green), new Rectangle((int)Math.Round(a.X) - 5, (int)Math.Round(a.Y) - 5, 10, 10));
            g.FillEllipse(new SolidBrush(Color.Green), new Rectangle((int)Math.Round(b.X) - 5, (int)Math.Round(b.Y) - 5, 10, 10));
        }

        /// <summary>
        /// Checks for colision between <paramref name="A"/> and <paramref name="B"/>
        /// </summary>
        /// <param name="A">Convex object</param>
        /// <param name="B">Convex object</param>
        /// <param name="W">Initial simplex</param>
        /// <returns>touching vector</returns>
        public Vector ProximityGJK(Polyline A, Polyline B, Simplex W) {
            Vector v = new Vector(1, 0);
            double delta = 0;

            Vector w = new Vector(A.points.First().X, A.points.First().Y);
            Vector normalizedV = v;
            normalizedV.Normalize();
            while (normalizedV * normalizedV - delta > -0.1) {
                v = ClosestPoint(W);
                normalizedV = v;
                if (normalizedV.X != 0 && normalizedV.Y != 0) {
                    normalizedV.Normalize();
                }
                w = SupportHC(A, v) - SupportHC(B, -v);  // JE v A -v SPRAVNE?
                W = BestSimplex(W, w);  // Namiesto tohoto, proste pridat vertex a ked je dlzky 3, testovat ci je kolizia? "Simplex.contains(ORIGIN)"
                // TODO skusit si nakreslit BestSimplex s konkretnymi bodmi a odkrokovat...
                if (Vector.Multiply(v, w) > 0) {
                    delta = Math.Max(delta, ((Vector.Multiply(v, w) * Vector.Multiply(v, w)) / (normalizedV * normalizedV)));
                }
                label3.Text = label3.Text + w.X.ToString() + "," + w.Y.ToString() + " | ";
            }
            label1.Text = "X: " + w.X.ToString();
            label2.Text = "Y: " + w.Y.ToString();
            return w;
        }

        /// <summary>
        /// Find the closest point to the origin.
        /// </summary>
        /// <param name="W">Simplex</param>
        /// <returns>Closest point on simplex to origin.</returns>
        public Vector ClosestPoint(Simplex W) {
            Vector d = new Vector();
            if (W.count >= 2) {
                d = W.B - W.A;
            }

            switch (W.count) {
                case 0:
                    return new Vector(0, 0);
                case 1:
                    return W.A;
                case 2:
                    return W.A - (((d * W.A) / (d * d)) * d);
            }
            return new Vector(0, 0);
        }

        /// <summary>
        /// Hill Climbing support funtion for GJK algorithm.
        ///  For convex polytopes do a local search to “refine” the support point from previous simulation state.
        /// </summary>
        /// <param name="A">Convex polytype</param>
        /// <param name="d">Direction vector</param>
        /// <param name="w">Initial support vertex</param>
        /// <returns>New support vertex with minimal projection <paramref name="w"/></returns>
        public Vector SupportHC(Polyline A, Vector d) {  // TODO pamatat si visited?
            Vector w = new Vector(A.points.First().X, A.points.First().Y);
            double u = Vector.Multiply(d, w);
            bool found = false;
            while (!found) {
                found = true;
                List<CanvasPoint> neighbours = getNeighbours(w, A);
                foreach (var neighbour in neighbours) {
                    if (d.X * neighbour.X + d.Y * neighbour.Y < u) {
                        found = false;
                        u = d.X * neighbour.X + d.Y * neighbour.Y;
                        w = new Vector(neighbour.X, neighbour.Y);
                        break;
                    }
                }
            }
            return w;
        }


        public List<CanvasPoint> getNeighbours(Vector v, Polyline A) {
            CanvasPoint first = A.points.First();
            CanvasPoint last = A.points.Last();
            List<CanvasPoint> result = new List<CanvasPoint>();
            if (first.X == v.X && first.Y == v.Y) {
                result.Add(A.points[1]);
                result.Add(A.points.Last());
            }
            else if (last.X == v.X && last.Y == v.Y) {
                result.Add(A.points[A.points.Count - 2]);
                result.Add(A.points.First());
            }
            else {
                int index = A.points.FindIndex(point => point.X == v.X && point.Y == v.Y);
                result.Add(A.points[index - 1]);
                result.Add(A.points[index + 1]);
            }
            return result;
        }

        /// <summary>
        /// Form new simplex and test in which external Voronoi region the origin lies.
        /// </summary>
        /// <param name="W">Simplex</param>
        /// <param name="w">New point in CSO surface</param>
        /// <returns>New smallest simplex <paramref name="W"/> containing <paramref name="w"/> and closest point to origin.</returns>
        public Simplex BestSimplex(Simplex W, Vector w) {  // TODO test and debug this shit
            Simplex result = new Simplex();
            Vector d = w;
            Vector e1 = W.A - w;
            Vector e2 = W.B - w;
            d.Negate();
            switch (W.count) {
                case 0: {
                        result.A = w;
                        result.count = 1;
                        break;
                    }
                case 1: {
                        if ((Vector.Multiply(d, e1)) > 0) {
                            result.A = w;
                            result.count = 1;
                        }
                        else {
                            result.A = W.A;
                            result.B = w;
                            result.count = 2;
                        }
                        break;
                    }
                case 2: {
                        Vector3 e1_3D = new Vector3((float)e1.X, (float)e1.Y, 0);
                        Vector3 e2_3D = new Vector3((float)e2.X, (float)e2.Y, 0);
                        Vector3 u1 = Vector3.Cross(e1_3D, Vector3.Cross(e1_3D, e2_3D));
                        Vector3 v1 = Vector3.Cross(Vector3.Cross(e1_3D, e2_3D), e2_3D);
                        if ((d.X * e1.X + d.Y * e1.Y) < 0 && (d.X * e2.X + d.Y * e2.Y) < 0) {
                            result.A = w;
                            result.count = 1;
                        }
                        else if ((d.X * e1.X + d.Y * e1.Y) > 0 && (d.X * u1.X + d.Y * u1.Y + 0 * u1.Z) > 0) {
                            result.A = W.A;
                            result.B = w;
                            result.count = 2;
                        }
                        else if ((d.X * e2.X + d.Y * e2.Y) > 0 && (d.X * v1.X + d.Y * v1.Y + 0 * v1.Z) > 0) {
                            result.A = W.B;
                            result.B = w;
                            result.count = 2;

                        }
                        else if ((d.X * u1.X + d.Y * u1.Y + 0 * u1.Z < 0) && (d.X * v1.X + d.Y * v1.Y + 0 * v1.Z < 0)) {
                            result.A = W.A;
                            result.B = W.B;
                            result.C = w; // Added
                            result.count = 3;
                        }
                        break;
                    }
            }
            //result.count = 3; //Preco?
            return result;
        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            Simplex s = new Simplex();
            s.count = 0;
            ProximityGJK(objects[0], objects[1], s);
        }

        private void label2_Click(object sender, EventArgs e) {

        }
    }


    /// <summary>
    /// Simplex data structure. Represents <0-2> simplexes.
    /// </summary>
    public class Simplex {
        public Vector A;
        public Vector B;
        public Vector C;
        public int count;
    }
}