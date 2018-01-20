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
        /// List of interactive objects in form of polylines created from objects defined in <see cref="objects_file"/>
        /// </summary>
        public List<Polyline> objects = new List<Polyline>();

        public class Pair<T1, T2> {
            public T1 First { get; set; }
            public T2 Second { get; set; }
        }
        /// <summary>
        ///  Struct make for ProximityGJK return value, which needs to return multiple values.
        /// </summary>
        public struct GJKOutput {
            public bool Collision;
            public double Distance;
            public Pair<Vector, Vector> closestFeatures;
            public Vector touchingVector;
            public Pair<Vector, Vector> touchingVectorPoints;
        }

        public bool connect = false;

        /// <summary>
        /// Closest Points on both geometries, to be connected if <see cref="connect"/> is true
        /// </summary>
        public Vector connectedPointA = new Vector();
        public Vector connectedPointB = new Vector();

        public Vector connectedTouchingPointA = new Vector();
        public Vector connectedTouchingPointB = new Vector();

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
            if (!connect) {
                Pen p1 = new Pen(Color.Green, 3);
                Pen p2 = new Pen(Color.Blue, 3);
                connectPoints(e.Graphics, connectedPointA, connectedPointB, p1);
                //connectPoints(e.Graphics, connectedTouchingPointA, connectedTouchingPointB, p2);
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
            if (objects.Count == 2 && objects[0].finished && objects[1].finished) {
                Simplex s = new Simplex();
                s.count = 0;
                GJKOutput result = ProximityGJK(objects[0], objects[1], s);
                if (result.Collision) {
                    collisionValueLabel.Text = "Yes";
                    distanceValueLabel.Text = result.Distance.ToString("F4");
                    closestFeaturesValueALabel.Text = "A: -";
                    closestFeaturesValueBLabel.Text = "B: -";
                    connect = true;

                }
                else {
                    collisionValueLabel.Text = "No";
                    distanceValueLabel.Text = result.Distance.ToString("F4");
                    string A = "(" + result.closestFeatures.First.X.ToString("F2") + ", " + result.closestFeatures.First.Y.ToString("F2") + ")";
                    string B = "(" + result.closestFeatures.Second.X.ToString("F2") + ", " + result.closestFeatures.Second.Y.ToString("F2") + ")";
                    closestFeaturesValueALabel.Text = "A: " + A;
                    closestFeaturesValueBLabel.Text = "B: " + B;
                    connect = false;
                    connectedPointA = result.closestFeatures.First;
                    connectedPointB = result.closestFeatures.Second;
                    connectedTouchingPointA = result.touchingVectorPoints.First;
                    connectedTouchingPointB = result.touchingVectorPoints.Second;
                }
            }

            Invalidate();
        }

        /// <summary>
        /// Draws dotted line between two points, used for drawing of closest points.
        /// </summary>
        /// <param name="g"></param>
        private void connectPoints(Graphics g, Vector A, Vector B, Pen p) {
            CanvasPoint a = new CanvasPoint((float)A.X, (float)A.Y);
            CanvasPoint b = new CanvasPoint((float)B.X, (float)B.Y);
            p.DashStyle = DashStyle.Dash;
            g.DrawLine(p, a, b);
            g.FillEllipse(new SolidBrush(p.Color), new Rectangle((int)Math.Round(a.X) - 5, (int)Math.Round(a.Y) - 5, 10, 10));
            g.FillEllipse(new SolidBrush(p.Color), new Rectangle((int)Math.Round(b.X) - 5, (int)Math.Round(b.Y) - 5, 10, 10));
        }

        /// <summary>
        /// Checks for colision between <paramref name="A"/> and <paramref name="B"/>
        /// </summary>
        /// <param name="A">Convex object</param>
        /// <param name="B">Convex object</param>
        /// <param name="W">Initial simplex</param>
        public GJKOutput ProximityGJK(Polyline A, Polyline B, Simplex W) {
            Vector v = new Vector(1, 0);
            GJKOutput result;
            double delta = 0;
            Vector w = new Vector(A.points.First().X, A.points.First().Y);
            Simplex old = new Simplex();
            int i = 0;
            bool start = true;

            while (Vector.Multiply(v, v) - delta > 0.1 || (v.X == 0 && v.Y == 0)) {
                i++;
                if (!start) {
                    v = ClosestPoint(W);
                }
                Vector supportA = SupportHC(A, v);
                Vector supportB = SupportHC(B, -v);

                w = supportA - supportB;
                ShapePoint sp = new ShapePoint();
                sp.S1 = supportA;
                sp.S2 = supportB;
                sp.MinkowskiPoint = w;
                W = BestSimplex(W, sp);

                if (W.count == 3 || (w.X == 0 && w.Y == 0) || (i > 10000 && old == W)) {
                    Pair<Vector, double> epa = computeEPA(A, B, W);
                    result.Collision = true;
                    result.Distance = epa.Second;

                    result.closestFeatures = new Pair<Vector, Vector>();
                    result.touchingVector = new Vector();
                    result.touchingVectorPoints = new Pair<Vector, Vector>();
                    return result;
                }
                if (Vector.Multiply(v, w) > 0 && !start) {
                    delta = Math.Max(delta, ((Vector.Multiply(v, w) * Vector.Multiply(v, w)) / Vector.Multiply(v, v)));
                }
                start = false;
                old = W;
            }
            result.Collision = false;
            result.Distance = v.Length;
            result.closestFeatures = getClosestFeatures(W);
            result.touchingVector = w;
            result.touchingVectorPoints = getTouchingVectorPoints(W, w);
            return result; 
        }

        /// <summary>
        /// (Expanding Polytope Algorithm) Used to compute penetration depth and normal vector of closest edge to origin.
        /// </summary>
        /// <param name="A">Convex object</param>
        /// <param name="B">Convex object</param>
        /// <param name="W">Initial simplex</param>
        public Pair<Vector, double> computeEPA(Polyline A, Polyline B, Simplex W) {

            int winding = 0;
            double a = (W.B.MinkowskiPoint.X - W.A.MinkowskiPoint.X) * (W.B.MinkowskiPoint.Y + W.A.MinkowskiPoint.Y);
            double b = (W.C.MinkowskiPoint.X - W.B.MinkowskiPoint.X) * (W.C.MinkowskiPoint.Y + W.B.MinkowskiPoint.Y);
            double c = (W.A.MinkowskiPoint.X - W.C.MinkowskiPoint.X) * (W.A.MinkowskiPoint.Y + W.C.MinkowskiPoint.Y);
            double sumOverEdges = a + b + c;
            if (sumOverEdges < 0) {
                winding = 1;
            }

            List<Vector> simplex = new List<Vector>();
            simplex.Add(W.A.MinkowskiPoint);
            simplex.Add(W.B.MinkowskiPoint);
            simplex.Add(W.C.MinkowskiPoint);
            while (true) {
                Edge e = findClosestEdge(simplex, winding);
                Vector normal = new Vector(-(e.B.Y - e.A.Y), e.B.Y - e.A.Y);
                Vector p = SupportHC(A, -e.normal) - SupportHC(B, e.normal);
                double d = Vector.Multiply(p, e.normal);
                Vector length = new Vector(Math.Abs(e.A.X - e.B.X), Math.Abs(e.A.Y - e.B.Y));
                if (d - e.distance < 0.001) {
                    Pair<Vector, double>  result = new Pair<Vector, double>();
                    result.First = e.normal;
                    result.Second = d;
                    return result;
                }
                else {
                    simplex.Insert(e.index, p);
                }
            }
        }
        /// <summary>
        ///  Obtains the edge closest to the origin on the Minkowski Difference.
        /// </summary>
        /// <param name="simplex"></param>
        /// <param name="winding"></param>
        /// <returns></returns>
        public Edge findClosestEdge(List<Vector> simplex, int winding) {
            Edge closest = new Edge();
            for (int i = 0; i < simplex.Count; i++) {
                int j = i + 1 == simplex.Count ? 0 : i + 1;
                Vector a = simplex[i];
                Vector b = simplex[j];
                Vector e = b - a;
                Vector oa = a;
                Vector n;
                if (winding == 0) {
                    n = new Vector(e.Y, -e.X);
                }
                else {
                    n = new Vector(-e.Y, e.X);
                }
                //Vector n = tripleProduct(e, oa, e);
                n.Normalize();
                double d = Vector.Multiply(n, a);
                if (d < closest.distance) {
                    closest.distance = d;
                    closest.normal = n;
                    closest.index = j;
                }
            }
            return closest;
        }

        /// <summary>
        /// Computes (A x B) x C = B(C.dot(A)) – A(C.dot(B))
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public Vector tripleProduct(Vector A, Vector B, Vector C) {
            double AdotC = Vector.Multiply(A, C);
            double BdotC = Vector.Multiply(B, C);
            Vector left = B * AdotC;
            Vector right = A * BdotC;
            return left - right;
        }

        public Pair<Vector, Vector> getTouchingVectorPoints(Simplex W, Vector w) {
            Pair<Vector, Vector> result = new Pair<Vector, Vector>();
            if (W.A.MinkowskiPoint == w) {
                result.First = W.A.S1;
                result.Second = W.A.S2;
            }
            else if (W.B.MinkowskiPoint == w) {
                result.First = W.B.S1;
                result.Second = W.B.S2;
            }
            return result;
        }


        /// <summary>
        /// Finds closest points on polygons when they are not colliding, using conves combination.
        /// </summary>
        /// <param name="W"></param>
        /// <returns></returns>
        public Pair<Vector, Vector> getClosestFeatures(Simplex W) {
            Pair<Vector, Vector> result = new Pair<Vector, Vector>();

            if (W.count == 1) {
                result.First = W.A.S1;
                result.Second = W.A.S2;
                return result;
            }
            Vector L = W.B.MinkowskiPoint - W.A.MinkowskiPoint;
            double LdotL = Vector.Multiply(L, L);
            double LdotA = Vector.Multiply(L, W.A.MinkowskiPoint);
            double Lambda2 = -LdotA / LdotL;
            double Lambda1 = 1 - Lambda2;

            Vector AClosest = Vector.Multiply(Lambda1, W.A.S1) + Vector.Multiply(Lambda2, W.B.S1);
            Vector BClosest = Vector.Multiply(Lambda1, W.A.S2) + Vector.Multiply(Lambda2, W.B.S2);

            if (Lambda1 < 0) {
                AClosest = W.B.S1;
                BClosest = W.A.S2;
            }
            else if (Lambda2 < 0) {
                AClosest = W.A.S1;
                BClosest = W.B.S2;
            }
            if (L.X == 0 && L.Y == 0) {
                AClosest = W.A.S1;
                BClosest = W.A.S2;
            }

            result.First = AClosest;
            result.Second = BClosest;

            return result;
        }

        /// <summary>
        /// Find the closest point to the origin.
        /// </summary>
        /// <param name="W">Simplex</param>
        /// <returns>Closest point on simplex to origin.</returns>
        public Vector ClosestPoint(Simplex W) {
            Vector d = new Vector();
            if (W.count >= 2) {
                d = W.B.MinkowskiPoint - W.A.MinkowskiPoint;
            }

            switch (W.count) {
                case 0:
                    return new Vector(0, 0);
                case 1:
                    return W.A.MinkowskiPoint;
                case 2:
                    return W.A.MinkowskiPoint - (((d * W.A.MinkowskiPoint) / (d * d)) * d);
                case 3: {
                        Vector3 W1 = new Vector3((float)W.A.MinkowskiPoint.X, (float)W.A.MinkowskiPoint.Y, 0);
                        Vector3 W2 = new Vector3((float)W.B.MinkowskiPoint.X, (float)W.B.MinkowskiPoint.Y, 0);
                        Vector3 W3 = new Vector3((float)W.C.MinkowskiPoint.X, (float)W.C.MinkowskiPoint.Y, 0);
                        Vector3 n = Vector3.Cross((W2 - W1), (W3 - W1));
                        Vector3 result = Vector3.Multiply(((Vector3.Multiply(n, W1)) / (Vector3.Multiply(n, n))), n);
                        return new Vector(result.X, result.Y);
                    }
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
        public Vector SupportHC(Polyline A, Vector d) {
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

        /// <summary>
        /// Finds two neighbours of given vector (point <paramref name="v"/>) on polyline <paramref name="A"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="A"></param>
        /// <returns></returns>
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

        private Vector ClosestToSegment(Vector pt, Vector p1, Vector p2) {
            Vector closest = new Vector();
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            if ((dx == 0) && (dy == 0)) {
                // It's a point not a line segment.
                closest = p1;
                return closest;
            }

            // Calculate the t that minimizes the distance.
            double t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) / (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0) {
                closest = new Vector(p1.X, p1.Y);
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
            }
            else if (t > 1) {
                closest = new Vector(p2.X, p2.Y);
                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;
            }
            else {
                closest = new Vector(p1.X + t * dx, p1.Y + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }

            return closest;
        }

        /// <summary>
        /// Form new simplex and test in which external Voronoi region the origin lies.
        /// </summary>
        /// <param name="W">Simplex</param>
        /// <param name="w">New point in CSO surface</param>
        /// <returns>New smallest simplex <paramref name="W"/> containing <paramref name="w"/> and closest point to origin.</returns>
        public Simplex BestSimplex(Simplex W, ShapePoint w) {
            Simplex result = new Simplex();
            switch (W.count) {
                case 0: {
                        result.A = w;
                        result.count = 1;
                        break;
                    }
                case 1: {
                        Simplex line = new Simplex();
                        line.A = w;
                        line.B = W.A;
                        line.count = 2;
                        Vector closest = ClosestToSegment(new Vector(0, 0), w.MinkowskiPoint, W.A.MinkowskiPoint);
                        if (closest == w.MinkowskiPoint) {
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
                        Vector d = new Vector(0, 0) - w.MinkowskiPoint; // d.negate(); AO = O - A; O = origin (0, 0)
                        Vector e1 = W.A.MinkowskiPoint - w.MinkowskiPoint; // AB = B - A
                        Vector e2 = W.B.MinkowskiPoint - w.MinkowskiPoint;
                        Vector3 e1_3D = new Vector3((float)e1.X, (float)e1.Y, 0);
                        Vector3 e2_3D = new Vector3((float)e2.X, (float)e2.Y, 0);
                        Vector3 u1 = Vector3.Cross(e1_3D, Vector3.Cross(e1_3D, e2_3D));
                        Vector3 v1 = Vector3.Cross(Vector3.Cross(e1_3D, e2_3D), e2_3D);
                        if (Vector.Multiply(d, e1) < 0 && Vector.Multiply(d, e2) < 0) {
                            result.A = w;
                            result.count = 1;
                        }
                        else if (Vector.Multiply(d, e1) > 0 && (d.X * u1.X + d.Y * u1.Y + 0 * u1.Z) > 0) {
                            result.A = W.A;
                            result.B = w;
                            result.count = 2;
                        }
                        else if (Vector.Multiply(d, e2) > 0 && (d.X * v1.X + d.Y * v1.Y + 0 * v1.Z) > 0) {
                            result.A = W.B;
                            result.B = w;
                            result.count = 2;

                        }
                        else if ((d.X * u1.X + d.Y * u1.Y + 0 * u1.Z < 0) && (d.X * v1.X + d.Y * v1.Y + 0 * v1.Z < 0)) {
                            result.A = W.A;
                            result.B = W.B;
                            result.C = w;
                            result.count = 3;
                        }
                        else {
                            result = W;
                        }
                        break;
                    }
            }
            return result;
        }
    }


    /// <summary>
    /// Simplex data structure. Represents <0-2> simplexes.
    /// </summary>
    public class Simplex {
        public ShapePoint A;
        public ShapePoint B;
        public ShapePoint C;
        public int count;

        public Simplex() {
            A = new ShapePoint();
            B = new ShapePoint();
            C = new ShapePoint();
        }
    }

    public class ShapePoint {
        public Vector S1;
        public Vector S2;
        public Vector MinkowskiPoint;
    }

    public class Edge {
        public Vector A;
        public Vector B;
        public int index;
        public Vector normal;
        public double distance = Double.MaxValue;
    }
}