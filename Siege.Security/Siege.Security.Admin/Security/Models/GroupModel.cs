﻿namespace Siege.Security.Admin.Security.Models
{
    public class GroupModel : SecurityModel
    {
        public int? GroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public bool IsNew
        {
            get { return GroupID == null; }
        }
    }
}