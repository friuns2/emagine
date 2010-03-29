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
using System.Linq;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace EmagineClient
{
    public class DrawingArea
    {
        #region Private Variables

        private Canvas _canvas;
        ToolHelper toolHelper = new ToolHelper();
        List<Shape> tempHolder = new List<Shape>();
        CanvasHelper canHelper = new CanvasHelper();
        Line _virtualLine = new Line() { Visibility = Visibility.Collapsed, Stroke = new SolidColorBrush(Colors.LightGray), StrokeThickness = 2 };
        CurrentTool tool = CurrentTool.Pencil;
        ObservableCollection<TalkItem> messages = new ObservableCollection<TalkItem>();

        #endregion

        #region Properties

        public ObservableCollection<TalkItem> MessageCollection
        {
            get { return messages; }
        }


        /// <summary>
        /// Current tool to perform draw
        /// </summary>
        public CurrentTool Tool
        {
            get { return tool; }
            set { tool = value; }
        }


        /// <summary>
        /// Temporary holder to hold list of objects
        /// </summary>
        public List<Shape> TempHolder
        {
            get { return tempHolder; }
            set { tempHolder = value; }
        }


        /// <summary>
        /// Previous point
        /// </summary>
        public Point PrevPoint 
        {
           get;set;
        }

        /// <summary>
        /// Starting Point
        /// </summary>
        public Point StartPoint 
        {
           get;set;
        }

        /// <summary>
        /// Stroke color
        /// </summary>
        public SolidColorBrush StrokeColor { get; set; }

        /// <summary>
        /// Stroke width
        /// </summary>
        public double StrokeWidth { get; set; }

        /// <summary>
        /// Filling color
        /// </summary>
        public SolidColorBrush FillColor { get; set; }

        #endregion

        #region Events


        public event Action<string> DataAdded;

        #endregion

        #region Ctor and Methods

        /// <summary>
        /// Drawing area constructor
        /// </summary>
        /// <param name="c"></param>
        public DrawingArea(Canvas c)
        {
            _canvas = c;
            _canvas.Children.Add(_virtualLine);
        }


        /// <summary>
        /// Draws item on complete
        /// </summary>
        public Point DrawOnComplete(Point cupt)
        {
            switch (tool)
            {
                case CurrentTool.Pen:
                    {
                        var pen = toolHelper.CreatePen(PrevPoint, cupt);
                        ApplyAttributes(pen as Shape);
                        _canvas.Children.Add(pen);
                        tempHolder.Add(pen as Shape);
                        PrevPoint = cupt;
                        break;
                    }

                case CurrentTool.Rectangle:
                    {
                        var shp = toolHelper.CreateShape<Rectangle>(PrevPoint, cupt);
                        ApplyAttributes(shp as Shape);
                        _canvas.Children.Add(shp);
                        tempHolder.Add(shp as Shape);
                        PrevPoint = cupt;
                        break;
                    }
                case CurrentTool.Ellipse:
                    {
                        var shp = toolHelper.CreateShape<Ellipse>(PrevPoint, cupt);
                        ApplyAttributes(shp as Shape);
                        _canvas.Children.Add(shp);
                        tempHolder.Add(shp as Shape);
                        PrevPoint = cupt;
                        break;
                    }
            }

            if (tempHolder.Count > 0)
            {
                SubmitDelta();
            }

            return cupt;
        }

        /// <summary>
        /// Draws the item on move
        /// </summary>
        public  Point DrawOnMove(Point cupt)
        {
            switch (tool)
            {
                case CurrentTool.Brush:
                    {
                        HideVirtualLine();
                        var spot = toolHelper.CreateBrush(PrevPoint, cupt, StrokeWidth);
                        (spot as Shape).StrokeThickness = 0;
                        (spot as Shape).Fill = FillColor;
                        _canvas.Children.Add(spot);
                        tempHolder.Add(spot as Shape);
                        if (tempHolder.Count > 10) SubmitDelta();
                        PrevPoint = cupt;
                        break;

                    }

                case CurrentTool.Pencil:
                    {
                        var pen = toolHelper.CreatePen(PrevPoint, cupt);
                        ApplyAttributes(pen as Shape);
                        (pen as Shape).StrokeThickness = 3;
                       _canvas.Children.Add(pen);
                        tempHolder.Add(pen as Shape);
                        if (tempHolder.Count > 10) SubmitDelta();

                        PrevPoint = cupt;
                        break;
                    }
                default:
                    {
                        ShowVirtualLine(StartPoint.X, StartPoint.Y, cupt.X, cupt.Y);
                        
                        break;
                    }

            }
            return cupt;
        }

        /// <summary>
        /// Apply shape attribute
        /// </summary>
        public  void ApplyAttributes(Shape e)
        {
            e.Stroke = StrokeColor;
            e.Fill = FillColor;
            e.StrokeThickness = 2;
            e.StrokeThickness = StrokeWidth;
        }


        /// <summary>
        /// Hide virtual line
        /// </summary>
        public void HideVirtualLine()
        {
            _virtualLine.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Shows the virtual line
        /// </summary>
        public void ShowVirtualLine(double x1, double y1, double x2, double y2)
        {
            if (!_canvas.Children.Contains(_virtualLine))
                _canvas.Children.Add(_virtualLine);
            _virtualLine.Visibility = Visibility.Visible;
            Canvas.SetZIndex(_virtualLine, 0);
            _virtualLine.X1 = x1;
            _virtualLine.X2 = x2;
            _virtualLine.Y1 = y1;
            _virtualLine.Y2 = y2;
        }


        /// <summary>
        /// Submits a delta to the service
        /// </summary>
        /// <param name="shapes"></param>
        void SubmitDelta()
        {

            try
            {
                var objects = from sh in TempHolder
                              select canHelper.Shape2Object(sh);
                string data = objects.ToArray().JsonSerialize();
                if (DataAdded != null) DataAdded(data);
            }
            catch (Exception ex)
            {
                //Ignore
                //MessageBox.Show(ex.ToString());
            }

            tempHolder.Clear();

        }


        /// <summary>
        /// Add an object to canvas
        /// </summary>
        /// <param name="drawingCanvas"></param>
        /// <param name="objdata"></param>
        public void AddObjects(string objdata, string from)
        {
            try
            {
                var objects = objdata.JsonDeserialize<ScreenObject[]>();

                foreach (var obj in objects)
                {
                    if (obj.Type == ScreenObjectType.Text)
                    {
                        this.messages.Insert(0, new TalkItem()
                        {
                            Text = obj.Text,
                            Time = DateTime.Now.ToShortTimeString(),
                            From = from
                        });
                    }
                    else
                    {
                        var shp = canHelper.Object2Shape(obj);
                        if (shp != null)
                        {
                            _canvas.Children.Add(shp);
                        }
                    }
                }

            }
            catch { }
        }

        #endregion

    }


    #region TalkItem

    public class TalkItem
    {
        public string Text { get; set; }
        public string From { get; set; }
        public string Time { get; set; }
    }

    #endregion

    #region Enum
    public enum CurrentTool
    {
        Pen,
        Pencil,
        Brush,
        Sticky,
        Ellipse,
        Rectangle
    }
    #endregion
}
