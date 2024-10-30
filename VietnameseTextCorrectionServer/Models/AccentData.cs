using Microsoft.ML.Data;

namespace VietnameseTextCorrectionServer.Models
{
    /// <summary>
    /// Lớp đại diện cho dữ liệu đầu vào cho mô hình ML.NET.
    /// </summary>
    public class AccentData
    {
        /// <summary>
        /// Văn bản đầu vào cần được xử lý (không dấu).
        /// </summary>
        [LoadColumn(0)]
        public string InputText { get; set; }

        /// <summary>
        /// Văn bản đầu ra mong muốn (có dấu) - sử dụng trong quá trình huấn luyện.
        /// </summary>
        [LoadColumn(1)]
        public string OutputText { get; set; }
    }
}
