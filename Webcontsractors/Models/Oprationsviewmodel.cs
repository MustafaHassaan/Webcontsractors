using Microsoft.Reporting.Map.WebForms.BingMaps;
using Repo.Unitofwork;
using System;
using Webcontsractors.Domain.Models;

namespace Webcontsractors.Models
{
    public class Oprationsviewmodel
    {
        IUnitofwork _IUW;
        public Oprationsviewmodel(IUnitofwork IUW)
        {
            _IUW = IUW;
        }
        public void AddProjects(int usrid)
        {
            var dataid = _IUW.TblTransactions.GetAll().Select(x => x.Id).Last();
            Opration Opr = new Opration();
            Opr.Oprationname = "inserted at";
            Opr.Detailes = "تم اضافة المشروع في";
            Opr.Tblname = "Project";
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var time = DateTime.Now.ToString("hhL:mm:ss");
            Opr.Date = DateOnly.Parse(date);
            Opr.Time = time;
            Opr.Tblid = dataid;
            Opr.Usrid = usrid;
            _IUW.Oprations.Insert(Opr);
            _IUW.Complete();
        }
        public void AddTransactions(int usrid)
        {
            var dataid = _IUW.TblTransactions.GetAll().Select(x => x.Id).Last();
            Opration Opr = new Opration();
            Opr.Oprationname = "inserted at";
            Opr.Detailes = "تم اضافة التعامل في";
            Opr.Tblname = "Tbl_Transaction";
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var time = DateTime.Now.ToString("hhL:mm:ss");
            Opr.Date = DateOnly.Parse(date);
            Opr.Time = time;
            Opr.Tblid = dataid;
            Opr.Usrid = usrid;
            _IUW.Oprations.Insert(Opr);
            _IUW.Complete();
        }
        public void Edit(int tblid, string type, int usrid)
        {
            if (type == "Project")
            {
                var data = _IUW.Oprations.GetAll();
                var ddd = data.Where(x => x.Tblname == type && x.Tblid == tblid).LastOrDefault();
                if (ddd != null)
                {
                    Opration Opr = new Opration();
                    Opr.Oprationname = "Modefaied at";
                    Opr.Detailes = "تم تعديل المشروع في";
                    Opr.Tblname = "Project";
                    var date = DateTime.Now.ToString("yyyy-MM-dd");
                    var time = DateTime.Now.ToString("hhL:mm:ss");
                    Opr.Date = DateOnly.Parse(date);
                    Opr.Time = time;
                    Opr.Tblid = ddd.Tblid;
                    Opr.Usrid = usrid;
                    _IUW.Oprations.Update(Opr);
                    _IUW.Complete();
                }
            }
            else
            {
                var data = _IUW.Oprations.GetAll();
                var ddd = data.Where(x => x.Tblname == type && x.Tblid == tblid).LastOrDefault();
                if (ddd != null)
                {
                    Opration Opr = new Opration();
                    Opr.Oprationname = "Modefaied at";
                    Opr.Detailes = "تم تعديل التعامل في";
                    Opr.Tblname = "Tbl_Transaction";
                    var date = DateTime.Now.ToString("yyyy-MM-dd");
                    var time = DateTime.Now.ToString("hhL:mm:ss");
                    Opr.Date = DateOnly.Parse(date);
                    Opr.Time = time;
                    Opr.Tblid = ddd.Tblid;
                    Opr.Usrid = usrid;
                    _IUW.Oprations.Update(Opr);
                    _IUW.Complete();
                }
            }
        }
        public void Delete(int tblid, string type)
        {
            if (type == "Project")
            {
                var data = _IUW.Oprations.GetAll();
                var ddd = data.Where(x => x.Tblname == type && x.Tblid == tblid).LastOrDefault();
                if (ddd != null)
                {
                    _IUW.Oprations.Delbyid(ddd.Id);
                    _IUW.Complete();
                }
            }
            else
            {
                var data = _IUW.Oprations.GetAll();
                var ddd = data.Where(x => x.Tblname == type && x.Tblid == tblid).LastOrDefault();
                if (ddd != null)
                {
                    _IUW.Oprations.Delbyid(ddd.Id);
                    _IUW.Complete();
                }
            }
        }
    }
}
