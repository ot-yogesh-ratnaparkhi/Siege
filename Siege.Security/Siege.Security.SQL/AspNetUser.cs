using System;
using System.Web.Security;

namespace Siege.Security.SQL
{
    public class AspNetUser : User
    {
        public virtual DateTime FailedPasswordAttemptWindowStart { get; set; }
        public virtual int FailedPasswordAttemptCount { get; set; }
        public virtual DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }
        public virtual int FailedPasswordAnswerAttemptCount { get; set; }
        public virtual DateTime LastPasswordChangedDate { get; set; }
        public virtual DateTime LastLockoutDate { get; set; }
        public virtual DateTime LastLoginDate { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual string PasswordSalt { get; set; }
        public virtual MembershipPasswordFormat PasswordFormat { get; set; }
        public virtual Guid ApplicationId { get { return Application != null ? Application.ID ?? Guid.Empty : Guid.Empty; } set { } }
        public virtual DateTime LastActivityDate { get; set; }
        public virtual string LoweredUserName { get; set; }
    }
}