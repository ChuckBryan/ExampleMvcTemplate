namespace ExampleMvcTemplate.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Models;
    using Models.Home;

    public class EditorController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult GetDisposition(int id)
        {
            return PartialView( new DispositionModel{Id= id});
        }


        [ChildActionOnly]
        public ActionResult GetStateCheckList(int id)
        {

            IList<StateCheckListModel> stateCheckListModel = new List<StateCheckListModel>()
            {
                new StateCheckListModel {State = "VA", StateName = "Virginia"},
                new StateCheckListModel {State = "SC", StateName = "South Carolina"},
                new StateCheckListModel {State = "NC", StateName = "North Carolina"},
                new StateCheckListModel {State = "NY", StateName = "New York"},
                new StateCheckListModel {State = "CA", StateName = "California"},
            };

            return PartialView(stateCheckListModel);
        }

        [HttpPost]
        public ActionResult SaveDisposition(DispositionModel model)
        {
            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        public ActionResult SaveDisposition2(DispositionModel model)
        {
            return JsonSuccess(model);
        }

        [HttpPost]
        public ActionResult SaveStates(IList<StateCheckListModel> selectedStates )
        {
            return JsonSuccess(true);
        }
    }

    
}