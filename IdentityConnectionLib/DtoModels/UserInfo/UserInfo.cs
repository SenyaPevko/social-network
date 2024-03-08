namespace IdentityConnectionLib.DtoModels.UserInfoLists
{
    public record UserInfo
    {
        /// <summary>
        /// Usser's first name
        /// </summary>
        public required string FirstName { get; init; }

        /// <summary>
        /// Usser's second name
        /// </summary>
        public required string SecondName { get; init; }
    }
}
