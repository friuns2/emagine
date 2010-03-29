using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace EmagineClient
{

    public enum ScreenObjectType
    {
        Line,
        Ellipse,
        Text,
        Shape,
        Rectangle
    }

    public class ScreenObject
    {
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
        public double Thickness { get; set; }
        public string StrokeColor { get; set; }
        public string FillColor { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public ScreenObjectType Type { get; set; }
        public string Text;
    }
}
