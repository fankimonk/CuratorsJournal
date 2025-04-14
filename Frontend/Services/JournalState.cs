using Contracts.Journal.Pages;
using Contracts.Journal;
using Microsoft.AspNetCore.Components;

namespace Frontend.Services
{
    public class JournalState(HttpClient httpClient, NavigationManager navigationManager, APIService apiService)
    {
        public int JournalId { get; private set; }

        public int CurrentPageId { get; private set; }

        public JournalContentsResponse? JournalContents { get; private set; }

        public LinkedListNode<PageResponse>? CurrentPageNode { get; private set; }
        public PageTypeResponse? CurrentPageType => CurrentPageNode?.Value.PageType;
        public bool IsCurrentPageApproved => CurrentPageNode != null && CurrentPageNode.Value.IsApproved;

        public Dictionary<int, bool> PageTypesNavLinksExpanded = [];

        public Action? OnInitialize;
        public Action? OnFecthContents;
        public Action? OnToggleIsApproved;

        private readonly HttpClient _httpClient = httpClient;
        private readonly NavigationManager _navigationManager = navigationManager;
        private readonly APIService _apiService = apiService;
        private LinkedList<PageResponse> _pages = [];

        public void Reset()
        {
            JournalId = 0;
            CurrentPageId = 0;
            JournalContents = null;
            CurrentPageNode = null;
            PageTypesNavLinksExpanded.Clear();
        }

        public async Task Initialize(int journalId, int pageId)
        {
            CurrentPageId = pageId;

            if (JournalId != journalId)
            {
                JournalId = journalId;
                await FetchContents();

                if (JournalContents == null) return;

                PageTypesNavLinksExpanded.Clear();
                foreach (var pt in JournalContents!.PageTypes.Where(pt => pt.MaxPages != 1))
                {
                    PageTypesNavLinksExpanded.Add(pt.Id, false);
                }
            }

            UpdateCurrentPage();

            if (CurrentPageNode == null) return;

            if (PageTypesNavLinksExpanded.ContainsKey(CurrentPageNode!.Value!.PageType!.Id))
            {
                PageTypesNavLinksExpanded[CurrentPageNode.Value.PageType.Id] = true;
            }

            OnInitialize?.Invoke();
        }

        public async Task ToggleIsCurrentPageApproved()
        {
            if (CurrentPageNode == null) return;

            var response = await _httpClient.PutAsync("api/page/toggleisapproved/" + CurrentPageId, null);
            if (response.IsSuccessStatusCode)
            {
                var isApproved = await response.Content.ReadFromJsonAsync<bool>();
                CurrentPageNode.Value.IsApproved = isApproved;

                OnToggleIsApproved?.Invoke();
            }
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

        public async Task FetchContents()
        {
            Console.WriteLine("Fetch contents " + JournalId);

            JournalContents = null;
            CurrentPageNode = null;

            var hasAccess = await _apiService.GetFromJsonAsync<bool>("api/journal/verifyaccess/" + JournalId);
            if (!hasAccess)
            {
                _navigationManager.NavigateTo("/AccessDenied");
                return;
            }

            JournalContents = await _httpClient.GetFromJsonAsync<JournalContentsResponse>("api/journal/" + JournalId.ToString() + "/contents/");

            RefillPages();

            OnFecthContents?.Invoke();
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
                    return;
                }

                currentNode = currentNode.Next;
            }
            _navigationManager.NavigateTo("/Error");
        }
    }
}
