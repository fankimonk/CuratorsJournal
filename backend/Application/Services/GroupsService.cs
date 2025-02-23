using Application.Interfaces;
using DataAccess.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class GroupsService : IGroupsService
    {
        private readonly IGroupsRepository _groupsRepository;
        private readonly ISpecialtiesRepository _specialtiesRepository;

        private readonly IJournalsService _journalsService;

        public GroupsService(IGroupsRepository groupsRepository, ISpecialtiesRepository specialtiesRepository, IJournalsService journalsService)
        {
            _groupsRepository = groupsRepository;
            _specialtiesRepository = specialtiesRepository;
            _journalsService = journalsService;
        }

        public async Task<Group?> CreateGroup(string number, int specialtyId, int admissionYear)
        {
            var group = new Group { Number = number, AdmissionYear = admissionYear, SpecialtyId = specialtyId, CuratorId = null };
            var createdGroup = await _groupsRepository.CreateAsync(group);

            if (createdGroup == null) return null;

            await _journalsService.CreateJournal(createdGroup.Id);
            return createdGroup;
        }

        public async Task<Group?> AppointCurator(int groupId, int curatorId)
        {
            var updatedGroup = await _groupsRepository.UpdateCuratorAsync(groupId, curatorId);
            return updatedGroup;
        }
    }
}
