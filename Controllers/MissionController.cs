using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taleOfDungir.Data;
using taleOfDungir.Helpers;
using taleOfDungir.Models;

namespace taleOfDungir.Controllers
{
    [Authorize]
    [ApiController]
    [Route("missions")]
    public class MissionController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public MissionController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetMissions()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Character character = this.dbContext.Users.Include(u => u.Character).ThenInclude(c => c.Missions).FirstOrDefault(u => u.Id == userId).Character;
            //Mission is already active
            Mission activeMission;
            if ((activeMission = character.Missions.FirstOrDefault(m => m.Started)) != default)
            {
                return Ok(new
                {
                    activeMission.Id,
                    activeMission.Name,
                    activeMission.Rarity,
                    activeMission.CharacterId,
                    activeMission.Duration
                });
            }
            if (character.Missions.Count < 3)
            {
                this.GenerateNewMissions(character);
            }
            return Ok(character.Missions.Select(m => new { m.Id, m.Name, m.Rarity, m.CharacterId, m.Duration }));
        }

        [HttpGet]
        [Route("start/{id}")]
        public IActionResult StartMission(Int64 id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Character character = this.dbContext.Users.Include(u => u.Character).ThenInclude(c => c.Missions).FirstOrDefault(u => u.Id == userId).Character;
            Mission selectedMission;
            //mission does not exist
            if ((selectedMission = character.Missions.FirstOrDefault(m => m.Id == id)) == default)
            {
                return BadRequest();
            }
            //one mission is already started
            if (character.Missions.FirstOrDefault(m => m.Started) != default)
            {
                return BadRequest();
            }
            dbContext.Update(selectedMission);
            selectedMission.Started = true;
            selectedMission.StartTime = DateTime.Now;
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("active")]
        public IActionResult GetActiveMission()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Mission mission = this.dbContext.Users.Include(u => u.Character.Missions).FirstOrDefault(u => u.Id == userId).Character.Missions.FirstOrDefault(m => m.Started);
            //No active mission
            if (mission == default)
            {
                return Ok();
            }
            //Mission finished
            else if (DateTime.Now > mission.StartTime.Value.AddSeconds(mission.Duration))
            {
                Character character = this.dbContext.Users.Include(u => u.Character.Missions).FirstOrDefault(u => u.Id == userId).Character;
                this.GenerateNewMissions(character);
                return Ok();
            }
            //Active mission
            else
            {
                return Ok(new
                {
                    mission.Id,
                    mission.Name,
                    mission.Rarity,
                    mission.Started,
                    mission.StartTime,
                    mission.Duration
                });
            }
        }

        private void GenerateNewMissions(Character character)
        {
            this.dbContext.Update(character);
            character.Missions.Clear();
            for (int i = 0; i < 3; i++)
            {
                character.Missions.Add(NewMission(character));
            }
            this.dbContext.SaveChanges();
        }

        private Mission NewMission(Character character)
        {
            return new Mission() { Name = "Dung", CharacterId = character.CharacterId, Character = character, Rarity = RarityHelper.RandomRarity(), Duration = 10 };
        }
    }
}