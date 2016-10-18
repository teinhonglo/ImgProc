﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace TestWD1
{
    public partial class Form1 : Form
    {
        static string uploadPath = System.Environment.CurrentDirectory + "\\Origin\\";
        static string savePath = System.Environment.CurrentDirectory + "\\Output\\";
        static string readPath = System.Environment.CurrentDirectory + "\\Read\\";
        static bool isFst = true;

        public Form1()
        {
            InitializeComponent();
            originImg.SizeMode = PictureBoxSizeMode.Zoom;
            procImg.SizeMode = PictureBoxSizeMode.Zoom;
            
        }
        private void UploadBt_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string ImgPath = openFileDialog1.FileName;
                try
                {
                    if (Directory.Exists(uploadPath) == false) Directory.CreateDirectory(uploadPath);
                    if (Directory.Exists(savePath) == false) Directory.CreateDirectory(savePath);
                    if (Directory.Exists(readPath) == false) Directory.CreateDirectory(readPath);

                    MemoryStream ms = new MemoryStream();
                    using (FileStream fs = new FileStream(ImgPath, FileMode.Open))
                    {
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        ms.Write(buffer, 0, buffer.Length);
                    }

                    Bitmap bitmap = new Bitmap(ms);
                    System.Console.WriteLine(uploadPath + "origin.png");
                    System.Console.WriteLine(savePath + "output.png");
                    // Dirty Code
                    bitmap.Save(readPath + "origin.png");
                    originImg.Load(readPath + "origin.png");
                    bitmap.Save(readPath + "output.png");
                    procImg.Load(readPath + "output.png");

                    // Save Image
                    bitmap.Save(uploadPath + "origin.png");
                    bitmap.Save(savePath + "output.png");
                    bitmap.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                originImg.Load(uploadPath + "origin.png");
                procImg.Load(savePath + "output.png");

            }
        }

        private void HistogramBT_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)Image.FromFile(uploadPath + "origin.png");
            int[] ImgData = new int[256];
            LockBitmap lockBitmap = new LockBitmap(bmp);
            lockBitmap.LockBits();
            Color compareClr = Color.FromArgb(255, 255, 255, 255);
            for (int y = 0; y < lockBitmap.Height; y++)
            {
                for (int x = 0; x < lockBitmap.Width; x++)
                {
                    int val = (int)((lockBitmap.GetPixel(x, y).R + lockBitmap.GetPixel(x, y).G + lockBitmap.GetPixel(x, y).B) / 3.0);
                    ImgData[val]++;
                }
            }
            lockBitmap.UnlockBits();
            System.Console.WriteLine(lockBitmap.Height * lockBitmap.Width);
            Bitmap Histogram = null;
            drawHistogram(ref ImgData, 256, 300, out Histogram);
            bmp.Dispose();

            // Dirty Code
            Histogram.Save(readPath + "Histogram.png");
            procImg.Load(readPath + "Histogram.png");

            Histogram.Save(savePath + "Histogram.png");
            procImg.Load(savePath + "Histogram.png");
            Histogram.Dispose();
        }

        private void Guassion_NoiseBT_Click(object sender, EventArgs e)
        {
            string value = "10";
            int stdVal = 0;
            if (InputBox.InputBoxImpement("STD", "", ref value) == DialogResult.OK)
            {
                stdVal = Convert.ToInt32(value);
            }
            Bitmap bmp = (Bitmap)Image.FromFile(uploadPath + "origin.png");

            // Dirty code
            System.Console.WriteLine(savePath + "Guassian_Histogram.png");
            System.Console.WriteLine(File.Exists(savePath + "Guassian_Histogram.png"));
            if (File.Exists(savePath + "Guassian_Histogram.png") == true)
            {
                Bitmap bmp_histogram = (Bitmap)Image.FromFile(savePath + "Guassian_Histogram.png");
                bmp_histogram.Save(readPath + "Guassian_Histogram.png");
                procImg.Load(readPath + "Guassian_Histogram.png");
                bmp_histogram.Dispose();
            }
            
            
            Bitmap G_Bmp = AddNoise(bmp, stdVal, 0);

            // Dirty Code
            G_Bmp.Save(readPath + "Guassian_Noise.png");
            originImg.Load(readPath + "Guassian_Noise.png");

            G_Bmp.Save(savePath + "Guassian_Noise.png");
            G_Bmp.Dispose();

            originImg.Load(savePath + "Guassian_Noise.png");
            procImg.Load(savePath + "Guassian_Histogram.png");
        }


        // Add Guassian Noise to NewBitmap and Return it.
        public static Bitmap AddNoise(Bitmap OriginalImage, int stdDev, int mean)
        {
            int[] GuassianVal = new int[8 * stdDev + 1];
            System.Drawing.Imaging.PixelFormat format = OriginalImage.PixelFormat;
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height, format);
            LockBitmap newBitmap = new LockBitmap(NewBitmap);
            LockBitmap oldBitmap = new LockBitmap(OriginalImage);
            newBitmap.LockBits();
            oldBitmap.LockBits();
            Random TempRandom = new Random();

            for (int y = 0; y < newBitmap.Height; y++)
            {
                for (int x = 0; x < newBitmap.Width; x++)
                {
                    double u1 = TempRandom.NextDouble(); //these are uniform(0,1) random doubles
                    double u2 = TempRandom.NextDouble();
                    double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                 Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                    int randNormal =
                                 (int)(mean + stdDev * randStdNormal); //random normal(mean,stdDev^2)

                    randNormal = randNormal > 4 * stdDev ? 4 * stdDev : randNormal;
                    randNormal = randNormal < -4 * stdDev ? -4 * stdDev : randNormal;

                    GuassianVal[randNormal + 4 * stdDev]++;
                    int R = oldBitmap.GetPixel(x, y).R + randNormal;
                    int G = oldBitmap.GetPixel(x, y).G + randNormal;
                    int B = oldBitmap.GetPixel(x, y).B + randNormal;
                    // Normalize
                    R = R > 255 ? 255 : R;
                    R = R < 0 ? 0 : R;
                    G = G > 255 ? 255 : G;
                    G = G < 0 ? 0 : G;
                    B = B > 255 ? 255 : B;
                    B = B < 0 ? 0 : B;
                    // Set Guassian White Noise
                    Color TempValue = Color.FromArgb(R, G, B);
                    newBitmap.SetPixel(x, y, TempValue);
                }
            }
            newBitmap.UnlockBits();
            oldBitmap.UnlockBits();
            Bitmap CuassianNoise = null;
            GuassianVal[GuassianVal.Length / 2] /= 2;
            drawHistogram(ref GuassianVal, GuassianVal.Length, 300, out CuassianNoise);

            CuassianNoise.Save(savePath + "Guassian_Histogram.png");
            return NewBitmap;
        }
        //Draw Histogram
        private static bool drawHistogram(ref int[] datas, int width, int height, out Bitmap myimage)
        {
            int WIDTH_BUF = width / 11;
            int HEIGHT_BUF = height / 10;
            myimage = new Bitmap(width + WIDTH_BUF * 2, height + HEIGHT_BUF);
            Graphics g = Graphics.FromImage(myimage);
            Pen pen = new Pen(Color.Blue);
            int[] cpDatas = new int[datas.Length];
            int MAX_VALUE = getMaxVal(ref datas);

            Array.Copy(datas, 0, cpDatas, 0, datas.Length);
            if (MAX_VALUE > height)
            {
                for (int i = 0; i < cpDatas.Length; i++)
                {
                    cpDatas[i] = System.Convert.ToInt32(cpDatas[i] * height / MAX_VALUE);
                }
            }
            else
            {
                height = MAX_VALUE;
            }

            for (int i = 0; i < cpDatas.Length; i++)
            {
                g.DrawLine(pen, i + WIDTH_BUF, height - cpDatas[i] + height / 10, i + WIDTH_BUF, myimage.Height);
            }

            return true;
        }

        private static int getMaxVal(ref int[] datas)
        {
            int maxVal = 0;
            for (int i = 0; i < datas.Length; i++)
            {
                if (datas[i] > maxVal) maxVal = datas[i];
            }
            return maxVal;
        }

        private void clrSpaceBT_Click(object sender, EventArgs e)
        {
            Bitmap originBitmap = new Bitmap(uploadPath + "origin.png");
            Bitmap samplingBtimap = samplingQuarter(originBitmap);
            samplingBtimap.Save(savePath + "QuarterGray.png");
            Bitmap c1 = mergeImg(samplingBtimap, samplingBtimap, 0);
            Bitmap c2 = mergeImg(samplingBtimap, samplingBtimap, 0);
            Bitmap output = mergeImg(c1, c1, 1);
            output.Save(readPath + "ColorSpace.png");
            procImg.Load(readPath + "ColorSpace.png");
            output.Save(savePath + "ColorSpace.png");
            procImg.Load(savePath + "ColorSpace.png");
            
        }

        public static void RGB2HSV(int R, int G, int B, ref double h, ref double s, ref double v)
        {
            double r = (double)R / 255;
            double g = (double)G / 255;
            double b = (double)B / 255;

            double min, max, delta;

            min = Math.Min(r, Math.Min(g, b));
            max = Math.Max(r, Math.Max(g, b));

            v = max;//Get V value
            delta = max - min;
            if (max != 0)
            {
                s = delta / max;
                if (r == max)
                {
                    if (g < b)
                        h = (g - b) / delta + 6;
                }
                else
                {
                    if (g == max)
                    {
                        h = (b - r) / delta + 2;
                    }
                    else
                    {
                        h = (r - g) / delta + 4;
                    }
                }
                h *= 60;
                s = s * 100;
                v = v * 100;
            }
            else
            {
                s = 0;
                h = 0;
            }
        }

        private static Bitmap samplingQuarter(Bitmap b1){
            Bitmap NewBitmap = new Bitmap(b1.Width, b1.Height, b1.PixelFormat);
            LockBitmap newBitmap = new LockBitmap(NewBitmap);
            LockBitmap oldBitmap = new LockBitmap(b1);
            newBitmap.LockBits();
            oldBitmap.LockBits();
            for (int x = 0; x < oldBitmap.Width; x+=2)
            {
                for (int y = 0; y < oldBitmap.Height; y += 2)
                {
                    int R = oldBitmap.GetPixel(x, y).R;
                    int G = oldBitmap.GetPixel(x, y).G;
                    int B = oldBitmap.GetPixel(x, y).B;
                    int GrayVal = (R + G + B) / 3;
                    Color tempColor = Color.FromArgb(GrayVal, GrayVal, GrayVal);
                    newBitmap.SetPixel(x, y, tempColor);
                }
            }
            newBitmap.UnlockBits();
            oldBitmap.UnlockBits();
            NewBitmap = ResizeImage(NewBitmap, NewBitmap.Width / 2, NewBitmap.Height / 2);
            
            return NewBitmap;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private Bitmap mergeImg(Bitmap b1, Bitmap b2, int type) {
            Bitmap mergeImg = default(Bitmap);
            PixelFormat Format = b1.PixelFormat;
            
            if (type == 0) 
            {
                //水平合併
                int width = b1.Width + b2.Width;
                int height = (b1.Height > b2.Height) ? b1.Height : b2.Height;
                Bitmap myBitmap = new Bitmap(width, height, Format);
                Graphics gr = Graphics.FromImage(myBitmap);
                //處理第一張圖片
                gr.DrawImage(b1, 0, 0);
                //處理第二張圖片
                gr.DrawImage(b2, b1.Width, 0);
                mergeImg = myBitmap;
                gr.Dispose();           
            }
            else
            {
                //垂直合併
                int width = (b1.Width > b2.Width) ? b1.Width : b2.Width;
                int height = b1.Height + b2.Height;
                Bitmap myBitmap = new Bitmap(width, height, Format);
                Graphics gr = Graphics.FromImage(myBitmap);
                //處理第一張圖片
                gr.DrawImage(b1, 0, 0);
                //處理第二張圖片
                gr.DrawImage(b2, 0, b1.Height);
                mergeImg = myBitmap;
                gr.Dispose();

            }
            return mergeImg;
        }

    }
}

