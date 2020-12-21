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
                return Ok(this.MissionFinished(mission, character));
            }
            //Active mission
            else
            {
                return Ok(new MissionResoult("mission", new
                {
                    mission.Id,
                    mission.Name,
                    mission.Rarity,
                    mission.Started,
                    mission.StartTime,
                    mission.Duration
                }));
            }
        }

        [HttpGet]
        [Route("active/event/{eventActionId}")]
        public IActionResult MissionEventAction(Int64 eventActionId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Int64 charactedId = this.dbContext.Users.Select(u => new { u.Id, u.CharacterId }).FirstOrDefault(u => u.Id == userId).CharacterId;
            Mission mission = this.dbContext.Missions.FirstOrDefault(m => m.CharacterId == charactedId && m.Started);
            if (mission == default)
            {
                return BadRequest();
            }
            MissionSkillReq msr = SystemHelper.Deserialize<MissionSkillReq>(mission.Events);
            if (msr.Equals(default))
            {
                return BadRequest();
            }
            Event missionEvent = this.dbContext.Events.Include(e => e.EventActions).FirstOrDefault(e => e.Id == msr.EventId);
            EventAction eventAction = missionEvent.EventActions.FirstOrDefault(ea => ea.Id == eventActionId);
            Character character = this.dbContext.Characters.Include(c => c.Skills).FirstOrDefault(c => c.ApplicationUserId == userId);
            this.dbContext.Update(mission);
            mission.EventsFinished = true;
            this.dbContext.SaveChanges();
            //Success
            if (eventActionId != -1 && CheckStat(character, eventAction, msr.EventActionIdToValue[eventActionId]))
            {
                return Ok(new MissionResoult("finished", new
                {
                    won = true,
                    reward = this.MissionFinished(mission, character, true)
                }));
            }
            //Failure
            else
            {
                return Ok(new MissionResoult("finished", new
                {
                    won = false,
                    reward = this.MissionFinished(mission, character, false)
                }));
            }
        }

        private bool CheckStat(Character character, EventAction eventAction, int value)
        {
            if (eventAction.SkillName == "Vitality")
            {
                return Compare(character.Skills.Vitality, value);
            }
            else if (eventAction.SkillName == "Combat")
            {
                return Compare(character.Skills.Combat, value);
            }
            else if (eventAction.SkillName == "Luck")
            {
                return Compare(character.Skills.Luck, value);
            }
            else if (eventAction.SkillName == "Perception")
            {
                return Compare(character.Skills.Perception, value);
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

        private object MissionFinished(Mission mission, Character character, bool won = false)
        {
            //No event, finished
            if (mission.Events == default)
            {
                FightResoult fr = Fight(character);
                this.CloseMission(mission, character);
                return new MissionResoult("finished", new
                {
                    fight = fr,
                    reward = this.RewardCharacter(character, mission, fr.Won)
                });
            }
            //Event not finished
            else if (mission.Events != default && mission.EventsFinished == false)
            {
                MissionSkillReq missionSkillReq = SystemHelper.Deserialize<MissionSkillReq>(mission.Events);
                return new MissionResoult("event", this.TransportEvent(mission, missionSkillReq));
            }
            //Event finished
            else
            {
                object rewardValue = this.RewardCharacter(character, mission, true);
                this.CloseMission(mission, character);
                return rewardValue;
            }
        }

        private void CloseMission(Mission mission, Character character)
        {
            this.GenerateNewMissions(character);
        }

        private object RewardCharacter(Character character, Mission mission, bool won)
        {
            Int64 expAmount = 50 * ((int)mission.Rarity + 1);
            Int64 goldAmount = new Random().Next(0, 10);
            this.characterHelper.AddExp(character, expAmount);
            this.characterHelper.AddGold(character, goldAmount);
            return new
            {
                expAmount,
                goldAmount
            };
        }

        private FightResoult Fight(Character character)
        {
            Monster monster = new Monster() { Health = 50, Damage = 5 };
            List<FightTurn> fightTurns = new List<FightTurn>();
            long playerHealth = character.Health, maxPlayerHealth = character.Health;
            long enemyHealth = monster.Health, maxEnemyHealth = monster.Health;
            while (playerHealth > 0 && enemyHealth > 0)
            {
                enemyHealth -= character.Damage;
                fightTurns.Add(new FightTurn() { DamageDealt = character.Damage, PlayerAttack = true });
                if (enemyHealth <= 0)
                    break;
                playerHealth -= monster.Damage;
                fightTurns.Add(new FightTurn() { DamageDealt = monster.Damage, PlayerAttack = false });
                if (playerHealth <= 0)
                    break;
            }
            return new FightResoult((character.Health > 0), fightTurns, maxPlayerHealth, maxEnemyHealth);
        }

        //Event occured, send it to client
        private object TransportEvent(Mission mission, MissionSkillReq missionSkillReq)
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
            character.Missions = this.dbContext.Missions.Where(m => m.CharacterId == character.CharacterId).ToList();
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
            byte[] events = RandomEvent();
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

        private byte[] RandomEvent()
        {
            byte[] events = null;
            Random random = new Random();
            if (random.Next(1, 101) > 60)
            {
                int amount = this.dbContext.Events.Count();
                int x = random.Next(0, amount);
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
            }
            return events;
        }
    }
}