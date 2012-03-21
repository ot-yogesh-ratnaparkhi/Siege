using FluentNHibernate.Mapping;

namespace Siege.Security.SQL.Mappings
{
    public class AspNetMembershipMap : SubclassMap<AspNetUser>
    {
        public AspNetMembershipMap()
        {
            Table("aspnet_Membership");
            KeyColumn("UserId");
            Map(x => x.IsLockedOut);
            Map(x => x.ApplicationId);
            Map(x => x.IsActive, "IsApproved");
            Map(x => x.PasswordFormat).CustomType(typeof(int));
            Map(x => x.PasswordSalt);
            Map(x => x.CreateDate);
            Map(x => x.LastLoginDate);
            Map(x => x.LastLockoutDate);
            Map(x => x.LastPasswordChangedDate);
            Map(x => x.FailedPasswordAnswerAttemptCount);
            Map(x => x.FailedPasswordAnswerAttemptWindowStart);
            Map(x => x.FailedPasswordAttemptCount);
            Map(x => x.FailedPasswordAttemptWindowStart);
        }
    }
}