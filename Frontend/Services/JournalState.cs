using Contracts.Journal.Pages;
using Contracts.Journal;

namespace Frontend.Services
{
    public class JournalState(HttpClient httpClient)
    {
        public int JournalId { get; private set; }

        public int CurrentPageId { get; private set; }

        public JournalContentsResponse? JournalContents { get; private set; }

        public LinkedListNode<PageResponse>? CurrentPageNode { get; private set; }

        public Dictionary<int, bool> PageTypesNavLinksExpanded = [];

        public Action? OnInitialize;

        private readonly HttpClient _httpClient = httpClient;

        private LinkedList<PageResponse> _pages = [];

        public async Task Initialize(int journalId, int pageId)
        {
            CurrentPageId = pageId;

            if (JournalId != journalId)
            {
                JournalId = journalId;
                await FetchContents();

                PageTypesNavLinksExpanded.Clear();
                foreach (var pt in JournalContents!.PageTypes.Where(pt => pt.MaxPages != 1))
                {
                    PageTypesNavLinksExpanded.Add(pt.Id, false);
                }
            }

            UpdateCurrentPage();

            if (PageTypesNavLinksExpanded.ContainsKey(CurrentPageNode!.Value!.PageType!.Id))
            {
                PageTypesNavLinksExpanded[CurrentPageNode.Value.PageType.Id] = true;
            }

            OnInitialize?.Invoke();
        }

        public void AddPage(PageResponse page)
        {
            if (JournalContents == null) return;

            JournalContents.PageTypes.FirstOrDefault(pt => pt.Id == page.PageType!.Id)!.Pages!.Add(page);
            RefillPages();
        }

        private async Task FetchContents()
        {
            Console.WriteLine("Fetch contents " + JournalId);

            JournalContents = null;
            CurrentPageNode = null;

            JournalContents = await _httpClient.GetFromJsonAsync<JournalContentsResponse>("api/journal/" + JournalId.ToString() + "/contents/");

            RefillPages();
        }

        private void RefillPages()
        {
            _pages = [];

            foreach (var pt in JournalContents!.PageTypes)
            {
                foreach (var page in pt.Pages!)
                {
                    _pages.AddLast(page);
                }
            }
        }

        private void UpdateCurrentPage()
        {
            var currentNode = _pages.First;
            while (currentNode != null)
            {
                if (currentNode.Value.Id == CurrentPageId)
                {
                    CurrentPageNode = currentNode;
                    break;
                }

                currentNode = currentNode.Next;
            }
        }
    }
}
