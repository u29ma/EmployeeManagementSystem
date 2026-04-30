using EmployeeManagementSystem.Da;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace EmployeeManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountDa _da;

        public AccountController(AccountDa da)
        {
            _da = da;
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _da.ValidateUser(model.Email, model.Password);

            if (user != null)
            {
                // ✅ Session
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("UserName", user.Username);
                HttpContext.Session.SetString("EmployeeId", user.EmployeeId.ToString());

                // 🔹 Employee Mapping
                var employee = _da.GetEmployeeByUserId(user.UserId);

                if (employee != null)
                {
                    HttpContext.Session.SetString("EmployeeId", employee.EmployeeId.ToString());

                    // 🔥 Redirect if profile not complete
                    if (!employee.IsProfileComplete)
                    {
                        return RedirectToAction("CompleteProfile", "Employee");
                    }
                }
                else
                {
                    HttpContext.Session.SetString("EmployeeId", "0");
                }

                // ✅ Role-based redirect
                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "AdminDashboard");
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            ViewBag.Error = "Invalid Email or Password";
            return View(model);
        }

        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = _da.Register(model);

                if (!result)
                {
                    ModelState.AddModelError("", "Email already exists");
                    return View(model);
                }

                return RedirectToAction("Login");
            }

            return View(model);
        }

        // GET
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordModel model)
        {
            var user = _da.GetByEmail(model.Email);

            if (user != null)
            {
                ViewBag.Message = "Your password is: " + user.Password;
            }
            else
            {
                ViewBag.Message = "Email not found!";
            }

            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

//using EmployeeManagementSystem.Da;
//using EmployeeManagementSystem.Data;
//using EmployeeManagementSystem.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq;

//namespace EmployeeManagementSystem.Controllers
//{
//    public class AccountController : Controller
//    {

//        private readonly ApplicationDbContext _context;

//        public AccountController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: Login Page
//        public IActionResult Login()
//        {

//                return View();
//        }

//        // POST: Login Process      
//        [HttpPost]
//        public IActionResult Login(LoginModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }

//            if (ModelState.IsValid)
//            {
//                var user = _context.Users
//                    .FirstOrDefault(u => u.Email == model.Email
//                                      && u.Password == model.Password);

//                if (user != null)
//                {
//                    // Store session
//                    HttpContext.Session.SetString("UserEmail", user.Email);
//                    HttpContext.Session.SetString("Role", user.Role);

//                    HttpContext.Session.SetString("UserId", user.UserId.ToString());

//                    // 🔥 IMPORTANT PART - Emp Mapping
//                    var employee = _context.Employees
//                        .FirstOrDefault(e => e.UserId == user.UserId);

//                    if (employee != null)
//                    {
//                        HttpContext.Session.SetString("EmployeeId", employee.EmployeeId.ToString());
//                    }
//                    else
//                    {
//                        // Optional: handle if employee record missing
//                        HttpContext.Session.SetString("EmployeeId", "0");
//                    }


//                    // ✅ ROLE-BASED REDIRECTION
//                    if (user.Role == "Admin")
//                    {
//                        return RedirectToAction("Index", "AdminDashboard");
//                    }
//                    else if (user.Role == "Employee")
//                    {
//                        return RedirectToAction("Index", "Dashboard");
//                    }

//                }
//                else
//                {
//                    ViewBag.Error = "Invalid Email or Password";
//                }
//            }

//            return View(model);
//        }

//        [HttpPost]
//        public IActionResult Register(RegisterModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = new Users
//                {
//                    Name = model.Name,
//                    Email = model.Email,
//                    Password = model.Password,
//                    Role = "Employee"
//                };

//                _context.Users.Add(user);
//                _context.SaveChanges();

//                var employee = new Employees
//                {
//                    UserId = user.UserId,
//                    Email = user.Email,
//                    Status = "Active",
//                    IsProfileComplete = false
//                };

//                _context.Employees.Add(employee);
//                _context.SaveChanges();

//                return RedirectToAction("Login");
//            }

//            return View(model);
//        }


//        // Logout
//        public IActionResult Logout()
//        {
//            HttpContext.Session.Clear();
//            return RedirectToAction("Login", "Account");
//            //return RedirectToAction("Login");
//        }
//    }

//}

