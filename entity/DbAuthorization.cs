using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sap_hana_user_export.entity
{
    internal class DbAuthorization
    {
        public string objectName { get; set; }
        public string schemaName { get; set; }
        public string privilege { get; set; }
        public bool isGrantable { get; set; }

        public DbAuthorization(string objectName, string schemaName, string privilege, bool isGrantable)
        {
            this.objectName = objectName;
            this.schemaName = schemaName;
            this.privilege = privilege;
            this.isGrantable = isGrantable;
        }
    }
}
