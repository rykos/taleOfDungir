namespace taleOfDungir.Models
{
    public struct Entity
    {
        public long Health { get; set; }
        public long MaxHealth { get; set; }
        public long AvatarID { get; set; }

        public Entity(long health, long maxHealth, long avatarID)
        {
            Health = health;
            MaxHealth = maxHealth;
            AvatarID = avatarID;
        }

        public Entity(Character character){
            Health = character.Health;
            MaxHealth = character.MaxHealth;
            AvatarID = character.CharacterAvatarId;
        }
    }
}