using Microsoft.AspNetCore.Mvc;
using SmartApp.Data;
using SmartApp.Models;

namespace SmartApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ClientController(ApplicationDbContext db)
        {
            _db = db;
        }


        //public IActionResult Index()
        //{
        //    IEnumerable<Client> objClientList= _db.Clients.ToList(); 
        //    return View(objClientList);
        //}

        //Get
        public IActionResult Create()
        {
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Client obj)
        {
            //if(obj.Nom==null)
            //ModelState.AddModelError("Nom", "Champ obligatoire");

            if (ModelState.IsValid)
            {
                _db.Clients.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Client added successfully";
                return RedirectToAction("Index");
            }
           
                return View(obj); 
                
        }

        //Get
        public IActionResult Edit(int? id)
        {
            if(id==null|| id == 0)
            {
                return NotFound();  
            }

            var clientFromDb = _db.Clients.Find(id);
            //var clientFromDbFirst=_db.Clients.FirstOrDefault(u=>u.Id==id);
            //var clientFromDbSingle=_db.Clients.SingleOrDefault(u=>u.Id==id);

            if(clientFromDb==null)
            {
                return NotFound();
            }
                return View(clientFromDb);

            
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Client obj)
        {
         
            if (ModelState.IsValid)
            {
                _db.Clients.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Client edited successfully";

                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
        }



        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var clientFromDb = _db.Clients.Find(id);
            //var clientFromDbFirst = _db.Clients.FirstOrDefault(u => u.Id == id);
            //var clientFromDbSingle = _db.Clients.SingleOrDefault(u => u.Id == id);

            if (clientFromDb == null)
            {
                return NotFound();
            }
            return View(clientFromDb);


        }

        //Post
        [HttpPost,ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var clientFromDb = _db.Clients.Find(id);

            if (clientFromDb == null)
            {
                return NotFound();
            }

       
            _db.Clients.Remove(clientFromDb);
            _db.SaveChanges();

            TempData["success"] = "Client deleted successfully";

            return RedirectToAction("Index");
            
        }


        // GET: Client/Index
        public IActionResult Index(string searchString)
        {
            // Query to retrieve clients
            var clients = from c in _db.Clients select c;

            // If search string is not null or empty, filter clients based on the search string
            if (!string.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(c => c.Name.Contains(searchString)
                                         || c.Address.Contains(searchString)
                                         || c.PhoneNumber.Contains(searchString)
                                         || c.Email.Contains(searchString)
                                         || c.Order.Contains(searchString));

            }

            // Pass the clients and the search string to the view
            ViewData["SearchString"] = searchString;
            return View(clients.ToList());
        }

    }
}
