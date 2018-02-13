using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Png2Srf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);
        }

        void Form1_DragEnter(object sender, DragEventArgs e) {
          if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                if (!file.ToUpperInvariant().EndsWith(".PNG"))
                {
                    DropLabel.Text = "Dropped File is NOT a PNG File!\nCannot convert to SRF!";
                    DropLabel.ForeColor = Color.FromArgb(0, 255, 0, 0);
                    continue;
                }

                int TotalPolygons = 0;

                try
                {
                    DropLabel.Text = "Working...";
                    DropLabel.ForeColor = Color.FromArgb(0, 255, 220, 0);
                    this.Refresh();
                    Bitmap bitmap = new Bitmap(file);

                    if (bitmap.Width * bitmap.Height > (128 * 128))
                    {
                        DialogResult result = MessageBox.Show("That's a very large PNG file to convert! (> 128^2 pixels to be converted to SRF)... Are you sure about this?", "Uh Oh!", MessageBoxButtons.OKCancel);
                        if (result == DialogResult.Cancel)
                        {
                            DropLabel.Text = "Drop Input PNG Here!";
                            DropLabel.ForeColor = Color.FromArgb(0, 255, 255, 255);
                            return;
                        }
                    }

                    double ScalingFactor = (bitmap.Width > bitmap.Height) ? (1.0d / bitmap.Width) : (1.0d / bitmap.Height);
                    double ShiftFactorX = (bitmap.Width / 2) * ScalingFactor;
                    double ShiftFactorY = (bitmap.Height / 2) * ScalingFactor;

                    List<string> SRFFileOutput = new List<string>();
                    SRFFileOutput.Add("SURF");

                    //Add all the vertecies!
                    for (int i = 0; i < bitmap.Height; i++)
                    {
                        for (int j = 0; j < bitmap.Width; j++)
                        {
                            //Color pixel = bitmap.GetPixel(j, i);
                            SRFFileOutput.Add("V " + ((j + 0) * ScalingFactor - ShiftFactorX) + " 0 " + ((i + 0) * -ScalingFactor + ShiftFactorY));    //Top Left Vertex.
                            SRFFileOutput.Add("V " + ((j + 1) * ScalingFactor - ShiftFactorX) + " 0 " + ((i + 0) * -ScalingFactor + ShiftFactorY));    //Top Right Vertex.
                            SRFFileOutput.Add("V " + ((j + 1) * ScalingFactor - ShiftFactorX) + " 0 " + ((i + 1) * -ScalingFactor + ShiftFactorY));    //Bottom Right Vertex.
                            SRFFileOutput.Add("V " + ((j + 0) * ScalingFactor - ShiftFactorX) + " 0 " + ((i + 1) * -ScalingFactor + ShiftFactorY));    //Bottom Left Vertex.
                        }
                    }

                    //Add all the Faces!
                    for (int i = 0; i < bitmap.Height; i++)
                    {
                        for (int j = 0; j < bitmap.Width; j++)
                        {
                            Color pixel = bitmap.GetPixel(j, i);
                            if (pixel.A <= 0)
                            {
                                //Won't see this pixel, it will waste space in the model!
                                continue;
                            }
                            SRFFileOutput.Add("F");
                            SRFFileOutput.Add("C " + pixel.R + " " + pixel.G + " " + pixel.B);

                            double TopLeftVertexX = ((j + 0) * ScalingFactor - ShiftFactorX);    //Top Left Vertex X.
                            double TopLeftVertexY = ((i + 0) * -ScalingFactor + ShiftFactorY);    //Top Left Vertex Y.
                            double BottomRightVertexX = ((j + 1) * ScalingFactor - ShiftFactorX);    //Bottom Right Vertex X.
                            double BottomRightVertexY = ((i + 1) * -ScalingFactor + ShiftFactorY);    //Bottom Right Vertex Y.

                            double CenterX = (TopLeftVertexX + BottomRightVertexX) / 2;
                            double CenterY = (TopLeftVertexY + BottomRightVertexY) / 2;
                            SRFFileOutput.Add("N " + CenterX + " 0 " + CenterY + " 0.00 1.00 0.00");

                            int PolygonNumber = (i * bitmap.Width) + j;
                            SRFFileOutput.Add("V " + ((PolygonNumber * 4) + 0) + " " + ((PolygonNumber * 4) + 1) + " " + ((PolygonNumber * 4) + 2) + " " + ((PolygonNumber * 4) + 3));
                            SRFFileOutput.Add("E");
                            TotalPolygons++;
                        }
                    }

                    //Add all the alpha's!
                    int RejectedPolys = 0;
                    for (int i = 0; i < bitmap.Height; i++)
                    {
                        for (int j = 0; j < bitmap.Width; j++)
                        {
                            Color pixel = bitmap.GetPixel(j, i);
                            if (pixel.A <= 0)
                            {
                                RejectedPolys++;
                                continue;
                            }
                            if (pixel.A == 255)
                            {
                                //Valid poly, but skip the Alpha encoding as it's a waste of SRF space!
                                continue;
                            }
                            int PolygonNumber = (i * bitmap.Width) + j - RejectedPolys;
                            SRFFileOutput.Add("ZA " + PolygonNumber + " " + (255 - (pixel.A)));
                        }
                    }

                    //TotalPolygons = bitmap.Width * bitmap.Height;
                    SRFFileOutput.Add("E");
                    File.WriteAllLines(file.Substring(0, file.Length - 3) + "SRF", SRFFileOutput.ToArray());
                }
                catch
                {
                    DropLabel.Text = "Conversion Failed! Something Broke!";
                    DropLabel.ForeColor = Color.FromArgb(0, 255, 0, 0);
                }

                DropLabel.Text = "Done (" + TotalPolygons + " Polygons)!\nDrop Another PNG to Convert...";
                DropLabel.ForeColor = Color.FromArgb(0, 0, 255, 0);
            }
        }
    }
}
