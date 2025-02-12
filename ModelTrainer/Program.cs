﻿using System;
using Microsoft.ML;
using Microsoft.ML.Data;
using VietnameseTextCorrectionServer.Models;

namespace ModelTrainer
{
    class Program
    {
        private static readonly string _dataPath = @"Data\accent_data.csv";
        private static readonly string _modelPath = @"MLModels\AccentModel.zip";

        static void Main(string[] args)
        {
            // Tạo thư mục lưu mô hình nếu chưa tồn tại
            System.IO.Directory.CreateDirectory("MLModels");

            // Khởi tạo MLContext
            var mlContext = new MLContext();

            // Bước 1: Tải dữ liệu
            Console.WriteLine("Đang tải dữ liệu...");
            IDataView dataView = mlContext.Data.LoadFromTextFile<AccentData>(
                path: _dataPath,
                hasHeader: true,
                separatorChar: ',');

            // Bước 2: Chia dữ liệu thành tập huấn luyện và kiểm tra
            Console.WriteLine("Đang chia dữ liệu thành tập huấn luyện và kiểm tra...");
            var trainTestData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            var trainingData = trainTestData.TrainSet;
            var testingData = trainTestData.TestSet;

            // Bước 3: Xây dựng pipeline
            Console.WriteLine("Đang xây dựng pipeline...");
            var pipeline = mlContext.Transforms.Text.FeaturizeText(
                    outputColumnName: "Features",
                    inputColumnName: nameof(AccentData.InputText))
                .Append(mlContext.Transforms.Conversion.MapValueToKey(
                    outputColumnName: "Label",
                    inputColumnName: nameof(AccentData.OutputText)))
                .AppendCacheCheckpoint(mlContext)
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
                .Append(mlContext.Transforms.Conversion.MapKeyToValue(
                    outputColumnName: "PredictedLabel",
                    inputColumnName: "PredictedLabel"));

            // Bước 4: Huấn luyện mô hình
            Console.WriteLine("Đang huấn luyện mô hình...");
            var model = pipeline.Fit(trainingData);

            // Bước 5: Đánh giá mô hình
            Console.WriteLine("Đang đánh giá mô hình...");
            var predictions = model.Transform(testingData);
            var metrics = mlContext.MulticlassClassification.Evaluate(
                data: predictions,
                labelColumnName: "Label",
                predictedLabelColumnName: "PredictedLabel");

            // In kết quả đánh giá
            Console.WriteLine($"Độ chính xác (MicroAccuracy): {metrics.MicroAccuracy:P2}");
            Console.WriteLine($"Độ chính xác (MacroAccuracy): {metrics.MacroAccuracy:P2}");
            Console.WriteLine($"Log Loss: {metrics.LogLoss:F2}");

            // Bước 6: Lưu mô hình
            Console.WriteLine("Đang lưu mô hình...");
            mlContext.Model.Save(model, trainingData.Schema, _modelPath);
            Console.WriteLine($"Mô hình đã được lưu tại {_modelPath}");

            Console.WriteLine("Huấn luyện mô hình hoàn tất.");
        }
    }
}
