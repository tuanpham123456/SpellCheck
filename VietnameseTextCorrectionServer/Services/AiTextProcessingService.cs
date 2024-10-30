using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ML;
using VietnameseTextCorrectionServer.Interfaces;
using VietnameseTextCorrectionServer.Models;

namespace VietnameseTextCorrectionServer.Services
{
    public class AiTextProcessingService : ITextProcessingService
    {
        private static readonly string ModelPath = @"D:\SpellCheck\ModelTrainer\MLModels\AccentModel.zip";

        //private static readonly string ModelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MLModels", "AccentModel.zip");
        private readonly MLContext _mlContext;
        private readonly ITransformer _mlModel;
        private readonly PredictionEngine<AccentData, AccentPrediction> _predictionEngine;

        public AiTextProcessingService()
        {
            // Khởi tạo MLContext
            _mlContext = new MLContext();

            // Kiểm tra sự tồn tại của tệp mô hình
            if (!File.Exists(ModelPath))
            {
                throw new FileNotFoundException($"Không tìm thấy mô hình ML.NET tại đường dẫn: {ModelPath}");
            }

            // Tải mô hình đã huấn luyện
            using (var stream = new FileStream(ModelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _mlModel = _mlContext.Model.Load(stream, out var modelInputSchema);
            }

            // Tạo PredictionEngine
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<AccentData, AccentPrediction>(_mlModel);
        }

        public Task<string> ProcessAsync(string inputText)
        {
            if (string.IsNullOrWhiteSpace(inputText))
            {
                return Task.FromResult(string.Empty);
            }

            var input = new AccentData { InputText = inputText };

            try
            {
                var prediction = _predictionEngine.Predict(input);
                return Task.FromResult(prediction.PredictedText);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần thiết
                Console.WriteLine($"Lỗi khi xử lý văn bản: {ex.Message}");
                // Trả về văn bản gốc nếu có lỗi
                return Task.FromResult(inputText);
            }
        }
    }
}
