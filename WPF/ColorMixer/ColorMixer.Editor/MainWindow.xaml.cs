using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ColorMixer.Business;
using ColorMixer.Domain;
using Petzold.Media2D;

namespace ColorMixer.Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //max 50 nodes(In any case this will allocate  )
        private const int NodeCount = 50;
        private readonly BusinessHandler business;

        //Holds the taken color info of the object in use      
        private Color CurrentColor;

        //The edges are line objects therefore have starting and ending points
        //all the edges are held by a list
        Dictionary<string, List<LineGeometry>> EndLines = new Dictionary<string, List<LineGeometry>>();
        Dictionary<string, List<LineGeometry>> StartLines = new Dictionary<string, List<LineGeometry>>();
        List<LineGeometry> _Line = new List<LineGeometry>();

        //Ellipse represents our nodes and 
        //if nodes in the screen are counted and index of shapeCount helps developer
        private static Ellipse sourceEllipse = null;
        int shapeCount = 0;

        //**
        // flag for enabling "New ellipse" mode
        bool isAddNewEllipse = false;
        // flag for enabling "New link" mode
        bool isAddNewLink = false;
        // flag that indicates that the link drawing with a mouse started
        bool isLinkStarted = false;
        // variable for holding the starting ellipse while drawing link 
        Ellipse linkedEllipse;
        // Line drawn by the mouse before connection established
        LineGeometry link;
        //**
        

        public MainWindow()
        {
            InitializeComponent();
            this.CurrentColor = Colors.DodgerBlue;
            //BusinessHandler allocates for the graph object with size 50
            business = new BusinessHandler(NodeCount);
    }

        //CustomColorPicker is taken from other sources via internet
        //and added as a usercontrol in the solution
        //customCP is the specific instance used in xaml
        void customCP_SelectedColorChanged(Color obj)
        {
            this.CurrentColor = obj;
            colorTextBox.Background = new SolidColorBrush(obj);
           
        }

        //Ellipsis are created dynamically at run-time
        //and are held by the node class objects.
        //These node objects are added into a graph
        private Ellipse CreateNode(MouseButtonEventArgs e)
        {
            // Graph update
            var graphNode = business.CreateNode(ConvertToDrawingColor(this.CurrentColor));
            StartLines = new Dictionary<string, List<LineGeometry>>();
            EndLines = new Dictionary<string, List<LineGeometry>>();

            // Interface node creation.
            Ellipse shapeNode = new Ellipse()
            {
                Uid = graphNode.Id.ToString(),
                StrokeThickness = 2,
                Height = 50,
                Width = 50,
                Fill = ConvertToMediaBrush(this.CurrentColor),

            };
            
            return shapeNode;
        }

        

        private System.Drawing.Color ConvertToDrawingColor(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private Brush ConvertToMediaBrush(Color color)
        {
            return new SolidColorBrush(color);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(Window1_PreviewMouseLeftButtonDown);
            this.PreviewMouseMove += new MouseEventHandler(Window1_PreviewMouseMove);
            this.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(Window1_PreviewMouseLeftButtonUp);
        }



        private void Window1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            sourceEllipse = e.Source as Ellipse;
            // If adding new action...
            if (isAddNewEllipse)
            {
                if (CanvasArea.IsMouseOver)
                {
                    // Create new ellipse object 

                    Ellipse shapeNode = CreateNode(e);

                    Point position = e.MouseDevice.GetPosition(CanvasArea);
                    Canvas.SetLeft(shapeNode, position.X - (shapeNode.Width / 2));
                    Canvas.SetTop(shapeNode, position.Y - (shapeNode.Height / 2));
                    CanvasArea.Children.Add(shapeNode);

                    // resume common layout for application
                    isAddNewEllipse = false;
                    Mouse.OverrideCursor = null;
                    btnAdd.IsEnabled = true;
                    e.Handled = true;
                    shapeCount++;

                    //if max number of link is established there is no need for an add link button
                    if (shapeCount >= 2 && possibleMaxLinkCount(shapeCount) > connectors.Children.Count)
                        btnAddLink.IsEnabled = true;
                }
            }

            // Is adding new link and an Ellipse object is clicked...
            if (isAddNewLink && e.Source.GetType() == typeof(Ellipse))
            {
                Ellipse infoNode = e.Source as Ellipse;
                if (!isLinkStarted)
                {
                    if (link == null || link.EndPoint != link.StartPoint)
                    {
                        Point position = e.MouseDevice.GetPosition(CanvasArea);
                        link = new LineGeometry(position, position);
                        connectors.Children.Add(link);
                        isLinkStarted = true;
                        linkedEllipse = e.Source as Ellipse;
                        e.Handled = true;
                        updateInfoCircle(firstCircle, Convert.ToInt32(infoNode.Uid));
                    }
                }
            }
        }

        private void Window1_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (isAddNewLink && isLinkStarted)
            {
                // Set the new link end point to current mouse position
                link.EndPoint = e.MouseDevice.GetPosition(CanvasArea);
                e.Handled = true;
            }
        }
        private void Window1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            // If "Add link" mode enabled and line drawing started (line placed to canvas)
            if (isAddNewLink && isLinkStarted)
            {
                // declare the linking state
                bool linked = false;
                // We released the button on Ellipse object
                if (e.Source.GetType() == typeof(Ellipse))
                {
                    Ellipse targetEllipse = e.Source as Ellipse;
                    // define the final endpoint of line
                    link.EndPoint =e.MouseDevice.GetPosition(CanvasArea);
                    // if any line was drawn (avoid just clicking on the thumb)
                    if (link.EndPoint != link.StartPoint && linkedEllipse != targetEllipse)
                    {
                        //This condition controls whether any link has been established between to nodes.(in order to provide one-direction)
                        if (business.UnLinkedBefore(Convert.ToInt32(sourceEllipse.Uid), Convert.ToInt32(targetEllipse.Uid)))
                        {
                            //TODO -- shape uid gives us string ids. Therefore unnecessary converting operations
                            //are being applied. In order to avert it, id member of node class should be changed into string
                            //or just after creating ellipsis the uids can be converted for once and can be reused without converting it everytime
                            Node sourceNode = business.FindNodeInGraph(Convert.ToInt32(sourceEllipse.Uid));
                            Node targetNode = business.FindNodeInGraph(Convert.ToInt32(targetEllipse.Uid));

                            // establish connection
                            LinkTo(sourceEllipse, targetEllipse, link);
                            //merge node colors
                            System.Drawing.Color newColor = business.MergeTwoColors(sourceNode.Color, targetNode.Color, (0.5f));
                            //Second ellipse at the top assigned by the color of the end-line node  
                            updateInfoCircle(secondCircle, targetNode.Id);
                            //Node is added as the connection of the first node
                            business.BindNodes(sourceNode.Id, targetNode.Id);
                            //Ellipses both in the canvas area and at the top are coloured
                            targetEllipse.Fill = thirdCircle.Fill = ConvertToMediaBrush(System.Windows.Media.Color.FromArgb(newColor.A, newColor.R, newColor.G, newColor.B));
                            
                            // set linked state to true
                            linked = true;
                        }

                    }
                }
                // if we didn't manage to approve the linking state
                // button is not released on Ellipse object or double-clicking was performed
                if (!linked)
                {
                    // remove line from the canvas
                    connectors.Children.Remove(link);
                    // clear the link variable
                    link = null;
                }

                // exit link drawing mode
                isLinkStarted = isAddNewLink = false;
                // configure GUI
                btnAdd.IsEnabled = true;
                //if max number of link is established there is no need for an add link button
                if (possibleMaxLinkCount(shapeCount) > connectors.Children.Count)
                    btnAddLink.IsEnabled = true;

                Mouse.OverrideCursor = null;
                e.Handled = true;
            }
            topLabel.Content = "Links established: " + connectors.Children.Count.ToString();
        }

        //This method is used for updating the colors of ellipsis at the top 
        private void updateInfoCircle(Ellipse relatedCircle,int id)
        {
            Node n= business.FindNodeInGraph(Convert.ToInt32(id));
            relatedCircle.Fill = ConvertToMediaBrush(System.Windows.Media.Color.FromArgb(n.Color.A,n.Color.R,n.Color.G,n.Color.B));
        }



        //calculates max possible links that can be drawn between the nodes. Between each two nodes, only one directed link can be drawn.
        private int possibleMaxLinkCount(int nodecount)
        {
            int maxLinkCount = 0;
            for (int i = 0; i < nodecount; i++)
            {
                maxLinkCount += (nodecount - (i + 1));
            }
            return maxLinkCount;
        }


        #region Linking logic
        // This method establishes a link between selected ellipse and specified one.
        // Returns a line geometry with updated positions to be processed outside.

        // Note: this is commonly to be used for drawing links with mouse when the line object is predefined outside this class
        // Arrowline class is taken from Petzold.Media2D
        public bool LinkTo(Ellipse source, Ellipse target, LineGeometry line)
        {
            ArrowLine aline1 = new ArrowLine();
            _Line.Add(line);

            // Save as starting line for current thumb
            if (StartLines.ContainsKey(source.Uid))
                StartLines[source.Uid] = _Line;
            else
                StartLines.Add(source.Uid, _Line);
            // Save as ending line for target thumb
            if (EndLines.ContainsKey(target.Uid))
                EndLines[target.Uid] = _Line;
            else
                EndLines.Add(target.Uid, _Line);
            // Ensure both tumbs the latest layout
            source.UpdateLayout();
            target.UpdateLayout();
            // Update line position
            line.StartPoint = new Point(Canvas.GetLeft(source) + source.ActualWidth / 2, Canvas.GetTop(source) + source.ActualHeight / 2);
            line.EndPoint = new Point(Canvas.GetLeft(target) + target.ActualWidth / 2, Canvas.GetTop(target) + target.ActualHeight / 2);

            aline1.X1 = line.StartPoint.X;
            aline1.Y1 = line.StartPoint.Y;
            aline1.X2 = line.EndPoint.X;
            aline1.Y2 = line.EndPoint.Y;
            aline1.Stroke = Brushes.Black;
            aline1.StrokeThickness = 1;
            CanvasArea.Children.Add(aline1);
            return true;
        }
        #endregion

        // This method updates all the starting and ending lines assigned for the given ellipse 
        // according to the latest known ellipse position on the canvas
        public void UpdateLinks(Ellipse ellipse)
        {
            double left = Canvas.GetLeft(ellipse);
            double top = Canvas.GetTop(ellipse);

            for (int i = 0; i < StartLines[ellipse.Uid].Count; i++)
                StartLines[ellipse.Uid][i].StartPoint = new Point(left + ellipse.ActualWidth / 2, top + ellipse.ActualHeight / 2);

            for (int i = 0; i < EndLines[ellipse.Uid].Count; i++)
                EndLines[ellipse.Uid][i].EndPoint = new Point(left + ellipse.ActualWidth / 2, top + ellipse.ActualHeight / 2);
        }

        private void btnAddLink_Click(object sender, RoutedEventArgs e)
        {
            isAddNewLink = true;
            Mouse.OverrideCursor = Cursors.Cross;
            btnAdd.IsEnabled = btnAddLink.IsEnabled = false;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            isAddNewEllipse = true;
            Mouse.OverrideCursor = Cursors.SizeAll;
            btnAdd.IsEnabled = btnAdd.IsEnabled = false;
        }
    }
}
