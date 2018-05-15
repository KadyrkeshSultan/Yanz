using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Yanz.Models.Quiz;

namespace Yanz.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Country { get; set; }

        public List<Folder> Folders { get; set; }
        public List<Session> Sessions { get; set; }
        public List<QuestionSet> QuestionSets { get; set; }
    }
}
