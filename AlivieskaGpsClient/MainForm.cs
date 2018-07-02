﻿/*
Copyright (C) 2018 Wampa842

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.IO;

namespace AlivieskaGpsClient
{
	// Form behaviour
	public partial class MainForm : Form
	{
		private readonly string _locationsPath = "resources\\locations.csv";    // A CSV file containing points of interest
		private readonly string _hazardsPath = "resources\\hazards.csv";        // A CSV file containing information about road hazards
		private readonly string _mapImagePath = "resources\\map.png";           // The map background image file
		private readonly string _configPath = "resources\\config";  // Plaintext file containing configuration key-value pairs
		private Bitmap _baseImage = null;                           // The image to display on the form
		private Point _imageCenter;                                 // The center coordinates of the image
		private int _prevX, _prevY;                                 // Used in calculating the delta position while panning the image
		private Size _imageSize;                                    // The magnification of the image
		private MapDrawing.PointOfInterest _hoverPoi = null;        // The point of interest the mouse cursor is over
		private MapDrawing.PointOfInterest _selectedPoi = null;     // A point of interest selected by clicking

		private MouseButtons _panButton = MouseButtons.Left;        // The mouse button that grabs and pans the map
		private MouseButtons _selectButton = MouseButtons.Right;    // The mouse button which selects points of interest on the map
		private MouseButtons _selectArbitraryButton = MouseButtons.Middle;  // The mouse button which selects an arbitrary point on the map

		private GpsData _gpsData;                                   // Object that handles the connection to the server
		private System.Timers.Timer _colorResetTimer = new System.Timers.Timer { AutoReset = false, Enabled = false, Interval = 250 };  // Timer that makes the light do the blinky

		private DetailsForm _detailsForm = new DetailsForm();                           // Shows info about a selected location
		private RecordLocationForm _recordForm;

		private void _loadConfig()
		{
			if (!File.Exists(_configPath))
				return;
			using (StreamReader reader = new StreamReader(_configPath))
			{
				string line;
				string[] tok;
				while (!reader.EndOfStream)
				{
					line = reader.ReadLine();
					tok = line.Trim().Split(' ');
					switch (tok[0].Trim().ToLowerInvariant())
					{
						case "url":
							connectionUrlText.Text = string.Join("", tok, 1, tok.Length - 1);
							break;
						case "follow":
							bool check = false;
							bool.TryParse(tok[1], out check);
							followCheck.Checked = check;
							break;
					}
				}
			}
		}

		private void _saveConfig()
		{
			using (StreamWriter writer = new StreamWriter(_configPath))
			{
				writer.WriteLine($"url {connectionUrlText.Text}");
				writer.WriteLine($"follow {followCheck.Checked.ToString().ToLowerInvariant()}");
			}
		}

		public MainForm()
		{
			InitializeComponent();
			_gpsData = new GpsData(this);
			//_recordForm.Data = _gpsData
			_recordForm = new RecordLocationForm(_gpsData);
			_colorResetTimer.Elapsed += (o, args) => connectionStatusLabel.ForeColor = Color.ForestGreen;
			_loadConfig();
		}

		// Load resources; initialize pan and zoom
		private void MainForm_Load(object sender, EventArgs e)
		{
			_baseImage = new Bitmap(_mapImagePath);
			_imageCenter = new Point(mapImage.Width / 2, mapImage.Height / 2);
			_imageSize = new Size(mapImage.Width, mapImage.Height);

			MapDrawing.PointsOfInterest = MapDrawing.PointOfInterest.ReadFromCsv(_locationsPath);
			MapDrawing.Hazards = MapDrawing.RoadHazard.ReadFromCsv(_hazardsPath);

			mapImage.Invalidate();
		}

		// Change the image magnification
		private void zoomSlider_Scroll(object sender, EventArgs e)
		{
			_imageSize.Width = _imageSize.Height = zoomSlider.Value * 4;
			zoomMultLabel.Text = ((TrackBar)sender).Value.ToString() + "%";
			mapImage.Invalidate();
		}

		// Display the image and points of interest
		private void mapImage_Paint(object sender, PaintEventArgs e)
		{
			MapDrawing.DrawImageCenter(e.Graphics, _baseImage, _imageCenter, _imageSize);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			MapDrawing.DrawPointsOfInterest(e.Graphics, _imageCenter, _imageSize, displayTownsCheck.Checked, displayJobsCheck.Checked, displayShopsCheck.Checked, displayLocationsCheck.Checked);
			MapDrawing.DrawRoadHazards(e.Graphics, _imageCenter, _imageSize, displayRoadHazardsCheck.Checked, displayTrafficHazardsCheck.Checked, displayRailwayHazardsCheck.Checked, displayHazardsCheck.Checked, displayHazardsCheck.Checked);
			if (_selectedPoi != null)
				MapDrawing.DrawCross(e.Graphics, new Pen(Color.Black, 1.0f), _selectedPoi.MapLocation(_imageCenter, _imageSize), mapImage.Size);
			if (_gpsData.Success)
				MapDrawing.DrawArrow(e.Graphics, _gpsData.MapPosition, _gpsData.Heading, _imageCenter, _imageSize);
		}

		// Set up previous coordinates for panning and handle point selection
		private void mapImage_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == _panButton)
			{
				_prevX = e.X;
				_prevY = e.Y;
			}
			if (e.Button == _selectButton)
			{
				bool inRange = false;
				if (MapDrawing.PointsOfInterest != null)
				{
					foreach (var poi in MapDrawing.PointsOfInterest.Reverse())
					{
						if (poi.Enabled && poi.InRange(e.Location, _imageCenter, _imageSize))
						{
							inRange = true;
							_selectedPoi = poi;
							_detailsForm.UpdateDetails(poi);
							break;
						}
					}
				}
				if (!inRange)
				{
					_selectedPoi = null;
				}
				else
				{

				}
			}
			if (e.Button == _selectArbitraryButton)
			{
				//float x = (e.X - _imageCenter.X) / (float)(zoomSlider.Value * 4);
				//float y = (e.Y - _imageCenter.Y) / (float)(zoomSlider.Value * 4);
				//gpsDataX.Text = x.ToString();
				//gpsDataZ.Text = y.ToString();
				//using (System.IO.StreamWriter writer = new System.IO.StreamWriter("locations.csv", true))
				//	writer.WriteLineAsync($"\"{outputName.Text}\",\"{x}\",\"{y}\"");
			}
			((PictureBox)sender).Invalidate();
		}

		// Pan the image and handle hovering
		private void mapImage_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == _panButton)
			{
				_imageCenter.X += e.X - _prevX;
				_imageCenter.Y += e.Y - _prevY;

				((PictureBox)sender).Invalidate();

				_prevX = e.X;
				_prevY = e.Y;
			}
			bool inRange = false;
			if (MapDrawing.PointsOfInterest != null)
			{
				foreach (var poi in MapDrawing.PointsOfInterest.Reverse())
				{
					if (poi.Enabled && poi.InRange(e.Location, _imageCenter, _imageSize))
					{
						_hoverPoi = poi;
						inRange = true;
						break;
					}
				}
			}
			((PictureBox)sender).Cursor = inRange ? Cursors.Hand : Cursors.SizeAll;
			if (!inRange)
			{
				_hoverPoi = null;
			}
		}

		// Connect to or disconnect from the server
		private void gpsConnectButton_Click(object sender, EventArgs e)
		{
			// If the timer is running, stop it and enable editing the URL text box.
			if (gpsUpdateTimer.Enabled)
			{
				gpsUpdateTimer.Stop();
				connectionUrlText.Enabled = true;
				gpsConnectButton.Text = "Connect";
				connectionStatusLabel.ForeColor = Color.DarkGray;
			}
			// If not, lock the text box and start the timer.
			else
			{
				gpsUpdateTimer.Start();
				connectionUrlText.Enabled = false;
				gpsConnectButton.Text = "Disconnect";
				connectionStatusLabel.ForeColor = Color.Blue;
			}
		}

		// Periodically request data from the server
		private void gpsUpdateTimer_Tick(object sender, EventArgs e)
		{
			_gpsData.Get(connectionUrlText.Text);
		}

		// Reset pan and zoom
		private void resetMapButton_Click(object sender, EventArgs e)
		{
			_imageCenter = new Point(mapImage.Width / 2, mapImage.Height / 2);
			_imageSize = new Size(mapImage.Width, mapImage.Height);
			zoomSlider.Value = zoomSlider.Minimum;
			zoomMultLabel.Text = zoomSlider.Value + "%";
			mapImage.Invalidate();
		}

		// Set URL to http://localhost:8080/
		private void default8080Button_Click(object sender, EventArgs e)
		{
			if (connectionUrlText.Enabled)
				connectionUrlText.Text = "http://localhost:8080/";
		}

		// Save config and close other forms when the window is closed
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			_saveConfig();
			_detailsForm.Close();
			_recordForm.Close();
		}

		// Set URL to http://localhost/gps/
		private void default80Button_Click(object sender, EventArgs e)
		{
			if (connectionUrlText.Enabled)
				connectionUrlText.Text = "http://localhost/gps/";
		}

		// Open/close Details form
		private void showDetailsButton_Click(object sender, EventArgs e)
		{
			if (_detailsForm.Visible)
			{
				_detailsForm.Hide();
				((Button)sender).Text = "Hide <<";
			}
			else
			{
				_detailsForm.Show();
				((Button)sender).Text = "Details >>";
			}
		}

		// Update location visibility
		private void poiDisplayCheck_CheckedChanged(object sender, EventArgs e)
		{
			foreach(var poi in MapDrawing.PointsOfInterest)
			{
				if (poi.Type == MapDrawing.PointOfInterest.PointOfInterestType.Town)
					poi.Enabled = displayTownsCheck.Checked;
				if (poi.Type == MapDrawing.PointOfInterest.PointOfInterestType.Service)
					poi.Enabled = displayShopsCheck.Checked;
				if (poi.Type == MapDrawing.PointOfInterest.PointOfInterestType.Work)
					poi.Enabled = displayJobsCheck.Checked;
			}
			foreach(var poi in MapDrawing.Hazards)
			{
				if (poi.Type == MapDrawing.RoadHazard.RoadHazardType.Topography)
					poi.Enabled = displayRoadHazardsCheck.Checked;
				if (poi.Type == MapDrawing.RoadHazard.RoadHazardType.Traffic)
					poi.Enabled = displayTrafficHazardsCheck.Checked;
				if (poi.Type == MapDrawing.RoadHazard.RoadHazardType.Railway)
					poi.Enabled = displayRailwayHazardsCheck.Checked;
			}
			mapImage.Invalidate();
		}

		// Uncheck/check all points of interest
		private void displayLocationsCheck_CheckedChanged(object sender, EventArgs e)
		{
			displayTownsCheck.Checked = displayTownsCheck.Enabled = ((CheckBox)sender).Checked;
			displayShopsCheck.Checked = displayShopsCheck.Enabled = ((CheckBox)sender).Checked;
			displayJobsCheck.Checked = displayJobsCheck.Enabled = ((CheckBox)sender).Checked;
			poiDisplayCheck_CheckedChanged(sender, e);
		}

		// Uncheck/check all hazards
		private void displayHazardsCheck_CheckedChanged(object sender, EventArgs e)
		{
			displayRoadHazardsCheck.Checked = displayRoadHazardsCheck.Enabled = ((CheckBox)sender).Checked;
			displayTrafficHazardsCheck.Checked = displayTrafficHazardsCheck.Enabled = ((CheckBox)sender).Checked;
			displayRailwayHazardsCheck.Checked = displayRailwayHazardsCheck.Enabled = ((CheckBox)sender).Checked;
			poiDisplayCheck_CheckedChanged(sender, e);
		}

		// Open/close the location recorder window
		private void showRecordButton_Click(object sender, EventArgs e)
		{
			if(_recordForm.Visible)
			{
				_recordForm.Hide();
			}
			else
			{
				_recordForm.Location = Point.Add(this.Location, new Size(0, this.Height));
				_recordForm.Show();
			}
		}

		// Keep sub-forms aligned
		private void MainForm_Move(object sender, EventArgs e)
		{
			_recordForm.Location = Point.Add(this.Location, new Size(0, this.Height));
		}

		// Update the form to display whatever data is currently present
		public void UpdateGpsData()
		{
			if (gpsUpdateTimer.Enabled)
			{
				if (_gpsData.Success)
				{
					gpsDataX.Text = _gpsData.X.ToString();
					gpsDataY.Text = _gpsData.Y.ToString();
					gpsDataZ.Text = _gpsData.Z.ToString();
					gpsDataHeading.Text = _gpsData.Heading.ToString();
					gpsDataSpeed.Text = _gpsData.Speed.ToString();

					connectionStatusLabel.ForeColor = Color.LimeGreen;
					_colorResetTimer.Start();
				}
				else
				{
					connectionStatusLabel.ForeColor = Color.Red;
				}
			}
			if (followCheck.Checked)
			{
				PointF p = _gpsData.MapPosition;
				_imageCenter.X = (int)(-p.X * _imageSize.Width + mapImage.Width / 2);
				_imageCenter.Y = (int)(-p.Y * _imageSize.Height + mapImage.Height / 2);
			}
			mapImage.Invalidate();
		}
	}

	// Drawing classes and methods
	public static class MapDrawing
	{
		#region DRAWING_GENERIC
		// Draw an image around its center
		public static void DrawImageCenter(Graphics g, Image image, Point center, Size size)
		{
			g.DrawImage(image, center.X - size.Width / 2, center.Y - size.Height / 2, size.Width, size.Height);
		}

		// Draw a cross spanning the entire area, intersecting at given coordinates
		public static void DrawCross(Graphics g, Pen pen, float x, float y, Size size)
		{
			g.DrawLine(pen, 0, y, size.Width, y);
			g.DrawLine(pen, x, 0, x, size.Height);
		}

		// Draw a cross spanning the entire area, intersecting at a given point
		public static void DrawCross(Graphics g, Pen pen, Point intersect, Size size)
		{
			DrawCross(g, pen, intersect.X, intersect.Y, size);
		}

		// Vertices of the arrow polygon
		private static PointF[] _arrowPoints =
		{
			new Point(0, -10),
			new Point(7, 10),
			new Point(0, 5),
			new Point(-7, 10)
		};

		private static PointF[] _arrowPointsTransformed = new PointF[4];

		// Draw an arrow, rotated
		public static void DrawArrow(Graphics g, PointF position, double angle, Point center, Size size)
		{
			double rad = angle * Math.PI / 180.0;
			float cos = (float)Math.Cos(rad);
			float sin = (float)Math.Sin(rad);

			for (int i = 0; i < _arrowPoints.Length; ++i)
			{
				float vx = _arrowPoints[i].X * cos - _arrowPoints[i].Y * sin;
				float vy = _arrowPoints[i].X * sin + _arrowPoints[i].Y * cos;
				_arrowPointsTransformed[i].X = vx + (position.X * size.Width + center.X);
				_arrowPointsTransformed[i].Y = vy + (position.Y * size.Height + center.Y);
			}

			g.FillPolygon(new SolidBrush(Color.Orange), _arrowPointsTransformed);
			g.DrawPolygon(new Pen(Color.Red, 1.75f), _arrowPointsTransformed);
		}
		#endregion

		#region POI
		// Properties of the circle to be drawn
		public class CircleStyle
		{
			public bool Stroke { get; set; }        // Should this circle be stroked?
			public Color StrokeColor { get; set; }  // Stroke line color
			public float StrokeWidth { get; set; }  // Stroke line thickness
			public bool Fill { get; set; }          // Should the circle be filled?
			public Color FillColor { get; set; }    // Fill color
			public float Radius { get; set; }       // Radius

			public CircleStyle(bool stroke, Color strokeColor, float strokeWidth, bool fill, Color fillColor, float radius)
			{
				this.Stroke = stroke;
				this.StrokeColor = strokeColor;
				this.StrokeWidth = strokeWidth;
				this.Fill = fill;
				this.FillColor = fillColor;
				this.Radius = radius;
			}

			public CircleStyle() : this(true, Color.FromArgb(255, 0, 0, 0), 1.0f, true, Color.FromArgb(127, 0, 0, 0), 10) { }

			public static CircleStyle[] Presets { get; } =
			{
				new CircleStyle(true, Color.Blue, 1.0f, false, Color.Black, 7.5f),
				new CircleStyle(true, Color.Blue, 2.0f, true, Color.FromArgb(80, Color.MediumBlue), 20.0f),
				new CircleStyle(true, Color.DarkGreen, 1.0f, true, Color.Green, 7.5f),
				new CircleStyle(true, Color.Orange, 1.0f, true, Color.Yellow, 7.5f)
			};
			public static CircleStyle Default => Presets[0];
			public static CircleStyle Town => Presets[1];
			public static CircleStyle Work => Presets[2];
			public static CircleStyle Shop => Presets[3];
		}

		// Draw a circle around its center coordinates using the specified style
		public static void DrawCircle(Graphics g, int x, int y, CircleStyle style)
		{
			Rectangle rect = new Rectangle((int)(x - style.Radius), (int)(y - style.Radius), (int)(2 * style.Radius), (int)(2 * style.Radius));
			if (style.Fill)
			{
				g.FillEllipse(new SolidBrush(style.FillColor), rect);
			}
			if (style.Stroke)
			{
				g.DrawEllipse(new Pen(style.StrokeColor, style.StrokeWidth), rect);
			}
		}

		// Draw a circle around its center point using the specified style
		public static void DrawCircle(Graphics g, Point center, CircleStyle style)
		{
			DrawCircle(g, center.X, center.Y, style);
		}

		// A point on the map
		public abstract class MapPoint
		{
			public PointF Location { get; } // X, Y location within the -0.5..0.5 boundaries
			public float X => Location.X;   // X coordinate between the -0.5..0.5 boundaries
			public float Y => Location.Y;   // Y coordinate between the -0.5..0.5 boundaries
			public bool Enabled { get; set; } = true;   // Indicates whether the point should be visible and selectable

			// Point on the map at origin
			public MapPoint()
			{

			}

			// Point on the map defined by a point
			protected MapPoint(PointF location)
			{
				this.Location = location;
			}

			// Point on the map defined by a pair of coordinates
			protected MapPoint(float x, float y)
			{
				this.Location = new PointF(x, y);
			}

			// X coordinate mapped to center and size
			public int MapX(Point center, Size size)
			{
				return (int)(this.X * size.Width + center.X);
			}

			// Y coordinate mapped to center and size
			public int MapY(Point center, Size size)
			{
				return (int)(this.Y * size.Height + center.Y);
			}

			// Point mapped to center and size
			public Point MapLocation(Point center, Size size)
			{
				return new Point(MapX(center, size), MapY(center, size));
			}
		}

		// A point of interest on the map
		public class PointOfInterest : MapPoint, IComparable<PointOfInterest>, IEquatable<PointOfInterest>
		{
			public enum PointOfInterestType { Other = 0, Town = 1, Service = 2, Work = 3 }
			public CircleStyle Style { get; }
			public string Name { get; }
			public string ID { get; }
			public PointOfInterestType Type { get; }

			public PointOfInterest(string id, string name, PointF location, CircleStyle style, PointOfInterestType type) : base(location)
			{
				this.Style = style;
				//this.Location = location;
				this.Name = name;
				this.ID = id;
				this.Type = type;
			}

			// Draw a circle on an area defined by its center point and size
			public void Draw(Graphics g, Point center, Size size)
			{
				//DrawCircle(g, (int)(this.X * size.Width + center.X), (int)(this.Y * size.Height + center.Y), this.Style);
				Rectangle rect = new Rectangle((int)(MapX(center, size) - this.Style.Radius), (int)(MapY(center, size) - this.Style.Radius), (int)(2 * this.Style.Radius), (int)(2 * this.Style.Radius));
				if (this.Style.Fill)
				{
					g.FillEllipse(new SolidBrush(this.Style.FillColor), rect);
				}
				if (this.Style.Stroke)
				{
					g.DrawEllipse(new Pen(this.Style.StrokeColor, this.Style.StrokeWidth), rect);
				}
			}

			// Check if a point is within the circle's radius
			public bool InRange(PointF point, Point center, Size size)
			{
				double x = MapX(center, size) - point.X;
				double y = MapY(center, size) - point.Y;
				double distSq = Math.Pow(x, 2) + Math.Pow(y, 2);

				return distSq <= Math.Pow(this.Style.Radius, 2);
			}

			// Parse a CSV line and return a new PointOfInterest object
			public static PointOfInterest FromCsvString(string line)
			{
				string[] tok = line.Split(',');
				return new PointOfInterest(tok[0].Trim('"'), tok[2].Trim('"'), new PointF(float.Parse(tok[3]), float.Parse(tok[4])), CircleStyle.Presets[int.Parse(tok[1])], (PointOfInterestType)int.Parse(tok[1]));
			}

			// Read all points of interest from a CSV file
			public static SortedSet<PointOfInterest> ReadFromCsv(string path)
			{
				SortedSet<PointOfInterest> set = new SortedSet<PointOfInterest>();
				using (System.IO.StreamReader reader = new System.IO.StreamReader(path))
				{
					while (!reader.EndOfStream)
					{
						set.Add(FromCsvString(reader.ReadLine().Trim()));
					}
				}
				return set;
			}

			// Sort by type, then by ID
			public int CompareTo(PointOfInterest other)
			{
				if (this.Type != other.Type)
					return ((int)this.Type).CompareTo((int)other.Type);
				return this.ID.CompareTo(other.ID);
			}

			// Only the ID has to be unique
			public bool Equals(PointOfInterest other)
			{
				if (this.ID == other.ID)
					return true;
				return object.Equals(this, other);
			}
		}

		// Set of points of interest, sorted by their type.
		public static SortedSet<PointOfInterest> PointsOfInterest;

		// Draw all points of interest
		public static void DrawPointsOfInterest(Graphics g, Point center, Size size, bool drawTowns, bool drawJobs, bool drawShops, bool drawOthers)
		{
			if (PointsOfInterest != null)
				foreach (var poi in PointsOfInterest)
				{
					if ((poi.Type == PointOfInterest.PointOfInterestType.Town && drawTowns) || (poi.Type == PointOfInterest.PointOfInterestType.Work && drawJobs) || (poi.Type == PointOfInterest.PointOfInterestType.Service && drawShops) || (poi.Type == PointOfInterest.PointOfInterestType.Other && drawOthers))
						poi.Draw(g, center, size);
				}
		}
#endregion

		#region HAZARD
		// A class representing a road hazard. TBD: create an abstract class to be inherited by this and PointOfInterest?
		public class RoadHazard : MapPoint, IComparable<RoadHazard>, IEquatable<RoadHazard>
		{
			private static Bitmap[] _icons = new Bitmap[]
			{
				new Bitmap("resources\\hazard_other.png"),
				new Bitmap("resources\\hazard_road.png"),
				new Bitmap("resources\\hazard_traffic.png"),
				new Bitmap("resources\\hazard_train_b.png"),
				new Bitmap("resources\\hazard_other.png")
			};
			public static Size IconSize = new Size(26, 26);
			public static Size HalfSize = new Size(IconSize.Width / 2, IconSize.Height / 2);

			public enum RoadHazardType { Other = 0, Topography = 1, Traffic = 2, Railway = 3, Police = 4 }
			public string ID { get; }
			public string Name { get; }
			public string Description { get; }
			public RoadHazardType Type { get; }
			public Bitmap Image { get { return _icons[(int)Type]; } }

			public RoadHazard(string id, string name, string description, PointF location, RoadHazardType type) : base(location)
			{
				this.ID = id;
				this.Name = name;
				this.Description = description;
				this.Type = type;
			}

			public void Draw(Graphics g, Point center, Size size)
			{
				g.DrawImage(Image, new Rectangle(Point.Subtract(MapLocation(center, size), HalfSize), IconSize));
			}

			public static RoadHazard FromCsvString(string line)
			{
				// "id",type,"name","desc",x,y
				string[] tok = line.Split(',');
				return new RoadHazard(tok[0].Trim('"').Trim(), tok[2].Trim('"').Trim(), tok[3].Trim('"').Trim(), new PointF(float.Parse(tok[4].Trim()), float.Parse(tok[5].Trim())), (RoadHazardType)int.Parse(tok[1].Trim()));
			}

			public static SortedSet<RoadHazard> ReadFromCsv(string path)
			{
				SortedSet<RoadHazard> set = new SortedSet<RoadHazard>();
				using (StreamReader reader = new StreamReader(path))
				{
					while (!reader.EndOfStream)
					{
						set.Add(FromCsvString(reader.ReadLine().Trim()));
					}
				}
				return set;
			}

			public int CompareTo(RoadHazard other)
			{
				return this.ID.CompareTo(other.ID);
			}

			public bool Equals(RoadHazard other)
			{
				if (this.ID == other.ID)
					return true;
				return object.Equals(this, other);
			}
		}

		public static SortedSet<RoadHazard> Hazards;

		public static void DrawRoadHazards(Graphics g, Point center, Size size, bool drawTopography, bool drawTraffic, bool drawRailway, bool drawPolice, bool drawOthers)
		{
			if (Hazards != null)
			{
				foreach (var h in Hazards)
				{
					//if ((h.Type == RoadHazard.RoadHazardType.Topography && drawTopography) || (h.Type == RoadHazard.RoadHazardType.Traffic && drawTraffic) || (h.Type == RoadHazard.RoadHazardType.Railway && drawRailway) || (h.Type == RoadHazard.RoadHazardType.Police && drawPolice) || (h.Type == RoadHazard.RoadHazardType.Other && drawOthers))
					if (h.Enabled)
					{
						h.Draw(g, center, size);
					}
				}
			}
		}

		#endregion
	}

	// Data received from the GPS server
	public class GpsData
	{
		private readonly MainForm _form;

		public Size Size = new Size(4200, 3350);
		public PointF Center = new PointF(-0.053f, 0.041f);

		public PointF MapPosition
		{
			get
			{
				return new PointF(this.X / this.Size.Width + this.Center.X, -this.Z / this.Size.Height + this.Center.Y);
			}
		}

		public GpsData(MainForm form)
		{
			this._form = form;
		}

		public float X = 0;         // West-east position
		public float Y = 0;         // Height above lake Peräjärvi
		public float Z = 0;         // North-south position
		public float Heading = 0;   // Angle from north in degrees
		public float Speed = 0;     // Displayed speed of the car
		public string ResponseString;   // The raw string received from the server
		public bool Success = false;    // Indicates whether the request was successful
		public HttpStatusCode Status;   // The status code of the response

		private XmlDocument _doc = new XmlDocument();
		//<GpsData>
		//	<X>1009.916</X>
		//	<Y>-0.8313327</Y>
		//	<Z>-738.0518</Z>
		//	<Heading>10</Heading>
		//	<Speed>30</Speed>
		//	<Time>0</Time>
		//</GpsData>

		private readonly HttpClient _client = new HttpClient();
		public async Task Get(string url)
		{
			HttpResponseMessage response = await _client.GetAsync(url);
			Success = response.IsSuccessStatusCode;
			Status = response.StatusCode;
			if (response.IsSuccessStatusCode)
			{
				ResponseString = await response.Content.ReadAsStringAsync();
				_doc.LoadXml(ResponseString);
				float.TryParse(_doc.DocumentElement["X"].InnerText.Trim(), out X);
				float.TryParse(_doc.DocumentElement["Y"].InnerText.Trim(), out Y);
				float.TryParse(_doc.DocumentElement["Z"].InnerText.Trim(), out Z);
				float.TryParse(_doc.DocumentElement["Heading"].InnerText.Trim(), out Heading);
				float.TryParse(_doc.DocumentElement["Speed"].InnerText.Trim(), out Speed);
			}
			_form.UpdateGpsData();
		}
	}
}
