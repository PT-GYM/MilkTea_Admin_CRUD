using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IHubService
    {
        Task NotiProCreated(Product product);
        Task NotiProUpdated(Product product);
        Task NotiProDeleted(int productId);
    }
}
