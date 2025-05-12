namespace Frontend.Utils
{
    public record FileUploadProgress(string fileName, long Size)
    {
        public long UploadedBytes { get; set; }
        public double UploadedPercentage => (double)UploadedBytes / (double)Size * 100d;
    }
}
