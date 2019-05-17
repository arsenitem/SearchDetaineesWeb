using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPrison.Models;

namespace WebPrison.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index() 
        {
            List<Detainees> detlist = new List<Models.Detainees>();
            if (Global.logged == true)
            {
                ViewBag.Logged = true;
                ViewBag.User = Global.current_user;     
            }
            using (JailDB db = new JailDB())
            {
                var res = db.Detainees.Where(x => x.IdDetainee==1);
                foreach (var r in res)
                {
                    detlist.Add(r);
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult FindGuest(string name, string l_name, string bday) //Поиск по 3 полям
        {
            ViewBag.SearchGuest = true;
            using (JailDB db = new JailDB())
            {
                var res = db.Detainees.Where(x => x.FirstName == name && x.SurName == l_name && x.BirthDay == bday).FirstOrDefault();
                return PartialView(res);
            }
          
        }
        [HttpPost]
        public ActionResult FindSchool(string school) //Поиск по уч заведению
        {
            ViewBag.SearchGuest = true;
            using (JailDB db = new JailDB())
            {
                var res = db.Detainees.Where(x => x.School==school).ToList();
                return PartialView(res);
            }
        }

        [HttpGet]
        public ActionResult Protocols() //Все протоколы
        {
            if (Global.logged == true)
            {
                ViewBag.Logged = true;
                ViewBag.User = Global.current_user;
                //Заполнение таблицы
                using (JailDB db = new JailDB())
                {
                    List<string[]> protolist = new List<string[]>();
                    var result = from protocols in db.Reports
                                 join officers in db.Officers on protocols.IdOfficer equals officers.IdOfficer
                                 join location in db.CompelLocations on protocols.IdLocation equals location.IdLocation
                                 select new
                                 {
                                     number = protocols.IdReport,
                                     part = protocols.Section,
                                     datetime = protocols.DateTime,
                                     officer_name = officers.FirstName,
                                     officer_sname = officers.SurName,
                                     uved = protocols.Notification,
                                     location = location.Title

                                 };

                    foreach (var report in result)
                    {                              
                        protolist.Add(new string[] { report.number.ToString(), report.part, report.datetime.ToString(), report.officer_name + " " + report.officer_sname, report.uved.ToString(), report.location });
                    }
                    return View(protolist);
                }    
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Login() //Окно логина
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string login,string pass) //Проверка 
        {
            if(login=="admin" && pass == "12345")
            {
                ViewBag.Logged = true;
                Global.logged = true;
                Global.current_user = login;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = true;
                ViewBag.Message = "Логин или пароль указан неверно";
                return View();
            }
        }
        public ActionResult LogOut() //Выход
        {
            Global.logged = false;
            Global.current_user = null;
            return RedirectToAction("Index");
        }
        public ActionResult Detainees() //Список задержанных
        {
            if (Global.logged == true)
            {
                ViewBag.Logged = true;
                ViewBag.User = Global.current_user;
                using(JailDB db=new JailDB())
                {
                    var res = db.Detainees.ToList();
                    return View(res);
                }
              
            }
                return RedirectToAction("Index"); 
        }
        public ActionResult Detentions() //Список задержаний //Дописать добалвение 
        {
            if (Global.logged == true)
            {
                ViewBag.Logged = true;
                ViewBag.User = Global.current_user;
                List<string[]> detentionslist = new List<string[]>();
                using (JailDB db = new JailDB())
                {

                    var result = db.Detentions.Join(db.Detainees,
                      p => p.IdDetainee,
                      c => c.IdDetainee,
                      (p, c) => new
                      {
                          name = c.FirstName,
                          s_name = c.SurName,
                          motion = p.Reason,
                          date_begin = p.Detaintime,
                          date_end = p.ReleaseTime

                      });
                    foreach (var res in result)
                    {

                        detentionslist.Add(new string[] { res.name + " " + res.s_name, res.date_begin.ToString(), res.motion, res.date_end.ToString() });
                    }
                }
                return View(detentionslist);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Days15() //Задержанные на 15 суток
        {
            List<string[]> detentionslist = new List<string[]>();
            using (JailDB db = new JailDB())
            {

                var result = db.Detentions.Join(db.Detainees,
                  p => p.IdDetainee,
                  c => c.IdDetainee,
                  (p, c) => new
                  {
                      name = c.FirstName,
                      s_name = c.SurName,
                      motion = p.Reason,
                      date_begin = p.Detaintime,
                      date_end = p.ReleaseTime

                  });
                foreach (var res in result)
                {
                    var totaldays = res.date_end.Value.Subtract(res.date_begin.Value).TotalDays;
                    if (totaldays == 15)
                    {
                        detentionslist.Add(new string[] { res.name + " " + res.s_name, res.date_begin.ToString(), res.motion, res.date_end.ToString() });
                    }
                       
                }
            }
            return PartialView(detentionslist);
        }
        [HttpPost]
        public ActionResult FindPeriod(string begin_date, string end_date) //Задержанный за определенный период
        {
            using(JailDB db=new JailDB())
            {
                List<string[]> protolist = new List<string[]>();
                var result = from protocols in db.Reports
                             join officers in db.Officers on protocols.IdOfficer equals officers.IdOfficer
                             join location in db.CompelLocations on protocols.IdLocation equals location.IdLocation
                             select new
                             {
                                 number = protocols.IdReport,
                                 part = protocols.Section,
                                 datetime = protocols.DateTime,
                                 officer_name = officers.FirstName,
                                 officer_sname = officers.SurName,
                                 uved = protocols.Notification,
                                 location = location.Title

                             };


                foreach (var report in result)
                {
                    if (report.datetime < Convert.ToDateTime(end_date) && report.datetime > Convert.ToDateTime(begin_date))
                    {
                        protolist.Add(new string[] { report.number.ToString(), report.part, report.datetime.ToString(), report.officer_name + " " + report.officer_sname, report.uved.ToString(), report.location });
                    }            
                }
                return PartialView(protolist);
            }
           
        }
        public ActionResult AdditionalInfo(string p_ser,string p_num)
        {
            
            using (JailDB db = new JailDB())
            {
                List<string[]> protolist = new List<string[]>();
                var ser = Convert.ToInt32(p_ser);
                var num = Convert.ToInt32(p_num);
                var result = from detainee in db.Detainees
                             join detentions in db.Detentions on detainee.IdDetainee equals detentions.IdDetainee
                             join protocols in db.Reports on detentions.IdReport equals protocols.IdReport
                             join officers in db.Officers on protocols.IdOfficer equals officers.IdOfficer
                             join location in db.CompelLocations on protocols.IdLocation equals location.IdLocation
                             where detainee.PassSeries == ser && detainee.PassNumber == num
                             select new
                             {
                                 num = protocols.IdReport,
                                 detaineName = detainee.FirstName,
                                 detaineSName = detainee.SurName,
                                 detaineBirthDay = detainee.BirthDay,
                                 //number = p.IdReport,
                                 part = protocols.Section,
                                 datetime = protocols.DateTime,
                                 officer_name = officers.FirstName,
                                 officer_sname = officers.SurName,
                                 uved = protocols.Notification,
                                 location = location.Title
                             };

                foreach (var r in result)
                {
                    protolist.Add(new string[] { r.num.ToString(), r.part, r.datetime.ToString(), r.officer_name + " " + r.officer_sname, r.uved.ToString(), r.location });
                }
                return PartialView(protolist);
            }
        
        }
        [HttpGet]
        public ActionResult AddNewDetention()
        {
            if (Global.logged == true)
            {
                ViewBag.Logged = true;
                ViewBag.User = Global.current_user;
                return View();
            }
            return RedirectToAction("Index");       
        }
        [HttpPost]
        public ActionResult AddNewDetention(NewDetension detension)
        {
            var det = detension;
            if (Global.logged == true)
            {
                ViewBag.Logged = true;
                ViewBag.User = Global.current_user;

                //старт
                using (JailDB db = new JailDB())
                {
                    var date = det.bday;
                    int passnum = int.Parse(det.pass_num);
                    int pass_series = int.Parse(det.pass_ser);
                    var id_detainee = 0;
                    Detainees detainee = new Detainees(); //get id
                    var res = db.Detainees.Where(x => x.PassNumber == passnum && x.PassSeries == pass_series).FirstOrDefault();
                    if (res == null)
                    {
                        detainee.FirstName = det.name;
                        detainee.SurName = det.lastname;
                        detainee.MiddleName = det.surname;
                        detainee.BirthDay = det.bday.ToString();
                        detainee.BirthLocation = det.b_place;
                        detainee.CurrLocation = det.live_place;
                        detainee.PassNumber = Convert.ToInt32(det.pass_num);
                        detainee.PassSeries = Convert.ToInt32(det.pass_ser);
                        detainee.PhoneNumber = det.phome;
                        detainee.School = det.school;
                        detainee.WorkPhone = det.workphone;
                        detainee.Work = det.workplace;
                        db.Detainees.Add(detainee);
                        id_detainee = db.Detainees.Where(x => x.PassNumber == passnum && x.PassSeries == pass_series).Select(x => x.IdDetainee).FirstOrDefault();
                        db.SaveChanges();
                    }
                    else
                    {
                        id_detainee = res.IdDetainee;
                    }

                    var id_location = 0;
                    var resLocation = db.CompelLocations.Where(x => x.Title == det.compel_place).FirstOrDefault();
                    if (resLocation == null)
                    {
                        CompelLocations location = new CompelLocations();
                        location.Title = det.compel_place;
                        location.Address = det.compel_adress;
                        db.CompelLocations.Add(location);
                        db.SaveChanges();
                        id_location = db.CompelLocations.Where(x => x.Title == det.compel_place).Select(x => x.IdLocation).FirstOrDefault();
                    }
                    else
                    {
                        id_location = resLocation.IdLocation;
                    }

                    var id_officer = 0;
                    var off = db.Officers.Where(x => x.FirstName == det.officer_name && x.SurName == det.officer_famil && x.Rank == det.officer_rank).FirstOrDefault();
                    if (off == null)
                    {
                        Officers officer = new Officers();
                        officer.FirstName = det.officer_name;
                        officer.SurName = det.officer_famil;
                        officer.Post = det.officer_post;
                        officer.Rank = det.officer_rank;
                        db.Officers.Add(officer);
                        db.SaveChanges();
                        id_officer = db.Officers.Where(x => x.FirstName == det.officer_name && x.SurName == det.officer_famil && x.Rank == det.officer_rank).Select(x => x.IdOfficer).FirstOrDefault();
                    }
                    else
                    {
                        id_officer = off.IdOfficer;
                    }

                    Reports report = new Reports();
                    report.Section = det.part;
                    report.DateTime = det.date_sost;
                    report.IdLocation = id_location;
                    report.IdOfficer = id_officer;
                    db.Reports.Add(report);
                    db.SaveChanges();




                    var res3 = db.Detainees.Where(x => x.PassNumber == passnum && x.PassSeries == pass_series).FirstOrDefault();
                    Detentions new_detension = new Detentions();
                    new_detension.Detaintime = det.det_date;
                    new_detension.IdDetainee = id_detainee;
                    new_detension.Reason = det.det_reason;
                    DateTime date1 = new DateTime();
                    date = det.date_sost;
                    new_detension.ReleaseTime = det.det_end;
                    new_detension.IdReport = db.Reports.Where(x => x.DateTime == date1).Select(x => x.IdReport).FirstOrDefault();
                    db.Detentions.Add(new_detension);
                    db.SaveChanges();            
                }
                ViewBag.Result = true;
                return View();
            }
            return RedirectToAction("Index");
        }
    }
    public static class Global
    {
        public static bool logged;
        public static string current_user;
    }
}