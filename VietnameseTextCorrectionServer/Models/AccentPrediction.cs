using Microsoft.ML.Data;

namespace VietnameseTextCorrectionServer.Models
{
    /// <summary>
    /// Lớp đại diện cho kết quả dự đoán của mô hình ML.NET.
    /// </summary>
    public class AccentPrediction
    {
        /// <summary>
        /// Văn bản sau khi đã được mô hình dự đoán (có dấu).
        /// </summary>
        [ColumnName("PredictedLabel")]
        public string PredictedText { get; set; }
    }
}
