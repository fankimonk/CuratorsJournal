namespace Contracts.Journal.Pages
{
    public record PageTypeResponse
    (
        int Id,
        string Name,
        List<PageResponse>? Pages
    );
}
