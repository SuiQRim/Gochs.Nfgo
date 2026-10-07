namespace Gochs.Nfgo.DTOs.Common;

/// <summary>
/// Стандартный ответ API при ошибке.
/// </summary>
public class ErrorResponseDto
{
    /// <summary>
    /// HTTP-код ошибки.
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Краткое описание категории ошибки.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Подробное сообщение об ошибке.
    /// </summary>
    public string Detail { get; set; } = null!;
}
