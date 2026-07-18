using Clinical.Web.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinical.Web.Controllers;

public class DashboardController : BaseController
{
    private readonly IPatientService _patients;
    private readonly IDoctorService _doctors;
    private readonly IAppointmentService _appointments;
    private readonly IMedicineService _medicines;

    public DashboardController(IPatientService patients, IDoctorService doctors,
        IAppointmentService appointments, IMedicineService medicines)
    {
        _patients = patients;
        _doctors = doctors;
        _appointments = appointments;
        _medicines = medicines;
    }

    public async Task<IActionResult> Index()
    {
        var (patients, doctors, appointments, lowStock) = await (
            _patients.GetAllAsync(),
            _doctors.GetAllAsync(),
            _appointments.GetAllAsync(),
            _medicines.GetLowStockAsync()
        ).WhenAll();

        ViewBag.TotalPatients = patients.Count();
        ViewBag.TotalDoctors = doctors.Count();
        ViewBag.TotalAppointments = appointments.Count();
        ViewBag.LowStockCount = lowStock.Count();
        ViewBag.RecentAppointments = appointments.OrderByDescending(a => a.AppointmentDate).Take(5);
        ViewBag.LowStockMedicines = lowStock.Take(5);

        return View();
    }
}

file static class TaskExtensions
{
    public static async Task<(T1, T2, T3, T4)> WhenAll<T1, T2, T3, T4>(
        this (Task<T1> t1, Task<T2> t2, Task<T3> t3, Task<T4> t4) tasks)
    {
        await Task.WhenAll(tasks.t1, tasks.t2, tasks.t3, tasks.t4);
        return (tasks.t1.Result, tasks.t2.Result, tasks.t3.Result, tasks.t4.Result);
    }
}
