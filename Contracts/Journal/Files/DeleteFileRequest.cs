namespace Contracts.Journal.Files
{
    public record DeleteFileRequest
    (
        int JournalId,
        string FileName
    );
}
