using System;
using System.Collections.Generic;
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
        private readonly CharacterHelperProvider characterHelper;
        public MissionController(AppDbContext dbContext, CharacterHelperProvider characterHelper)
        {
            this.dbContext = dbContext;
            this.characterHelper = characterHelper;
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
                return this.MissionFinished(mission, character);
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

        private IActionResult MissionFinished(Mission mission, Character character)
        {
            this.CloseMission(mission, character);
            if (mission.Events != default)
            {
                MissionSkillReq missionSkillReq = SystemHelper.Deserialize<MissionSkillReq>(mission.Events);
                return Ok(this.Event(mission, missionSkillReq));
            }
            return Ok(Fight(character));
        }

        private void CloseMission(Mission mission, Character character)
        {
            this.RewardCharacter(character, mission);
            this.GenerateNewMissions(character);
        }

        private void RewardCharacter(Character character, Mission mission)
        {
            this.characterHelper.AddExp(character, 50 * ((int)mission.Rarity + 1));
        }

        private object Fight(Character character)
        {
            List<FightTurn> fightTurns = new List<FightTurn>();
            Monster monster = new Monster() { Health = 50, Damage = 5 };
            while (character.Health > 0 && monster.Health > 0)
            {
                monster.Health -= character.Damage;
                fightTurns.Add(new FightTurn() { DamageDealt = character.Damage, PlayerAttack = true });
                if (monster.Health <= 0)
                    break;
                character.Health -= monster.Damage;
                fightTurns.Add(new FightTurn() { DamageDealt = monster.Damage, PlayerAttack = false });
                if (character.Health <= 0)
                    break;
            }
            return new
            {
                turns = fightTurns,
                player = new
                {
                    health = character.Health
                },
                enemy = monster
            };
        }

        //Event occured, send it to client
        private object Event(Mission mission, MissionSkillReq missionSkillReq)
        {
            Event e = this.dbContext.Events.FirstOrDefault(e => e.Id == missionSkillReq.EventId);
            if (e == default)
            {
                mission.Events = null;
                return new Response(taleOfDungir.Models.Response.Error, "event does not exist");
            }
            return missionSkillReq;
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
            int amount = this.dbContext.Events.Count();
            int x = new Random().Next(0, amount);
            byte[] events = null;
            Int64 eventId = this.dbContext.Events.Select(e => e.Id).Skip(x).FirstOrDefault();
            if (eventId != default)
            {
                MissionSkillReq msr = new MissionSkillReq() { EventId = eventId, Value = 1 };
                events = SystemHelper.Serialize(msr);
            }
            return new Mission()
            {
                Name = "Dung " + new Guid().ToString().Take(4),
                CharacterId = character.CharacterId,
                Character = character,
                Rarity = RarityHelper.RandomRarity(),
                Duration = 10,
                Events = events
            };
        }
    }
}