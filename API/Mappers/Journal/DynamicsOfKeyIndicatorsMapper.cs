using Contracts.Journal.DynamicsOfKeyIndicators;
using Domain.Entities.JournalContent.DynamicsOfKeyIndicators;

namespace API.Mappers.Journal
{
    public static class DynamicsOfKeyIndicatorsMapper
    {
        public static DynamicsOfKeyIndicatorsRecordResponse ToResponse(this DynamicsOfKeyIndicatorsRecord record)
        {
            return new DynamicsOfKeyIndicatorsRecordResponse(
                record.Id, record.KeyIndicator!.ToResponse(), record.Note,
                record.KeyIndicatorsByCourse.Select(v => v.ToResponse()).ToList()
            );
        }

        public static KeyIndicatorResponse ToResponse(this KeyIndicator indicator)
        {
            return new KeyIndicatorResponse(
                indicator.Id, indicator.Name
            );
        }

        public static KeyIndicatorByCourseResponse ToResponse(this KeyIndicatorByCourse value)
        {
            return new KeyIndicatorByCourseResponse(
                value.Id, value.DynamicsRecordId, value.Course, value.Value
            );
        }

        //public static KeyIndicatorByCourse ToEntity(this CreateKeyIndicatorValueRequest request)
        //{
        //    return new KeyIndicatorByCourse
        //    {
        //        DynamicsRecordId = request.DynamicsRecordId,
        //        Course = request.Course,
        //        Value = request.Value
        //    };
        //}

        public static KeyIndicatorByCourse ToEntity(this UpdateKeyIndicatorValueRequest request)
        {
            return new KeyIndicatorByCourse
            {
                Value = request.Value
            };
        }

        public static DynamicsOfKeyIndicatorsRecord ToEntity(this UpdateDynamicsOfKeyIndicatorsRecordRequest request)
        {
            return new DynamicsOfKeyIndicatorsRecord
            {
                Note = request.Note
            };
        }
    }
}
