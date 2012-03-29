﻿namespace Siege.Security.Admin.Security.Models
{
    public class ConsumerModel : SecurityModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public bool IsNew
        {
            get { return ConsumerID == null; }
        }
    }
}