namespace Domain.Clients.PostUsersInfo
{
    public record PostUserInfo
    {
        /// <summary>
        /// Usser's first name
        /// </summary>
        public required string FirstName { get; init; }

        /// <summary>
        /// Usser's second name
        /// </summary>
        public required string SecondName { get; init; }

        /// <summary>
        /// User's status
        /// </summary>
        public required string Status { get; init; }

        /// <summary>
        /// User's avatar
        /// </summary>
        public required string Avatar { get; init; }
    }
}
