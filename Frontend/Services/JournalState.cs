using Contracts.Journal.Pages;
using Contracts.Journal;
using Microsoft.AspNetCore.Components;

namespace Frontend.Services
{
    public class JournalState(HttpClient httpClient, NavigationManager navigationManager)
    {
        public int JournalId { get; private set; }

        public int CurrentPageId { get; private set; }

        public JournalContentsResponse? JournalContents { get; private set; }

        public LinkedListNode<PageResponse>? CurrentPageNode { get; private set; }

        public Dictionary<int, bool> PageTypesNavLinksExpanded = [];

        public Action? OnInitialize;

        private readonly HttpClient _httpClient = httpClient;
        private readonly NavigationManager _navigationManager = navigationManager;

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

        public void DeleteCurrentPage()
        {
            if (JournalContents == null) return;
            if (CurrentPageNode == null) return;

            var pageUri = $"/journal/{JournalContents.JournalId}/";
            PageResponse? page = null;
            if (CurrentPageNode.Previous != null)
            {
                page = _pages.FirstOrDefault(p => p.Id == CurrentPageNode.Previous.Value.Id);
            }
            else if (CurrentPageNode.Next != null)
            {
                page = _pages.FirstOrDefault(p => p.Id == CurrentPageNode.Next.Value.Id);
            }
            if (page == null) return;

            var pageType = JournalContents.PageTypes.FirstOrDefault(pt => pt.Id == CurrentPageNode.Value.PageType!.Id);
            if (pageType == null) return;

            pageType.Pages!.RemoveAll(p => p.Id == CurrentPageId);
            RefillPages();

            _navigationManager.NavigateTo(pageUri + page.PageType!.Name + "/" + page.Id);
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
