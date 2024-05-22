using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcCoreElastiCacheAWS.Models;
using MvcCoreElastiCacheAWS.Repositories;
using MvcCoreElastiCacheAWS.Services;

namespace MvcCoreElastiCacheAWS.Controllers
{
    public class CochesController : Controller
    {
        private RepositoryCoches repositoryCoches;
        private ServiceAWSCache serviceAWSCache;

        public CochesController(RepositoryCoches repositoryCoches, ServiceAWSCache serviceAWSCache)
        {
            this.repositoryCoches = repositoryCoches;
            this.serviceAWSCache = serviceAWSCache;
        }

        // GET: CochesController
        public ActionResult Index()
        {
            return View(repositoryCoches.GetCoches());
        }

        // GET: CochesController/Details/5
        public ActionResult Details(int id)
        {
            return View(repositoryCoches.GetCoche(id));
        }

        public async Task<ActionResult> ListFav()
        {
            ViewData["MENSAJE"] = "Favoritos";
            return View("Index", await serviceAWSCache.GetCochesFavoritosAsync());
        }

        public async Task<ActionResult> AddFav(int id)
        {
            Coche coche = repositoryCoches.GetCoche(id)!;
            await serviceAWSCache.AddFav(coche);
            return RedirectToAction("ListFav");
        }

        public async Task<ActionResult> RemoveFav(int id)
        {            
            await serviceAWSCache.RemoveFav(id);
            return RedirectToAction("ListFav");
        }
    }
}
