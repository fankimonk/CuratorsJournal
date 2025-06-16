using Application.Interfaces;
using DataAccess.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class GroupsService : IGroupsService
    {
        private readonly IGroupsRepository _groupsRepository;
        private readonly ICuratorsAppointmentHistoryRepository _curatorsAppointmentHistoryRepository;
        private readonly IJournalsService _journalsService;

        public GroupsService(IGroupsRepository groupsRepository, IJournalsService journalsService, ICuratorsAppointmentHistoryRepository curatorsAppointmentHistoryRepository)
        {
            _groupsRepository = groupsRepository;
            _journalsService = journalsService;
            _curatorsAppointmentHistoryRepository = curatorsAppointmentHistoryRepository;
        }

        public async Task<Group?> CreateGroup(string number, int specialtyId, int admissionYear, int? curatorId)
        {
            var group = new Group { Number = number, AdmissionYear = admissionYear, 
                SpecialtyId = specialtyId, CuratorId = curatorId };
            var createdGroup = await _groupsRepository.CreateAsync(group);
            if (createdGroup == null) return null;
            if (createdGroup.CuratorId != null) 
                await _curatorsAppointmentHistoryRepository.CreateAsync(new CuratorsAppointmentHistoryRecord
                {
                    GroupId = createdGroup.Id,
                    CuratorId = (int)createdGroup.CuratorId,
                    AppointmentDate = DateOnly.FromDateTime(DateTime.Now)
                });
            await _journalsService.CreateJournal(createdGroup.Id);
            return createdGroup;
        }

        public async Task<Group?> AppointCurator(int groupId, int? curatorId)
        {
            var updatedGroup = await _groupsRepository.UpdateCuratorAsync(groupId, curatorId);
            return updatedGroup;
        }
    }
}
