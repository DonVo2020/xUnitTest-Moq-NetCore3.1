namespace DonVo.Inventory.Domains
{
    public interface IIdentityInterface
    {
        string Username { get; set; }
        string Token { get; set; }
        int TimezoneOffset { get; set; }
    }
}
