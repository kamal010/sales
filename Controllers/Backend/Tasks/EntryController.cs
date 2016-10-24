using System.Threading.Tasks;
using System.Web.Mvc;
using Frapid.ApplicationState.Cache;
using Frapid.Areas;
using Frapid.Dashboard;

namespace MixERP.Sales.Controllers.Backend.Tasks
{
    public class CustomerSearchController : FrapidController
    {
        [Route("dashboard/sales/setup/customers/search")]
        [MenuPolicy(OverridePath = "/dashboard/sales/tasks/entry")]
        public ActionResult Index()
        {
            return this.View("/Areas/MixERP.Sales/Views/Setup/CustomerSearch.cshtml");
        }
    }

    public class EntryController : SalesDashboardController
    {
        [Route("dashboard/sales/tasks/entry/checklist/{tranId}")]
        [MenuPolicy(OverridePath = "/dashboard/sales/tasks/entry")]
        public ActionResult CheckList(long tranId)
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Tasks/Entry/CheckList.cshtml", this.Tenant), tranId);
        }

        [Route("dashboard/sales/tasks/entry")]
        [MenuPolicy]
        public ActionResult Index()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Tasks/Entry/Index.cshtml", this.Tenant));
        }

        [Route("dashboard/sales/tasks/entry/new")]
        [MenuPolicy(OverridePath = "/dashboard/sales/tasks/entry")]
        public ActionResult New()
        {
            return this.FrapidView(this.GetRazorView<AreaRegistration>("Tasks/Entry/New.cshtml", this.Tenant));
        }


        [HttpPost]
        [Route("dashboard/sales/tasks/entry/new")]
        public async Task<ActionResult> PostAsync(ViewModels.Sales model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.InvalidModelState();
            }

            var meta = await AppUsers.GetCurrentAsync().ConfigureAwait(true);

            model.UserId = meta.UserId;
            model.OfficeId = meta.OfficeId;
            model.LoginId = meta.LoginId;

            long tranId = await DAL.Backend.Tasks.Sales.PostAsync(this.Tenant, model).ConfigureAwait(true);
            return this.Ok(tranId);
        }
    }
}