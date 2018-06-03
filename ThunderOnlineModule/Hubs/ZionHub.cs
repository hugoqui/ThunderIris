using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using ThunderOnlineModule.Models;

namespace ThunderOnlineModule.Hubs
{
    public class ZionHub: Hub
    {
        public void SendPrintBill(int billId)
        {
            Clients.All.printBill(billId);
        }
    }
}

