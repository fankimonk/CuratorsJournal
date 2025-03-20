namespace Frontend.Services
{
    public class JournalState
    {
        private readonly TaskCompletionSource _tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public int JournalId { get; private set; }

        public int CurrentPageId { get; private set; }

        public void Initialize(int journalId, int pageId)
        {
            JournalId = journalId;
            CurrentPageId = pageId;
            _tcs.TrySetResult();
        }

        public Task WaitForInit() => _tcs.Task;
    }
}
