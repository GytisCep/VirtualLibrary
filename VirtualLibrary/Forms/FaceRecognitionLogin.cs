﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using VirtualLibrary.DataSources.Data;

namespace VirtualLibrary.Forms
{
    public partial class FaceRecognitionLogin : Form
    {
        private readonly VideoCapture _capture;
        EigenFaceRecognition faceRecognition;

        public FaceRecognitionLogin()
        {
            _capture = new VideoCapture();

            List<string> nicknames;
            List<Image<Gray, byte>> trainingSet;
            int[] labels;

            UserInformationInXmlFiles xml = new UserInformationInXmlFiles(new DirectoryInfo(Application.StartupPath).Parent.Parent.FullName + "\\UserInformation\\", 5);
            xml.GetTrainingSet(out trainingSet, out labels, out nicknames);

            faceRecognition = new EigenFaceRecognition(new DirectoryInfo(Application.StartupPath).Parent.Parent.FullName +
                                                                                    "\\UserInformation\\haarcascade_frontalface_alt2.xml",
                                                                               trainingSet, nicknames, Constants.FaceImagesPerUser);


            InitializeComponent();
        }

        private void StartRecognitionTimer_Tick(object sender, EventArgs e)
        {

            Image<Bgr, Byte> display = _capture.QueryFrame().ToImage<Bgr, Byte>();
            string currentNickname = faceRecognition.Recognize(display);

            if (currentNickname != null)
            {
                loginButton.Text = "Log in as " + currentNickname;
                nameLabel.Text = currentNickname;
                StaticDataSource.CurrUser = currentNickname;
            }
            else
            {
                nameLabel.Text = "Unknown";
            }

            cameraBox.Image = display;
        }


        private void LoginButton_Click(object sender, EventArgs e)
        {
            var library = new Library();
            library.Show();
            _capture.Dispose();
            Dispose();
            Close();
        }
    }
}