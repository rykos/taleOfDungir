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

        [HttpGet]
        [Route("active/event/{eventActionId}")]
        public IActionResult MissionEventAction(Int64 eventActionId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Mission mission = this.dbContext.Users.Include(u => u.Character.Missions)
                .ThenInclude(m => m.Events).FirstOrDefault(u => u.Id == userId).Character.Missions.FirstOrDefault(m => m.Started);
            if (mission == default)
            {
                return BadRequest();
            }
            MissionSkillReq msr = SystemHelper.Deserialize<MissionSkillReq>(mission.Events);
            if (msr.Equals(default))
            {
                return BadRequest();
            }
            Event e = this.dbContext.Events.Include(e => e.EventActions).FirstOrDefault(e => e.Id == msr.EventId);
            EventAction ea = e.EventActions.FirstOrDefault(ea => ea.Id == eventActionId);
            Character character = this.dbContext.Characters.Include(c => c.Skills).FirstOrDefault(c => c.ApplicationUserId == userId);
            this.dbContext.Update(mission);
            mission.EventsFinished = true;
            this.dbContext.SaveChanges();
            //Success
            if (CheckStat(character, ea, msr.EventActionIdToValue[eventActionId]))
            {
                this.MissionFinished(mission, character);
                return Ok(new Response(Models.Response.Success, "Event finished with a success"));
            }
            //Failure
            else
            {
                return this.MissionFinished(mission, character);
            }
        }

        private bool CheckStat(Character character, EventAction eventAction, int value)
        {
            if (eventAction.SkillName == "Vitality")
            {
                return Compare(character.Skills.Vitality, value);
            }
            else if (eventAction.SkillName == "Vitality")
            {
                return Compare(character.Skills.Vitality, value);
            }
            else if (eventAction.SkillName == "Luck")
            {
                return Compare(character.Skills.Vitality, value);
            }
            else if (eventAction.SkillName == "Perception")
            {
                return Compare(character.Skills.Vitality, value);
            }
            return false;
        }

        private bool Compare(int skillValue, int value)
        {
            if (skillValue >= value)
            {
                return true;
            }
            return false;
        }

        private IActionResult MissionFinished(Mission mission, Character character)
        {
            //No event
            if (mission.Events == default)
            {
                this.CloseMission(mission, character);
                return Ok(this.Fight(character));
            }
            //Event not finished
            else if (mission.Events != default && mission.EventsFinished == false)
            {
                MissionSkillReq missionSkillReq = SystemHelper.Deserialize<MissionSkillReq>(mission.Events);
                return Ok(this.Event(mission, missionSkillReq));
            }
            this.CloseMission(mission, character);
            return Ok();
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
                won = (character.Health > 0),
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
            Event e = this.dbContext.Events.Include(e => e.EventActions).FirstOrDefault(e => e.Id == missionSkillReq.EventId);
            if (e == default)
            {
                mission.Events = null;
                return new Response(taleOfDungir.Models.Response.Error, "event does not exist");
            }
            return new
            {
                Event = e,
                Msr = new
                {
                    missionSkillReq.EventId,
                    EventActionIdToValue = missionSkillReq.EventActionIdToValue.ToArray()
                }
            };
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
            Event e = this.dbContext.Events.Include(e => e.EventActions).Skip(x).FirstOrDefault();
            if (e.Id != default)
            {
                int eventActionCount = e.EventActions.Count();
                Dictionary<Int64, int> values = new Dictionary<Int64, int>();
                for (int i = 0; i < eventActionCount; i++)
                {
                    values.Add(e.EventActions[i].Id, i + 10);
                }
                MissionSkillReq msr = new MissionSkillReq()
                {
                    EventId = e.Id,
                    EventActionIdToValue = values
                };
                events = SystemHelper.Serialize(msr);
            }
            return new Mission()
            {
                Name = "Dung " + Guid.NewGuid().ToString().Substring(0, 4),
                CharacterId = character.CharacterId,
                Character = character,
                Rarity = RarityHelper.RandomRarity(),
                Duration = 10,
                Events = events
            };
        }
    }
}