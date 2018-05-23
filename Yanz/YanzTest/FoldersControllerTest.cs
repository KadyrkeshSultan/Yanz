using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Yanz;
using Yanz.Controllers.API;
using Yanz.Data;
using Yanz.Models;

namespace YanzTest
{
    public class FoldersControllerTest
    {
        TestServer server;
        HttpClient client;

        public FoldersControllerTest()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            client = server.CreateClient();
        }
        [Fact]
        public void GetTestWithNull()
        {
            //userManager = new UserManager<ApplicationUser>();
            //FoldersController controller = new FoldersController();
        }
    }
}
