using System.Threading.Tasks;

namespace VietnameseTextCorrectionServer.Interfaces
{
    /// <summary>
    /// Giao diện định nghĩa dịch vụ xử lý văn bản.
    /// </summary>
    public interface ITextProcessingService
    {
        /// <summary>
        /// Phương thức xử lý văn bản đầu vào và trả về kết quả.
        /// </summary>
        /// <param name="inputText">Văn bản đầu vào cần xử lý.</param>
        /// <returns>Văn bản sau khi đã được xử lý.</returns>
        Task<string> ProcessAsync(string inputText);
    }
}
