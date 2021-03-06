﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;

namespace VirtualLibrary
{
    class EigenFaceRecognition
    {
        private const int THRESHOLD = 3000;
        private const int DISTANCE = 3000;
        private const int COMPONENTS_NUMBER = 80;

        private List<Image<Gray, byte>> trainingSet;
        private List<string> namesList;
        private int faceImagesPerUser;

        private FaceRecognizer recognizer;
        private CascadeClassifier cascade;

        public EigenFaceRecognition(string faceDetectionTrainingFilePath, List<Image<Gray, byte>> trainingSet, List<string> namesList, int faceImagesPerUser)
        {
            //check if parameters are okey

            this.trainingSet = trainingSet;
            this.namesList = namesList;
            this.faceImagesPerUser = faceImagesPerUser;

            recognizer = new EigenFaceRecognizer(COMPONENTS_NUMBER, THRESHOLD);
            cascade = new CascadeClassifier(faceDetectionTrainingFilePath);

            Train();
        }



        public string Recognize(Image<Bgr, Byte> display)
        {
            Rectangle[] faces = cascade.DetectMultiScale(display.Convert<Gray, Byte>(), 1.2, 0);
            Image<Gray, Byte> faceImage;

            try
            {
                faceImage = display.Convert<Gray, Byte>().Copy(faces[0]).Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            FaceRecognizer.PredictionResult result = recognizer.Predict(faceImage);

            //For testing purpose
            Console.WriteLine(result.Distance);

            if (result.Distance <= 3000)
            {
                return namesList.ElementAt(result.Label / 5);
            }
            else
            {
                return null;
            }
        }



        public void AddUser(List<Image<Gray, byte>> faceImages, string name)
        {
            if (faceImages.Count < faceImagesPerUser)
            {
                //return something
            }

            namesList.Add(name);
            for (int i = 0; i < faceImagesPerUser; i++)
            {
                trainingSet.Add(faceImages.ElementAt(i));
            }

            Train();
        }


        private void Train()
        {
            List<int> labelsList = new List<int>();
            int label = 0;

            foreach (Image<Gray, byte> grayFaceImage in trainingSet)
            {
                labelsList.Add(label);
                label++;
            }
            recognizer.Train(trainingSet.ToArray(), labelsList.ToArray());
        }
    }
}
