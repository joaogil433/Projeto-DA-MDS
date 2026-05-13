using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_DA_MDS
{
    public class IshoppingDbInitializer: DropCreateDatabaseIfModelChanges<IshoppingContext>
    {
        protected override void Seed(IshoppingContext context)
        {
            base.Seed(context);
        }
    }
}
