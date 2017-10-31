using Microsoft.VisualStudio.TestTools.UnitTesting;
using GJK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace GJK.Tests {
    [TestClass()]
    public class Form1Tests {
        [TestMethod()]
        public void ClosestPointTest() {

            // Line
            SimplexVertex a = new SimplexVertex();
            a.vec = new Vector(-5, 1);
            SimplexVertex b = new SimplexVertex();
            b.vec = new Vector(2, 1);
            Simplex s = new Simplex();
            s.A = a;
            s.B = b;
            s.count = 2;
            Form1 f = new Form1();
            Vector res = f.ClosestPoint(s);
            Assert.AreEqual(res.X, 0);
            Assert.AreEqual(res.Y, 1);

            SimplexVertex a2 = new SimplexVertex();
            a2.vec = new Vector(4, 3);
            SimplexVertex b2 = new SimplexVertex();
            b2.vec = new Vector(1, -2);
            Simplex s2 = new Simplex();
            s2.A = a2;
            s2.B = b2;
            s2.count = 2;
            Vector res2 = f.ClosestPoint(s2);
            Assert.AreEqual(Math.Round(res2.X, 2), 1.62);
            Assert.AreEqual(Math.Round(res2.Y, 2), -0.97);

            // Face, is for 3D? if its for 2D then... its bullshit...
            //SimplexVertex a3 = new SimplexVertex();
            //a3.vec = new Vector(6, 2);
            //SimplexVertex b3 = new SimplexVertex();
            //b3.vec = new Vector(2, 4.5);
            //SimplexVertex c3 = new SimplexVertex();
            //c3.vec = new Vector(4, 5);
            //Simplex s3 = new Simplex();
            //s3.A = a3;
            //s3.B = b3;
            //s3.C = c3;
            //s3.count = 3;
            //Vector res3 = f.ClosestPoint(s3);
            //Assert.AreEqual(Math.Round(res3.X, 2), 1.62);
            //Assert.AreEqual(Math.Round(res3.Y, 2), -0.97);
        }
    }
}