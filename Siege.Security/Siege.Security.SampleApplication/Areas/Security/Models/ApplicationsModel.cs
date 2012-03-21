using System.Collections.Generic;

namespace Siege.Security.SampleApplication.Areas.Security.Models
{
    public class ApplicationsModel : SecurityModel
    {
        public IList<Application> Applications { get; set; }
    }
}