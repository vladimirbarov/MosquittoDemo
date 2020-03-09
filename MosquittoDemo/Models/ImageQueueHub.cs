using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosquittoDemo.Models
{
    public class ImageQueueHub: Hub
    {
        public async Task Send(string path)
        {
            // Call the newimage method to update clients.
            await Clients.All.SendAsync("newimage", path);
        }
    }
}
