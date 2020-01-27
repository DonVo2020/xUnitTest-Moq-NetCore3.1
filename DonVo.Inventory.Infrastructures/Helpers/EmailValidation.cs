using System.Text.RegularExpressions;

namespace DonVo.Inventory.Infrastructures.Helpers
{
    public static class EmailValidation
    {
        /// &lt;summary>
        /// Regular expression, which is used to validate an E-Mail address.
        /// &lt;/summary>
        public const string MatchEmailPattern =
                    @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
			[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
            + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
			[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

        /// &lt;summary>
        /// Checks whether the given Email-Parameter is a valid E-Mail address.
        /// &lt;/summary>
        /// &lt;param name="email">Parameter-string that contains an E-Mail address.&lt;/param>
        /// &lt;returns>True, when Parameter-string is not null and 
        /// contains a valid E-Mail address;
        /// otherwise false.&lt;/returns>
        public static bool IsValidEmail(string email)
        {
            if (email != null)
                return Regex.IsMatch(email, MatchEmailPattern);
            else
                return false;
        }
    }
}
