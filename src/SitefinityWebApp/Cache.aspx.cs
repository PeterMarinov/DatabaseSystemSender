using DatabaseSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SitefinityWebApp
{
    public partial class Cache : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                literalMachineKey.Text = Environment.MachineName;
            }
        }

        protected void buttonMain_Click(object sender, EventArgs e)
        {
            buttonMain.Enabled = false;
            divCallout.Visible = true;

            SystemMessagesProcessor processor = new SystemMessagesProcessor();
            processor.HandleSystemMessages();

            repeaterItems.DataSource = processor.Messages;
            repeaterItems.DataBind();

            literalHeading.Text = (processor.Messages.Count > 0) ? string.Format("Below are the {0} processed system messages", processor.Messages.Count) : "There are no cache messages";

            buttonMain.Enabled = true;
        }
    }
}