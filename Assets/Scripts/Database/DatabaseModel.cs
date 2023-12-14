namespace Snakefun.Data
{
    public struct DatabaseModel
    {
        public int ID { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public int AvatarID { get; set; }
        public int HighScore { get; set; }
        public int HighTime { get; set; }
        public int TotalScore { get; set; }
    }
}