namespace Contracts.Journal.Traditions
{
    public class TraditionResponse(
        int id, string name, string participationForm, string? note, int day, int month)
    {
        public int Id { get; set; } = id;

        public string Name { get; set; } = name;
        public string ParticipationForm { get; set; } = participationForm;
        public string? Note { get; set; } = note;

        public int Day { get; set; } = day;
        public int Month { get; set; } = month;
    }
}
