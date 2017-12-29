using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICDB.Entities;
using System.IO;
using System.Windows.Media.Imaging;
using ICDB.Models;
namespace ICDB.Controllers
{
    public class iMDbController : Controller
    {
        // GET: ICDB
        private ICDBCloudEntities dbContext = null;
        //Actions
        #region         
        public ActionResult Index()
        {
            int pageCount = 0;
            double recordCount = 0.0;
            List<GetMovieInfo_vw> _listMovies = null;
            try
            {
                using (dbContext = new ICDBCloudEntities())
                {
                    if (dbContext != null)
                    {
                        _listMovies = dbContext.GetMovieInfo_vw.OrderByDescending(x => x.Release_Date).ToList();
                        recordCount = _listMovies.Count() < 4 ? _listMovies.Count() / 4.0 : _listMovies.Count() / 4;
                        var result = Math.Round(recordCount, 0, MidpointRounding.AwayFromZero);
                        int.TryParse(result.ToString(), out pageCount);
                        if (pageCount == 0)
                            pageCount = 1;
                        ViewBag.PageCount = pageCount + 1;
                    }
                }
            }
            catch
            {
                throw new Exception("Site is under maintanance. It is up soon.");
            }
            return View();
        }
        public ActionResult BannerControl(int pageNum = 1)
        {
            int num = 0, loopCount = 1;
            List<GetMovieInfo_vw> listMovies = null;
            BannerControl bannerModel = new Models.BannerControl();
            using (dbContext = new ICDBCloudEntities())
            {
                if (dbContext != null)
                {
                    listMovies = dbContext.GetMovieInfo_vw.OrderByDescending(x => x.Release_Date).ToList();
                    if (pageNum == 1)
                    {
                        listMovies = listMovies.Take(4).ToList<GetMovieInfo_vw>();
                    }
                    else
                    {
                        listMovies = ListMoviesByPage(pageNum * 4, listMovies);
                    }
                }
            }
            if (listMovies != null && listMovies.Count > 0)
            {
                foreach (GetMovieInfo_vw model in listMovies)
                {
                    if (model.Poster != null)
                    {
                        ViewData["Image" + (++num)] = "data:image/png;base64," + Convert.ToBase64String(model.Poster, 0, model.Poster.Length);
                    }
                    switch (loopCount)
                    {
                        case 1:
                            bannerModel.MovieID_1 = model.MovieID;
                            bannerModel.Name_1 = model.MovieName;
                            bannerModel.Plot_1 = model.Plot;
                            break;
                        case 2:
                            bannerModel.MovieID_2 = model.MovieID;
                            bannerModel.Name_2 = model.MovieName;
                            bannerModel.Plot_2 = model.Plot;
                            break;
                        case 3:
                            bannerModel.MovieID_3 = model.MovieID;
                            bannerModel.Name_3 = model.MovieName;
                            bannerModel.Plot_3 = model.Plot;
                            break;
                        case 4:
                            bannerModel.MovieID_4 = model.MovieID;
                            bannerModel.Name_4 = model.MovieName;
                            bannerModel.Plot_4 = model.Plot;
                            break;
                        default:
                            break;
                    }
                    loopCount++;
                }
            }
            return View(bannerModel);
        }
        public ActionResult ListingMoviesTemplate(int pageNum = 1)
        {
            bool flag = false;
            List<GetMovieInfo_vw> listMovies = null;
            try
            {
                using (dbContext = new ICDBCloudEntities())
                {
                    if (dbContext != null)
                    {
                        listMovies = dbContext.GetMovieInfo_vw.OrderByDescending(x => x.Release_Date).ToList();
                        if (pageNum == 1)
                        {
                            listMovies = listMovies.Take(4).ToList<GetMovieInfo_vw>();
                        }
                        else
                        {
                            listMovies = ListMoviesByPage(pageNum * 4, listMovies);
                        }
                    }
                }
                if (listMovies != null && listMovies.Count > 0)
                {
                    foreach (GetMovieInfo_vw model in listMovies)
                    {
                        if (model.Poster != null)
                        {
                            ViewData[model.MovieID.ToString()] = "data:image/png;base64," + Convert.ToBase64String(model.Poster, 0, model.Poster.Length);
                            if (flag == false)
                            {
                                ViewData["DefaultImage"] = "data:image/png;base64," + Convert.ToBase64String(model.Poster, 0, model.Poster.Length);
                                flag = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong..." + ex.Message);
            }
            return View(listMovies);
        }
        public ActionResult AddMovies(AddMovie model = null)
        {            
            ICDB.Models.Producer producer = null;
            List<Models.Producer> _listProducer = new List<Models.Producer>();
            if (model != null)
            {
                using (dbContext = new ICDBCloudEntities())
                {                    
                    var listProducers = dbContext.Producers.ToList();                                                            
                    foreach (ICDB.Entities.Producer listItem in listProducers)
                    {
                        producer = new Models.Producer()
                        {
                            ProducerID = listItem.ID,
                            ProducerName = listItem.Name
                        };
                        _listProducer.Add(producer);
                    }
                    model.iProducers = _listProducer.AsEnumerable();
                }
            }
            return View(model);
        }
        #endregion
        //Ajax Methods
        #region     
        [HttpPost]    
        public ActionResult GetMovieInfoByID(int id)
        {
            //UpdateMovies(movieId);
            Movies _model = null;            
            using (dbContext = new ICDBCloudEntities())
            {
                if (dbContext != null)
                    _model = dbContext.Movies1.Where(x => x.ID == id).FirstOrDefault();
            }
            return Json(_model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public virtual ActionResult UploadFile(HttpPostedFileBase file, int movieID, string movieName, int releaseDate, string plot, string posterPath)
        {
            bool isUploaded = false;
            Movies model = null;
            string message = "Update failed";            
            var content = new byte[file.ContentLength];
            file.InputStream.Read(content, 0, file.ContentLength);            
            try
            {
                using (dbContext = new ICDBCloudEntities())
                {
                    if (dbContext != null)
                        model = dbContext.Movies1.Where(x => x.ID == movieID).FirstOrDefault();
                    if (model != null)
                    {
                        model.Name = movieName;
                        model.Release_Date = releaseDate;
                        if (file != null)
                        {
                            model.Poster = content;
                            model.PosterPath = file.FileName;
                        }                            
                        model.Plot = plot;
                        model.DateModified = DateTime.Now;
                        dbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                }
                isUploaded = true;
                message = "Updated Successfully";
            }
            catch(Exception ex)
            {
                message = "Update failed. " + ex.Message; 
            }
            return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        }
        public ActionResult SaveMovieInfo(HttpPostedFileBase file, string movieName, string plot, int releaseDate, string posterPath, string actorID, string directorID, string musicDirectorID, int producerID)
        {
            string filePath = string.Empty, message = string.Empty;
            var content = new byte[file.ContentLength];
            file.InputStream.Read(content, 0, file.ContentLength);
            try
            {
                ICDB.Entities.Movies objMovie = new ICDB.Entities.Movies();
                objMovie.Name = movieName;
                objMovie.Plot = plot;
                objMovie.Release_Date = releaseDate;
                if(file != null)
                {
                    objMovie.Poster = content;
                    objMovie.PosterPath = file.FileName;
                }
                objMovie.DateCreated = DateTime.Now;              
                using (dbContext = new ICDBCloudEntities())
                {
                    dbContext.Entry(objMovie).State = System.Data.Entity.EntityState.Added;
                    dbContext.SaveChanges();
                    dbContext.SP_iMDBAddMovie(objMovie.ID, actorID.Remove(actorID.Length-1), directorID.Remove(directorID.Length-1), musicDirectorID.Remove(musicDirectorID.Length-1), producerID, DateTime.Now, null);                 
                    message = "Saved Successfully";
                }
            }
            catch (Exception ex)
            {
                message = "Sorry something went wrong. " + ex.Message;
            }
            return Json(message,JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult AddCastAndCrew(string state, string actorName, string actorGender, string actorDOB,string actorBio,string pName,string pGender, string pDOB, string pBio,string mcName,string mcGender, string mcDOB,string mcBio,string dName,string dGender, string dDOB,string dBio)
        {            
            string message = "Save failed";
            try
            {
                switch (state)
                {
                    case "1":
                        {
                            if(!(string.IsNullOrEmpty(actorName)) && !(string.IsNullOrEmpty(actorGender)) && !(string.IsNullOrEmpty(actorDOB)))
                            {
                                using (dbContext = new ICDBCloudEntities())
                                {
                                    ICDB.Entities.Actor objActor = new Entities.Actor()
                                    {
                                        Name = actorName,
                                        Sex = (actorGender == "1" ? "Male" : "Female"),
                                        DOB = Convert.ToDateTime(actorDOB),
                                        Bio = actorBio,
                                        DateCreated = DateTime.Now
                                    };
                                    dbContext.Entry(objActor).State = System.Data.Entity.EntityState.Added;
                                    dbContext.SaveChanges();
                                    message = "Added successfully";
                                }
                            }
                            else
                                message = "Please fill out all this fields";                           
                        }  
                        break;
                    case "2":
                        {
                            if (!(string.IsNullOrEmpty(pName)) && !(string.IsNullOrEmpty(pGender)) && !(string.IsNullOrEmpty(pDOB)))
                            {
                                using (dbContext = new ICDBCloudEntities())
                                {
                                    ICDB.Entities.Producer objProducer = new Entities.Producer()
                                    {
                                        Name = pName,
                                        Sex = (pGender == "1" ? "Male" : "Female"),
                                        DOB = Convert.ToDateTime(pDOB),
                                        Bio = pBio,
                                        DateCreated = DateTime.Now
                                    };
                                    dbContext.Entry(objProducer).State = System.Data.Entity.EntityState.Added;
                                    dbContext.SaveChanges();
                                    message = "Added successfully";
                                }
                            }
                            else
                                message = "Please fill out all this fields";                            
                        }
                        break;
                    case "3":
                        {
                            if (!(string.IsNullOrEmpty(mcName)) && !(string.IsNullOrEmpty(mcGender)) && !(string.IsNullOrEmpty(mcDOB)))
                            {
                                using (dbContext = new ICDBCloudEntities())
                                {
                                    ICDB.Entities.MusicDirector objMusicDirector = new Entities.MusicDirector()
                                    {
                                        Name = mcName,
                                        Sex = (mcGender == "1" ? "Male" : "Female"),
                                        DOB = Convert.ToDateTime(mcDOB),
                                        Bio = mcBio,
                                        DateCreated = DateTime.Now
                                    };
                                    dbContext.Entry(objMusicDirector).State = System.Data.Entity.EntityState.Added;
                                    dbContext.SaveChanges();
                                    message = "Added successfully";
                                }
                            }
                            else
                                message = "Please fill out all this fields";                            
                        }
                        break;
                    case "4":
                        {
                            if (!(string.IsNullOrEmpty(dName)) && !(string.IsNullOrEmpty(dGender)) && !(string.IsNullOrEmpty(dDOB)))
                            {
                                using (dbContext = new ICDBCloudEntities())
                                {
                                    ICDB.Entities.Director objDirector = new Entities.Director()
                                    {
                                        Name = dName,
                                        Sex = (dGender == "1" ? "Male" : "Female"),
                                        DOB = Convert.ToDateTime(dDOB),
                                        Bio = dBio,
                                        DateCreated = DateTime.Now
                                    };
                                    dbContext.Entry(objDirector).State = System.Data.Entity.EntityState.Added;
                                    dbContext.SaveChanges();
                                    message = "Added successfully";
                                }
                            }
                            else
                                message = "Please fill out all this fields";                            
                        }
                        break;
                }                
            }
            catch(Exception ex)
            {
                message = "Save failed" + ex.Message;
            }            
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCaseAndCrew(int type)
        {
            string message = string.Empty;
            List<Entities.Actor> listActors = null;
            List<Entities.Director> listDirectors = null;
            List<Entities.MusicDirector> listMCs = null;
            try
            {
                using (dbContext = new ICDBCloudEntities())
                {
                    listActors = dbContext.Actors.ToList();
                    listDirectors = dbContext.Directors.ToList();
                    listMCs = dbContext.MusicDirectors.ToList();
                }
                switch (type)
                {
                    case 1:
                        {
                            return new JsonResult { Data = listActors, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }
                    case 2:
                        {
                            return new JsonResult { Data = listDirectors, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }
                    case 3:
                        {
                            return new JsonResult { Data = listMCs, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }            
                }                
            }
            catch(Exception ex)
            {
                message = "Retrieve failed " + ex.Message;
            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        
       
        #endregion

        //Methods
        #region
        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }
        private List<GetMovieInfo_vw> ListMoviesByPage(int pageNum, List<GetMovieInfo_vw> sourceList)
        {
            List<GetMovieInfo_vw> listMovies = new List<GetMovieInfo_vw>();
            int count = sourceList.Count();
            int previousPageNum = ((pageNum / 4) - 1) * 4;
            for (int i = previousPageNum; i < pageNum; i++)
            {
                if (i < count)
                {
                    listMovies.Add(sourceList.ElementAt(i));
                }
            }

            return listMovies;
        }
        public string GenerateImageFile(HttpPostedFileBase file)
        {
            string filePath = string.Empty;
            if (file != null && file.ContentLength != 0)
            {
                string pathForSaving = Server.MapPath("~/Uploads");
                if (this.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        file.SaveAs(Path.Combine(pathForSaving, file.FileName));
                        filePath = pathForSaving + "\\" + file.FileName;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return filePath;
        }
        #endregion


    }
    
}