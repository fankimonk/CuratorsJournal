namespace Contracts.Journal.EducationalProcessSchedule
{
    public class EducationalProcessSchedulePageResponse(int pageId, List<EducationalProcessScheduleRecordResponse> educationalProcessSchedule)
    {
        public int PageId { get; set; } = pageId;

        public List<EducationalProcessScheduleRecordResponse> EducationalProcessSchedule { get; set; } = educationalProcessSchedule;
    }
}
