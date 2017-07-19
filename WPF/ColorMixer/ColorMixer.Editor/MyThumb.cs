using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColorMixer.Editor
{
    public class MyThumb : Thumb
    {
        public int ID;
        public List<LineGeometry> EndLines { get; private set; }
        public List<LineGeometry> StartLines { get; private set; } 


        #region Constructors
        public MyThumb(int id)
            : base()
        {
            ID = id;
            StartLines = new List<LineGeometry>();
            EndLines = new List<LineGeometry>();
        }

        public MyThumb(int id, Point position)
            : this(id)
        {
            ID = id;
            this.SetPosition(position);
        }

        public MyThumb(int id, Point position, DragDeltaEventHandler dragDelta)
            : this(id, position)
        {
            ID = id;
            this.SetPosition(position);
            this.DragDelta += dragDelta;
        }
        #endregion

        // Helper method for setting the position of our thumb
        public void SetPosition(Point value)
        {
            Canvas.SetLeft(this, value.X);
            Canvas.SetTop(this, value.Y);
        }

        #region Linking logic
        // This method establishes a link between current thumb and specified thumb.
        // Returns a line geometry with updated positions to be processed outside.
        public LineGeometry LinkTo(MyThumb target)
        {
            // Create new line geometry
            LineGeometry line = new LineGeometry();
            // Save as starting line for current thumb
            this.StartLines.Add(line);
            // Save as ending line for target thumb
            target.EndLines.Add(line);
            // Ensure both tumbs the latest layout
            this.UpdateLayout();
            target.UpdateLayout();
            // Update line position
            line.StartPoint = new Point(Canvas.GetLeft(this) + this.ActualWidth / 2, Canvas.GetTop(this) + this.ActualHeight / 2);
            line.EndPoint = new Point(Canvas.GetLeft(target) + target.ActualWidth / 2, Canvas.GetTop(target) + target.ActualHeight / 2);
            // return line for further processing
            return line;
        }

        // This method establishes a link between current thumb and target thumb using a predefined line geometry
        // Note: this is commonly to be used for drawing links with mouse when the line object is predefined outside this class
        public bool LinkTo(MyThumb target, LineGeometry line)
        {
            // Save as starting line for current thumb
            this.StartLines.Add(line);
            // Save as ending line for target thumb
            target.EndLines.Add(line);
            // Ensure both tumbs the latest layout
            this.UpdateLayout();
            target.UpdateLayout();
            // Update line position
            line.StartPoint = new Point(Canvas.GetLeft(this) + this.ActualWidth / 2, Canvas.GetTop(this) + this.ActualHeight / 2);
            line.EndPoint = new Point(Canvas.GetLeft(target) + target.ActualWidth / 2, Canvas.GetTop(target) + target.ActualHeight / 2);
            return true;
        } 
        #endregion

        // This method updates all the starting and ending lines assigned for the given thumb 
        // according to the latest known thumb position on the canvas
        public void UpdateLinks()
        {
            double left = Canvas.GetLeft(this);
            double top = Canvas.GetTop(this);

            for (int i = 0; i < this.StartLines.Count; i++)
                this.StartLines[i].StartPoint = new Point(left + this.ActualWidth / 2, top + this.ActualHeight / 2);

            for (int i = 0; i < this.EndLines.Count; i++)
                this.EndLines[i].EndPoint = new Point(left + this.ActualWidth / 2, top + this.ActualHeight / 2);
        }
                
        // Upon applying template we apply the "Title" and "ImageSource" properties to the template elements.
        public override void OnApplyTemplate()
        {
            //base.OnApplyTemplate();

            //// Access the textblock element of template and assign it if Title property defined
            //if (this.Title != string.Empty)
            //{
            //    TextBlock txt = this.Template.FindName("tplTextBlock", this) as TextBlock;
            //    if (txt != null)
            //        txt.Text = Title;
            //}

            //// Access the image element of our custom template and assign it if ImageSource property defined
            //if (this.ImageSource != string.Empty)
            //{
            //    Image img = this.Template.FindName("tplImage", this) as Image;
            //    if (img != null)
            //        img.Source = new BitmapImage(new Uri(this.ImageSource, UriKind.Relative));
            //}
        }
    }
}
