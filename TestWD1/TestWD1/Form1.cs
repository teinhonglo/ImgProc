using System;
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
using Emgu.CV.Structure;
using Emgu.CV;
using System.Runtime.InteropServices;

namespace TestWD1
{
    public partial class Form1 : Form
    {
        static string uploadPath = System.Environment.CurrentDirectory + "\\Origin\\";
        static string savePath = System.Environment.CurrentDirectory + "\\Output\\";
        static string readPath = System.Environment.CurrentDirectory + "\\Read\\";
        static bool isFst = true;
        static Bitmap bitmap = null;

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
                    
                    bitmap = new Bitmap(ms);
                    System.Console.WriteLine("0");
                    // Dirty Code
                    bitmap.Save(readPath + "origin.png");
                    System.Console.WriteLine("1");
                    originImg.Load(readPath + "origin.png");
                    System.Console.WriteLine("2");
                    bitmap.Save(readPath + "output.png");
                    System.Console.WriteLine("3");
                    procImg.Load(readPath + "output.png");
                    System.Console.WriteLine("4");
                    // Save Image
                    bitmap.Save(uploadPath + "origin.png");
                    bitmap.Save(savePath + "output.png");
                    //bitmap.Dispose();
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
            try
            {
                originImg.Load(readPath + "origin.png");
                Bitmap bmp = (Bitmap)Image.FromFile(uploadPath + "origin.png");
                int[] ImgData = new int[256];
                LockBitmap lockBitmap = new LockBitmap(bmp);
                lockBitmap.LockBits();

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
                originImg.Load(uploadPath + "origin.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("請先上傳檔案");
            }
        }

        private void Guassion_NoiseBT_Click(object sender, EventArgs e)
        {
            try
            {
                originImg.Load(readPath + "origin.png");
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("請先上傳檔案");
            }
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
        // Color Space Button
        private void clrSpaceBT_Click(object sender, EventArgs e)
        {
            try
            {
                originImg.Load(readPath + "origin.png");
                Bitmap originBitmap = new Bitmap(uploadPath + "origin.png");
                Bitmap samplingBtimap = samplingQuarter(originBitmap, false);
                Bitmap samplingGrayBtimap = samplingQuarter(originBitmap, true);
                samplingBtimap.Save(savePath + "QuarterGray.png");
                string value = "0";
                int clrChoice = 0;

                if (InputBox.InputBoxImpement("Color Spacce", "Input(0 = CMYK, 1 = HSV, Other = RGB)", ref value) == DialogResult.OK)
                {
                    switch (value)
                    {
                        case "0":
                            {
                                clrChoice = 0;
                                break;
                            }
                        case "1":
                            {
                                clrChoice = 1;
                                break;
                            }
                        default:
                            {
                                clrChoice = 2;
                                break;
                            }
                    }
                }

                Bitmap x = new Bitmap(samplingBtimap.Width, samplingBtimap.Height, samplingBtimap.PixelFormat);
                Bitmap y = new Bitmap(samplingBtimap.Width, samplingBtimap.Height, samplingBtimap.PixelFormat);
                Bitmap z = new Bitmap(samplingBtimap.Width, samplingBtimap.Height, samplingBtimap.PixelFormat);
                colorSpaceConversion(samplingBtimap, clrChoice, ref x, ref y, ref z);
                x = convertGrayScale(x);
                y = convertGrayScale(y);
                z = convertGrayScale(z);
                Bitmap c1 = mergeImg(samplingGrayBtimap, x, 0);
                Bitmap c2 = mergeImg(y, z, 0);
                Bitmap output = mergeImg(c1, c2, 1);

                output.Save(readPath + "ColorSpace.png");
                procImg.Load(readPath + "ColorSpace.png");
                output.Save(savePath + "ColorSpace.png");
                procImg.Load(savePath + "ColorSpace.png");
                originImg.Load(uploadPath + "origin.png");
                originBitmap.Dispose();
                samplingBtimap.Dispose();
                samplingGrayBtimap.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("請先上傳檔案");
            }
        }
        public static void colorSpaceConversion(Bitmap b1, int type, ref Bitmap x_axis, ref Bitmap y_axis, ref Bitmap z_axis)
        {
            Rectangle rect = new Rectangle(0, 0, b1.Width, b1.Height);
            Bitmap myBitmap = b1.Clone(rect, b1.PixelFormat);
            LockBitmap oldBitmap = new LockBitmap(myBitmap);
            LockBitmap x_coor = new LockBitmap(x_axis);
            LockBitmap y_coor = new LockBitmap(y_axis);
            LockBitmap z_coor = new LockBitmap(z_axis);
            oldBitmap.LockBits();
            x_coor.LockBits();
            y_coor.LockBits();
            z_coor.LockBits();
            for (int x = 0; x < oldBitmap.Width; x++)
            {
                for (int y = 0; y < oldBitmap.Height; y++)
                {
                    int R = oldBitmap.GetPixel(x, y).R;
                    int G = oldBitmap.GetPixel(x, y).G;
                    int B = oldBitmap.GetPixel(x, y).B;
                    switch (type)
                    {
                        case 0:
                            {
                                //CMYK
                                double C = 0;
                                double M = 0;
                                double Y = 0;
                                double K = 0;

                                RGB2CMYK(R, G, B, ref C, ref M, ref Y, ref K);
                                C = C * 255;
                                M = M * 255;
                                Y = Y * 255;

                                C = (C > 255) ? 255 : C;
                                C = (C < 0) ? 0 : C;
                                M = (M > 255) ? 255 : M;
                                M = (M < 0) ? 0 : M;
                                Y = (Y > 255) ? 255 : Y;
                                Y = (Y < 0) ? 0 : Y;

                                Color tempVal = Color.FromArgb(255 - (int)C, G, B);
                                x_coor.SetPixel(x, y, tempVal);
                                tempVal = Color.FromArgb(R, 255 - (int)M, B);
                                y_coor.SetPixel(x, y, tempVal);
                                tempVal = Color.FromArgb(R, G, 255 - (int)Y);
                                z_coor.SetPixel(x, y, tempVal);
                                break;
                            }
                        case 1:
                            {
                                //HSV
                                double H = 0;
                                double S = 0;
                                double V = 0;

                                RGB2HSV(R, G, B, ref H, ref S, ref V);

                                H = (H > 255) ? 255 : H;
                                H = (H < 0) ? 0 : H;
                                S = (S > 255) ? 255 : S;
                                S = (S < 0) ? 0 : S;
                                V = (V > 255) ? 255 : V;
                                V = (V < 0) ? 0 : V;

                                Color tempVal = Color.FromArgb((int)H, G, B);
                                x_coor.SetPixel(x, y, tempVal);
                                tempVal = Color.FromArgb(R, (int)S, B);
                                y_coor.SetPixel(x, y, tempVal);
                                tempVal = Color.FromArgb(R, G, (int)V);
                                z_coor.SetPixel(x, y, tempVal);
                                break;
                            }
                        default:
                            {
                                //RGB

                                Color tempVal = Color.FromArgb(R, R, R);
                                x_coor.SetPixel(x, y, tempVal);
                                tempVal = Color.FromArgb(G, G, G);
                                y_coor.SetPixel(x, y, tempVal);
                                tempVal = Color.FromArgb(B, B, B);
                                z_coor.SetPixel(x, y, tempVal);
                                break;
                            }
                    }
                }
            }
            oldBitmap.UnlockBits();
            x_coor.UnlockBits();
            y_coor.UnlockBits();
            z_coor.UnlockBits();
            return;
        }

        public static void RGB2CMYK(int R, int G, int B, ref double c, ref double m, ref double y, ref double k)
        {
            // Normalize 0 ~ 1
            c = (double)(255 - R) / 255;
            m = (double)(255 - G) / 255;
            y = (double)(255 - B) / 255;

            k = (double)Math.Min(c, Math.Min(m, y));
            if (k == 1.0)
            {
                c = m = y = 0;
            }
            else
            {
                c = (c - k) / (1 - k);
                m = (m - k) / (1 - k);
                y = (y - k) / (1 - k);
            }
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

        private static Bitmap samplingQuarter(Bitmap b1, bool isGrayScale)
        {
            Bitmap NewBitmap = new Bitmap(b1.Width, b1.Height, b1.PixelFormat);
            LockBitmap newBitmap = new LockBitmap(NewBitmap);
            LockBitmap oldBitmap = new LockBitmap(b1);
            newBitmap.LockBits();
            oldBitmap.LockBits();
            for (int x = 0; x < oldBitmap.Width; x += 2)
            {
                for (int y = 0; y < oldBitmap.Height; y += 2)
                {
                    int R = oldBitmap.GetPixel(x, y).R;
                    int G = oldBitmap.GetPixel(x, y).G;
                    int B = oldBitmap.GetPixel(x, y).B;
                    int GrayVal = (R + G + B) / 3;
                    Color tempColor = (isGrayScale) ? Color.FromArgb(GrayVal, GrayVal, GrayVal) : Color.FromArgb(R, G, B);
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

        private Bitmap mergeImg(Bitmap b1, Bitmap b2, int type)
        {
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
        public static Bitmap convertGrayScale(Bitmap b1)
        {
            LockBitmap b = new LockBitmap(b1);
            b.LockBits();
            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    int R = b.GetPixel(x, y).R;
                    int G = b.GetPixel(x, y).G;
                    int B = b.GetPixel(x, y).B;
                    int GrayScale = (R + G + B) / 3;
                    Color tempColor = Color.FromArgb(GrayScale, GrayScale, GrayScale);
                    b.SetPixel(x, y, tempColor);
                }
            }
            b.UnlockBits();
            return b1;
        }

        private void FFT_BT_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bmp = new Bitmap(uploadPath + "origin.png");

                int width = bmp.Width;
                int height = bmp.Height;
                int widthPOWER_OF_2 = 1;
                int heightPOWER_OF_2 = 1;

                do
                {
                    widthPOWER_OF_2 *= 2;
                } while ((width /= 2) > 0);

                do
                {
                    heightPOWER_OF_2 *= 2;
                } while ((height /= 2) > 0);


                Bitmap largeBmp = new Bitmap(widthPOWER_OF_2, heightPOWER_OF_2, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                try
                {
                    using (Graphics largeGraphics = Graphics.FromImage(largeBmp))
                    {
                        largeGraphics.PageUnit = GraphicsUnit.Pixel;
                        largeGraphics.DrawImage(bmp, 0, 0);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                // create grayscale filter (BT709)
                AForge.Imaging.Filters.Grayscale filter = new AForge.Imaging.Filters.Grayscale(0.2125, 0.7154, 0.0721);
                // apply the filter
                Bitmap grayImage = filter.Apply(largeBmp);

                AForge.Imaging.ComplexImage cimage = AForge.Imaging.ComplexImage.FromBitmap(grayImage);
                cimage.ForwardFourierTransform();
                // get frequency view
                System.Drawing.Bitmap img = cimage.ToBitmap();
                procImg.Image = (Image)img;
                img.Save(savePath + "fft.png");
                originImg.Load(uploadPath + "origin.png");
                bmp.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void skinDetectionBT_Click(object sender, EventArgs e)
        {
            try
            {
                // Read image from upload path
                Bitmap bmp = new Bitmap(uploadPath + "origin.png");
                LockBitmap myBmp = new LockBitmap(bmp);
                myBmp.LockBits();
                for (int x = 0; x < myBmp.Width; x++)
                {
                    for (int y = 0; y < myBmp.Height; y++)
                    {
                        // RGB 
                        int R = myBmp.GetPixel(x, y).R;
                        int G = myBmp.GetPixel(x, y).G;
                        int B = myBmp.GetPixel(x, y).B;
                        // HSV 
                        double H = 0;
                        double S = 0;
                        double V = 0;

                        RGB2HSV(R, G, B, ref H, ref S, ref V);

                        Color color;
                        if (skinDetection(R, G, B))
                        {
                            color = Color.FromArgb(255, 255, 255);
                        }
                        else
                        {
                            color = Color.FromArgb(0, 0, 0);
                        }
                        myBmp.SetPixel(x, y, color);
                    }
                }
                myBmp.UnlockBits();
                originImg.Load(uploadPath + "origin.png");
                // Dirty code
                bmp.Save(readPath + "skin.png");
                procImg.Load(readPath + "skin.png");
                bmp.Save(savePath + "skin.png");
                procImg.Load(savePath + "skin.png");
                bmp.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static bool skinDetection(int r, int g, int b)
        {
            var Cb = (0.148 * r) - (0.291 * g) + (0.439 * b) + 128;
            var Cr = (0.439 * r) - (0.368 * g) - (0.071 * b) + 128;

            if ((r > 95 && g > 40 && b > 20
                && (Math.Max(Math.Max(r, g), b) - Math.Min(Math.Min(r, g), b) > 15)
                && (r - g) > 15 && r > g && r > b)
                && (Cr > 140 && Cr < 162 || Cb > 105 && Cb < 130))
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        private void EqualizationBT_Click(object sender, EventArgs e)
        {
            try
            {
                // Read image from upload path
                Bitmap bmp = new Bitmap(uploadPath + "origin.png");
                PixelFormat format = bmp.PixelFormat;
                Bitmap gray_bmp = new Bitmap(bmp.Width, bmp.Height, format);
                Bitmap Histogram;
                Bitmap Histogram_equalization;
                LockBitmap myBmp = new LockBitmap(bmp);
                LockBitmap my_gray_bmp = new LockBitmap(gray_bmp);
                myBmp.LockBits();
                my_gray_bmp.LockBits();
                int width = myBmp.Width;
                int height = myBmp.Height;
                int[] GrayLevel = new int[256];

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        // RGB 
                        int R = myBmp.GetPixel(x, y).R;
                        int G = myBmp.GetPixel(x, y).G;
                        int B = myBmp.GetPixel(x, y).B;

                        int GrayVal = (R + G + B) / 3;
                        GrayLevel[GrayVal]++;

                        Color color = Color.FromArgb(GrayVal, GrayVal, GrayVal);
                        my_gray_bmp.SetPixel(x, y, color);
                    }
                }
                myBmp.UnlockBits();
                my_gray_bmp.UnlockBits();

                int old_MaxValue = 0;
                int cdf_min = Int32.MaxValue;
                int[] CDF = new int[GrayLevel.Length];
                CDF[0] = GrayLevel[0];
                for (int i = 1; i < GrayLevel.Length; i++)
                {
                    CDF[i] = GrayLevel[i] + CDF[i - 1];
                    if (CDF[i] != 0 && CDF[i] < cdf_min)
                    {
                        cdf_min = CDF[i];
                    }
                    if (GrayLevel[i] > old_MaxValue)
                    {
                        old_MaxValue = GrayLevel[i];
                    }

                }

                int M = width;
                int N = height;
                int L = 256;
                int[] h = new int[GrayLevel.Length];

                for (int v = 0; v < h.Length; v++)
                {
                    double m = CDF[v] - cdf_min;
                    double d = M * N - cdf_min;
                    h[v] = (int)Math.Round((m / d) * (L - 1));
                    h[v] = (h[v] >= 0) ? h[v] : 0;
                }

                int[] new_GrayLevel = new int[GrayLevel.Length];
                int new_MaxValue = 0;

                for (int i = 0; i < GrayLevel.Length; i++)
                {
                    new_GrayLevel[h[i]] += GrayLevel[i];
                    if (new_GrayLevel[h[i]] > new_MaxValue)
                    {
                        new_MaxValue = new_GrayLevel[h[i]];
                    }
                }

                Bitmap NewBitmap = new Bitmap(bmp.Width, bmp.Height, format);
                LockBitmap newBitmap = new LockBitmap(NewBitmap);
                LockBitmap oldBitmap = new LockBitmap(gray_bmp);
                newBitmap.LockBits();
                oldBitmap.LockBits();
                for (int x = 0; x < oldBitmap.Width; x++)
                {
                    for (int y = 0; y < oldBitmap.Height; y++)
                    {
                        int R = oldBitmap.GetPixel(x, y).R;
                        int G = oldBitmap.GetPixel(x, y).G;
                        int B = oldBitmap.GetPixel(x, y).B;
                        int new_gray = h[R];
                        Color color = Color.FromArgb(new_gray, new_gray, new_gray);
                        newBitmap.SetPixel(x, y, color);
                    }
                }
                newBitmap.UnlockBits();
                oldBitmap.UnlockBits();

                Bitmap originBmp;
                Bitmap procBmp;
                drawHistogram(ref GrayLevel, GrayLevel.Length, 300, out Histogram);
                drawHistogram(ref new_GrayLevel, new_GrayLevel.Length, 300, out Histogram_equalization);
                gray_bmp.Save(savePath + "Gray_level.png");
                NewBitmap.Save(savePath + "HistogramEqualization.png");
                Histogram_equalization.Save(savePath + "HistogramEqualization_histogram.png");

                gray_bmp = ResizeImage(gray_bmp, 300, gray_bmp.Height * 300 / gray_bmp.Width);
                Histogram = ResizeImage(Histogram, 300, Histogram.Height * 300 / Histogram.Width);
                NewBitmap = ResizeImage(NewBitmap, 300, NewBitmap.Height * 300 / NewBitmap.Width);
                Histogram_equalization = ResizeImage(Histogram_equalization, 300, Histogram_equalization.Height * 300 / Histogram_equalization.Width);
                originBmp = mergeImg(gray_bmp, Histogram, 1);
                procBmp = mergeImg(NewBitmap, Histogram_equalization, 1);

                // Dirty code
                originBmp.Save(readPath + "HistogramEqualization_origin.png");
                originImg.Load(readPath + "HistogramEqualization_origin.png");
                originBmp.Save(savePath + "HistogramEqualization_origin.png");
                originImg.Load(savePath + "HistogramEqualization_origin.png");

                procBmp.Save(readPath + "HistogramEqualization_proc.png");
                procImg.Load(readPath + "HistogramEqualization_proc.png");
                procBmp.Save(savePath + "HistogramEqualization_proc.png");
                procImg.Load(savePath + "HistogramEqualization_proc.png");
                bmp.Dispose();
                Histogram.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

        private void ImgSmoothingBT_Click(object sender, EventArgs e)
        {

            double[,] filter = new double[3, 3];
            double factor = 1;
            double[,] Gaussian_3x3 = new double[3, 3] {  {1.0, 2.0, 1.0},
                                                          {2.0, 4.0, 2.0},
                                                          {1.0, 2.0, 1.0}};

            double[,] Averaging_3x3 = new double[3, 3] {  {1.0, 1.0, 1.0},
                                                          {1.0, 1.0, 1.0},
                                                          {1.0, 1.0, 1.0}};

            double[,] Gaussian_5x5 = new double[5, 5]    { {  1,   4,  6,  4,  1 },
                                                            {  4,  16, 24, 16,  4 },
                                                            {  6,  24, 36, 24,  6 },
                                                            {  4,  16, 24, 16,  4 },
                                                            {  1,   4,  6,  4,  1 }, };

            string value = "";
            try
            {
                Bitmap bmp = new Bitmap(uploadPath + "origin.png");
                //GrayLeval(ref bmp);
                if (InputBox.InputBoxImpement("Filter", "Input(0 = Gaussian_3x3, 1 = Averaging_3x3, Other = Gaussian_5x5)", ref value) == DialogResult.OK)
                {
                    switch (value)
                    {
                        case "0":
                            {
                                filter = Gaussian_3x3;
                                factor = 1 / 16.0;
                                break;
                            }
                        case "1":
                            {
                                filter = Averaging_3x3;
                                factor = 1 / 9.0;
                                break;
                            }
                        default:
                            {
                                filter = Gaussian_5x5;
                                factor = 1 / 256.0;
                                break;
                            }
                    }
                }
                Bitmap smoothing;
                smoothing = Extension.ConvolutionFilter(bmp, filter, factor);
                bmp.Save(readPath + "garyBmp.png");
                originImg.Load(readPath + "garyBmp.png");
                bmp.Save(savePath + "garyBmp.png");
                originImg.Load(savePath + "garyBmp.png");

                smoothing.Save(readPath + "smoothing.png");
                procImg.Load(readPath + "smoothing.png");
                smoothing.Save(savePath + "smoothing.png");
                procImg.Load(savePath + "smoothing.png");
                bmp.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

        private void EdgeDectectionBT_Click(object sender, EventArgs e)
        {
            double[,] filter = new double[3, 3];
            double[,] laplacian5x5 = new double[5, 5] {   {  0,  0, -1,  0,  0 },
                                                            {  0, -1, -2, -1,  0 },
                                                            { -1, -2, 16, -2, -1 },
                                                            {  0, -1, -2, -1,  0 },
                                                            {  0,  0, -1,  0,  0 } };

            double[,] laplacian3x3 = new double[3, 3] {    { -1, -1, -1, },
                                                            { -1,  8, -1, },
                                                            { -1, -1, -1, }, };

            double factor = 1.0;
            string value = "";
            try
            {
                Bitmap bmp = new Bitmap(uploadPath + "origin.png");
                GrayLeval(ref bmp);
                if (InputBox.InputBoxImpement("Filter", "Input(0 = laplacian5x5, 1 = laplacian3x3, Other = laplacian5x5)", ref value) == DialogResult.OK)
                {
                    switch (value)
                    {
                        case "0":
                            {
                                filter = laplacian5x5;
                                break;
                            }
                        case "1":
                            {
                                filter = laplacian3x3;
                                break;
                            }
                        default:
                            {
                                filter = laplacian5x5;
                                break;
                            }
                    }
                }
                Bitmap result;
                result = Extension.ConvolutionFilter(bmp, filter, factor);

                bmp.Save(readPath + "garyBmp.png");
                originImg.Load(readPath + "garyBmp.png");
                bmp.Save(savePath + "garyBmp.png");
                originImg.Load(savePath + "garyBmp.png");

                result.Save(readPath + "edgeDetection.png");
                procImg.Load(readPath + "edgeDetection.png");
                result.Save(savePath + "edgeDetection.png");
                procImg.Load(savePath + "edgeDetection.png");
                bmp.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }
        /*
        private Bitmap Convolution(Bitmap origin, double[,] filter)
        {
            int fltWidth = filter.GetLength(0);
            int fltHeight = filter.GetLength(1);
            int newWidth = origin.Width;
            int newHeight = origin.Height;

            PixelFormat newFormat = origin.PixelFormat;
            Bitmap NewBitmap = new Bitmap(newWidth, newHeight, newFormat);
            LockBitmap newBitmap = new LockBitmap(NewBitmap);
            LockBitmap originBitmap = new LockBitmap(origin);

            newBitmap.LockBits();
            originBitmap.LockBits();
            for (int x = 0; x < newBitmap.Width; x++)
            {
                for (int y = 0; y < newBitmap.Height; y++)
                {
                    double localSum = 0;
                    for (int fx = 0; fx < fltWidth; fx++)
                    {
                        for (int fy = 0; fy < fltHeight; fy++)
                        {
                            int scanVal = 0;
                            if (x + fx >= originBitmap.Width
                                || y + fy >= originBitmap.Height)
                            {
                                scanVal = 0;
                            }
                            else
                            {
                                int R = originBitmap.GetPixel(x + fx, y + fy).R;
                                int G = originBitmap.GetPixel(x + fx, y + fy).G;
                                int B = originBitmap.GetPixel(x + fx, y + fy).B;
                                int Gray = (R + G + B) / 3;
                                scanVal = Gray;

                            }

                            localSum += scanVal * filter[fx, fy];
                        }
                    }
                    localSum = (localSum > 255) ? 255 : localSum;
                    localSum = (localSum < 0) ? 0 : localSum;
                    int normalSum = (int)localSum;

                    Color color = Color.FromArgb(normalSum, normalSum, normalSum);
                    newBitmap.SetPixel(x, y, color);
                }
            }
            newBitmap.UnlockBits();
            originBitmap.UnlockBits();

            return NewBitmap;
        }*/

        private void GrayLeval(ref Bitmap origin)
        {
            LockBitmap lockOrigin = new LockBitmap(origin);
            lockOrigin.LockBits();
            for (int x = 0; x < lockOrigin.Width; x++)
            {
                for (int y = 0; y < lockOrigin.Height; y++)
                {
                    int R = lockOrigin.GetPixel(x, y).R;
                    int G = lockOrigin.GetPixel(x, y).G;
                    int B = lockOrigin.GetPixel(x, y).B;
                    int Gray = (R + G + B) / 3;
                    lockOrigin.SetPixel(x, y, Color.FromArgb(Gray, Gray, Gray));
                }
            }
            lockOrigin.UnlockBits();
        }

        private void handDetection_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bmp = new Bitmap(bitmap);
                double[,] Gaussian_5x5 = new double[5, 5]    { {  1,   4,  6,  4,  1 },
                                                            {  4,  16, 24, 16,  4 },
                                                            {  6,  24, 36, 24,  6 },
                                                            {  4,  16, 24, 16,  4 },
                                                            {  1,   4,  6,  4,  1 } };


                bmp = Extension.ConvolutionFilter(bmp, Gaussian_5x5, 1 / 256.0);
                // Clone a portion of the Bitmap object.
                Rectangle cloneRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                System.Drawing.Imaging.PixelFormat format = bmp.PixelFormat;
                Bitmap skinBmp = bmp.Clone(cloneRect, format);

                int[] hSkin = new int[bmp.Width];
                int[] vSkin = new int[bmp.Height];
                LockBitmap srcBmp = new LockBitmap(bmp);
                LockBitmap mySkinBmp = new LockBitmap(skinBmp);
                mySkinBmp.LockBits();
                for (int x = 0; x < mySkinBmp.Width; x++)
                {
                    for (int y = 0; y < mySkinBmp.Height; y++)
                    {
                        // RGB 
                        int R = mySkinBmp.GetPixel(x, y).R;
                        int G = mySkinBmp.GetPixel(x, y).G;
                        int B = mySkinBmp.GetPixel(x, y).B;
                        // HSV 
                        double H = 0;
                        double S = 0;
                        double V = 0;

                        RGB2HSV(R, G, B, ref H, ref S, ref V);

                        Color color;
                        if (skinDetection(R, G, B))
                        {
                            color = Color.FromArgb(255, 255, 255);
                            hSkin[x]++;
                            vSkin[y]++;
                        }
                        else
                        {
                            color = Color.FromArgb(0, 0, 0);
                        }
                        mySkinBmp.SetPixel(x, y, color);
                    }
                }
                int maxHVal = hSkin.Max();
                int maxHIdx = hSkin.ToList().IndexOf(maxHVal);

                int maxVVal = vSkin.Max();
                int maxVIdx = vSkin.ToList().IndexOf(maxVVal);

                int minY = Int32.MaxValue;
                int maxY = 0;
                int minX = Int32.MaxValue;
                int maxX = 0;
                //MessageBox.Show(maxHVal + ":" + maxHIdx);
                //MessageBox.Show(maxVVal + ":" + maxVIdx);

                isFst = true;
                for (int y = 0; y < srcBmp.Height; y++)
                {
                    int R = mySkinBmp.GetPixel(maxHIdx, y).R;
                    if (R == 255)
                    {
                        if (y < minY)
                        {
                            minY = y;
                        }
                        if (y > maxY)
                        {
                            maxY = y;
                        }
                        //mySkinBmp.SetPixel(maxHIdx, y, Color.FromArgb(200, 0, 0));
                        srcBmp.SetPixel(maxHIdx, y, Color.FromArgb(200, 0, 0));
                    }
                }
                bool isFindCenter = false;
                bool isDetectSkin = false;
                for (int x = 0; x < srcBmp.Width; x++)
                {

                    int R = mySkinBmp.GetPixel(x, maxVIdx).R;
                    int G = mySkinBmp.GetPixel(x, maxVIdx).G;
                    int B = mySkinBmp.GetPixel(x, maxVIdx).B;

                    if (R == 255)
                    {
                        isDetectSkin = true;
                        if (x < minX)
                        {
                            minX = x;
                        }
                        if (x > maxX)
                        {
                            maxX = x;
                        }
                        srcBmp.SetPixel(x, maxVIdx, Color.FromArgb(0, 200, 0));
                    }
                    else
                    {
                        if (isFindCenter)
                        {
                            break;
                        }
                    }

                    if (R == 200)
                    {
                        isFindCenter = true;
                    }

                }


                srcBmp.UnlockBits();
                mySkinBmp.UnlockBits();

                Pen bluePen = new Pen(Color.Blue, 5);
                Rectangle rect = new Rectangle(minX, minY, maxX - minX, maxY - minY);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawRectangle(bluePen, rect);
                    g.Dispose();
                }

                // Dirty code
                bmp.Save(readPath + "hand.png");
                procImg.Load(readPath + "hand.png");
                bmp.Save(savePath + "hand.png");
                procImg.Load(savePath + "hand.png");

                bmp.Dispose();
                skinBmp.Dispose();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cannyEdgeBT_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bmp = new Bitmap(readPath + "origin.png");
                Bitmap cannyImage = cannyEdgeDetection(bmp);

                // Dirty code
                cannyImage.Save(readPath + "canny.png");
                procImg.Load(readPath + "canny.png");
                cannyImage.Save(savePath + "canny.png");
                procImg.Load(savePath + "canny.png");
                bmp.Dispose();
                cannyImage.Dispose();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
         }

        private Bitmap cannyEdgeDetection(Bitmap bmp) {
            Image<Gray, Byte> image = new Image<Gray, Byte>(bmp);
            Image<Gray, Byte> cannyImage = image.Canny(125, 255);
            cannyImage._ThresholdBinary(new Gray(128), new Gray(350));
            return cannyImage.ToBitmap();
        }
    }

    public static class Extension
    {
        public static Bitmap ConvolutionFilter(this Bitmap sourceBitmap, double[,] filter, double factor = 1.0)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);


            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;


            int filterWidth = filter.GetLength(1);
            int filterHeight = filter.GetLength(0);


            int filterOffset = (filterWidth - 1) / 2;
            int calcOffset = 0;


            int byteOffset = 0;


            for (int offsetY = filterOffset; offsetY <
                 sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX <
                     sourceBitmap.Width - filterOffset; offsetX++)
                {
                    blue = 0;
                    green = 0;
                    red = 0;


                    byteOffset = offsetY *
                                    sourceData.Stride +
                                    offsetX * 4;


                    for (int filterY = -filterOffset;
                         filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset;
                             filterX <= filterOffset; filterX++)
                        {


                            calcOffset = byteOffset +
                                         (filterX * 4) +
                                         (filterY * sourceData.Stride);


                            blue += (double)(pixelBuffer[calcOffset]) *
                                     filter[filterY + filterOffset,
                                     filterX + filterOffset];


                            green += (double)(pixelBuffer[calcOffset + 1]) *
                                      filter[filterY + filterOffset,
                                      filterX + filterOffset];


                            red += (double)(pixelBuffer[calcOffset + 2]) *
                                    filter[filterY + filterOffset,
                                    filterX + filterOffset];
                        }
                    }


                    blue = factor * blue + 0.0;
                    green = factor * green + 0.0;
                    red = factor * red + 0.0;


                    if (blue > 255)
                    { blue = 255; }
                    else if (blue < 0)
                    { blue = 0; }


                    if (green > 255)
                    { green = 255; }
                    else if (green < 0)
                    { green = 0; }


                    if (red > 255)
                    { red = 255; }
                    else if (red < 0)
                    { red = 0; }


                    resultBuffer[byteOffset] = (byte)(blue);
                    resultBuffer[byteOffset + 1] = (byte)(green);
                    resultBuffer[byteOffset + 2] = (byte)(red);
                    resultBuffer[byteOffset + 3] = 255;
                }
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }
    }
}

