using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Core
{
    public class Line
    {
        public float A { get; private set; }
        public float B { get; private set; }
        public float C { get; private set; }

        public Line(Point first, Point second)
        {
            A = second.Y - first.Y;                              
            B = first.X - second.X;
            C = -first.X * second.Y + first.Y * second.X;
        }

        public Line(float a, float b, float c)
        {
            A = a;
            B = b;
            C = c;
        }

        public static Point GetIntersectionPoint(Line a, Line b)
        {
            var x = (a.B * b.C - b.B * a.C) / (a.A * b.B - b.A * a.B);
            var y = (b.A * a.C - a.A * b.C) / (a.A * b.B - b.A * a.B);
            
            return new Point(x, y);
        }

        public static bool Intersect(Line a, Line b)
        {
            if (a.A == 0 && a.B == 0 || b.A == 0 && b.B == 0) return false;
            if ((a.A * b.B - b.A * a.B) == 0) return false;
            
            return true;
        }

        public static float GetAngleBetween(Line a, Line b)
        {
            var numerator = a.A * b.A + a.B * b.B;
            var denominator = Mathf.Sqrt(a.A * a.A + a.B * a.B) * Mathf.Sqrt(b.A * b.A + b.B * b.B);
            
            return Mathf.Acos(numerator / denominator) * Mathf.Rad2Deg;
        }

        public static Line GetPerpendicularLine(Line a, Point p)
        {
            return new Line(-a.B, a.A, (a.B * p.X - a.A * p.Y));
        }

        public static Line GetParallelLine(Line a, Point p)
        {
            return new Line(a.A, a.B, -(a.A * p.X + a.B * p.Y));
        }
    }

    public class Point
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2 ToVector()
        {
            return new Vector2(this.X, this.Y);
        }

        public static float Distance(Point a, Point b)
        {
            return Mathf.Sqrt(Mathf.Pow(b.X - a.X, 2) + Mathf.Pow(b.Y - a.Y, 2));
        }
    }
}