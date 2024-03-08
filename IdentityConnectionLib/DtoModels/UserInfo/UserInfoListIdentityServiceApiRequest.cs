namespace IdentityConnectionLib.DtoModels.UserInfoLists
{
    public record UserInfoListIdentityServiceApiRequest : BaseRequest
    {
        /// <summary>
        /// Array of user's ids
        /// </summary>
        public required Guid[] UsersId { get; init; }
    }
}
