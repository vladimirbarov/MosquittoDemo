using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BackService.Hubs
{
    public class ImagesQueueHub: Hub
    {
        public async Task Send(string path)
        {
            await Clients.All.SendAsync("newimage", path);
        }
    }
}
