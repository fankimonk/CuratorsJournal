namespace Contracts.Journal.Files
{
    public record DownloadFileRequest
    (
        int JournalId,
        string FileName
    );
}
