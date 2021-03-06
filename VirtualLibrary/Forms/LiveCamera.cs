﻿using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

using VirtualLibrary.DataSources.Data;

namespace VirtualLibrary
{
    public partial class LiveCamera : Form
    {
        private VideoCapture _capture;
        private readonly CascadeClassifier _cascade;

        public Image<Gray, byte>[] GrayPictures;
        public byte[][] Pictures;

        public LiveCamera()
        {
            _cascade = new CascadeClassifier(new DirectoryInfo(Application.StartupPath).Parent.Parent.FullName +
                                            "\\UserInformation\\haarcascade_frontalface_alt2.xml");

            GrayPictures = new Image<Gray, byte>[Constants.FaceImagesPerUser];
            Pictures = new byte[Constants.FaceImagesPerUser][];

            InitializeComponent();
        }

        private void StartTakingPictures(object sender, EventArgs e)
        {
            MessageBox.Show("Look to the camera for 3 seconds", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Mat img1;
            Image<Bgr, byte> nextFrame;

            _capture = new VideoCapture();


            for (int i = 0; i < Constants.FaceImagesPerUser; i++)
            {
                img1 = _capture.QueryFrame();
                nextFrame = img1.ToImage<Bgr, byte>();
                {
                    if (nextFrame != null)
                    {
                        imageBox1.Image = nextFrame;

                        var grayframe = nextFrame.Convert<Gray, byte>();
                        var faces = _cascade.DetectMultiScale(grayframe, 1.2, 1);

                        try
                        {
                            GrayPictures[i] = grayframe.Copy(faces[0]).Resize(100, 100, Inter.Cubic);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            //Console.WriteLine(e.Message);
                            MessageBox.Show("Face was not detected. Try again", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }

                    Thread.Sleep(500);
                }

                //change imageBox variables to array
                switch (i)
                {
                    case 0:
                        imageBox1.Image = GrayPictures[i];
                        break;
                    case 1:
                        imageBox2.Image = GrayPictures[i];
                        break;
                    case 2:
                        imageBox3.Image = GrayPictures[i];
                        break;
                    case 3:
                        imageBox4.Image = GrayPictures[i];
                        break;
                    case 4:
                        imageBox5.Image = GrayPictures[i];
                        break;
                }
                
                    
            }

            _capture.Dispose();
        }

        


        private void ContinueButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Constants.FaceImagesPerUser; i++)
            {
                Pictures[i] = GrayPictures[i].Bytes;
            }

            Close();
        }

        private void LiveCamera_Load(object sender, EventArgs e)
        {

        }
    }
}