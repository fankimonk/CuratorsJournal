using Contracts.AcademicYears;

namespace Contracts.Journal.SocioPedagogicalCharacteristics
{
    public record SocioPedagogicalCharacteristicsAttributesResponse
    (
        int Id,
        AcademicYearResponse? AcademicYear
    );
}
