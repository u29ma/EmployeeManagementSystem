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

            // ❌ Invalid User
            if (user == null)
            {
                ViewBag.Error = "Invalid Email or Password";
                return View(model);
            }

            // 🔹 Get Employee
            var employee = _da.GetEmployeeByEmployeeId(user.EmployeeId);

            // ❌ Employee not found
            if (employee == null)
            {
                ViewBag.Error = "Employee record not found";
                return View(model);
            }

            // ❌ Inactive Employee
            if (employee.Status == false)
            {
                ViewBag.Error = "Account is inactive";
                return View(model);
            }
            // 3️⃣ Get RoleId safely (IMPORTANT FIX)
            int? roleId = employee.RoleId;
            if (roleId == 0)
            {
                ViewBag.Error = "Role not assigned to employee";
                return View(model);
            }
            // 4️⃣ Get Role Name
            string roleName = _da.GetRoleName(roleId);

            if (string.IsNullOrEmpty(roleName))
            {
                roleName = "Employee";
            }

            if (user != null)
            {
                // ✅ Session
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("Role", roleName);
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("UserName", user.Username);
                //HttpContext.Session.SetString("EmployeeId", user.EmployeeId.ToString());

                if (employee != null)
                {
                    HttpContext.Session.SetString("EmployeeId", employee.EmployeeId.ToString());
                }
                else
                {
                    HttpContext.Session.SetString("EmployeeId", "0");
                }

                // ✅ Role-based redirect
                if (roleName == "Admin")
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

        // GET
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _da.GetEmail(model.Email);

            if (user != null)
            {
                ViewBag.Message = "Password reset link sent to your email";
            }
            else
            {
                ViewBag.Message = "Email not found!";
            }

            return View();
        }


        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            string userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login");
            }

            // Confirm password check
            if (model.NewPassword != model.ConfirmPassword)
            {
                ViewBag.Error = "Passwords do not match";

                return View();
            }

            int id = Convert.ToInt32(userId);

            bool result = _da.ChangePassword(
                id,
                model.CurrentPassword,
                model.NewPassword);

            if (!result)
            {
                ViewBag.Error =
                    "Current password is incorrect";

                return View();
            }

            ViewBag.Success = "Password changed successfully";

            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            var model = new ResetPasswordModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ResetPassword( ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool result = _da.ResetPassword(model.Email, model.NewPassword);

            if (!result)
            {
                ViewBag.Message = "Unable to reset password";

                return View(model);
            }

            TempData["Success"] = "Password reset successful";

            return RedirectToAction("Login");
        }
    }
}

