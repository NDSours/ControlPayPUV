using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALControlPayPUV.Entities;

namespace DALControlPayPUV.Repository
{
    public class RepositoryDocumentPay
    {
        private MyDBContext context;
        public RepositoryDocumentPay()
        {
            context = new MyDBContext();
        }

        public void Delete(long id)
        {
            var findobj = context.DocumentsPay.Find(id);
            if(findobj != null)
            {
                context.DocumentsPay.Remove(findobj);
                context.SaveChanges();
            }
        }
    }
}
