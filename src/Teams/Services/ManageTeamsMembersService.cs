
using Teams.Repository;
using Teams.Models;
using Teams.Data;
using Teams.Security;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Teams.Services
{
    public class ManageTeamsMembersService : IManageTeamsMembersService
    {

        private readonly ICurrentUser _currentUser;
        private readonly IRepository<Team, int> _teamRepository;
        private readonly IRepository<TeamMember, int> _teamMemberRepository;

        public ManageTeamsMembersService(IRepository<Team, int> teamRepository, IRepository<TeamMember, int> memberRepository, ICurrentUser currentUser)
        {
            _currentUser = currentUser;
            _teamRepository = teamRepository;
            _teamMemberRepository = memberRepository;
        }
        
        public async Task<bool> RemoveAsync(int team_id, string member_id)
        {
            var team = await _teamRepository.GetAll().Where(t => t.Id == team_id).FirstOrDefaultAsync();
            if (team != null && team.TeamOwner == _currentUser.Current.Id() && MemberInTeam(team,member_id))
            {       
                return await _teamMemberRepository.DeleteAsync(team.TeamMembers.FirstOrDefault(t => t.MemberId == member_id));
            }
            return false;
        }
        
        private bool MemberInTeam(Team team, string member_id)
        {
            return team.TeamMembers.Any(t => t.MemberId == member_id);
        }

        public async Task<TeamMember> GetMemberAsync(int team_id, string member_id)
        {
             return await _teamMemberRepository.GetAll()
                    .Where(x => x.MemberId == member_id && x.TeamId == team_id)
                    .FirstOrDefaultAsync();

        }
    }
}