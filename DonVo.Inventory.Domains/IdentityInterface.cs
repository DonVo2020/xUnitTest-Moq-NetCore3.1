namespace DonVo.Inventory.Domains
{
    public class IdentityInterface : IIdentityInterface
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public int TimezoneOffset { get; set; }
    }
}
