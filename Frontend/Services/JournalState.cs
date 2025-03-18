namespace Frontend.Services
{
    public class JournalState
    {
        private readonly TaskCompletionSource _tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public int JournalId { get; private set; }

        public int CurrentPageId { get; private set; }

        public Action? OnPageChange { get; set; }

        public void SetPageId(int pageId)
        {
            CurrentPageId = pageId;
            OnPageChange?.Invoke();
        }

        public void SetJournalId(int journalId)
        {
            JournalId = journalId;
            _tcs.TrySetResult();
        }

        public Task WaitForJournalIdAsync() => _tcs.Task;
    }
}
