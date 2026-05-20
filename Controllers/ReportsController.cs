using EmployeeManagementSystem.Da;
using iText.Kernel.Events;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using iText.Kernel.Geom;

namespace EmployeeManagementSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ReportsDa _reportsDa;

        public ReportsController(ReportsDa da)
        {
            _reportsDa = da;
        }

        public IActionResult EmployeeReport(string search)
        {
            var data = _reportsDa.GetEmployeeReport(search);
            return View(data);
        }

        public IActionResult EmployeeReportPdf(string search)
        {
            var data = _reportsDa.GetEmployeeReport(search);

            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Margins
                document.SetMargins(60, 20, 50, 20);

                // ================= HEADER =================
                document.Add(new Paragraph("ABC Company Pvt Ltd")
                    .SetBold()
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.CENTER));

                document.Add(new Paragraph("Employee Report")
                    .SetFontSize(13)
                    .SetTextAlignment(TextAlignment.CENTER));

                document.Add(new Paragraph("Generated On: " +
                    DateTime.Now.ToString("dd-MM-yyyy"))
                    .SetTextAlignment(TextAlignment.RIGHT));

                document.Add(new Paragraph("\n"));

                // ================= TABLE =================
                float[] columnWidths = { 50, 150, 120, 120, 100 };

                Table table = new Table(columnWidths);

                // Table Headers
                table.AddHeaderCell("ID");
                table.AddHeaderCell("Name");
                table.AddHeaderCell("Department");
                table.AddHeaderCell("Designation");
                table.AddHeaderCell("Join Date");

                // Table Data
                foreach (var item in data)
                {
                    table.AddCell(item.EmployeeId.ToString());
                    table.AddCell(item.FullName);
                    table.AddCell(item.DepartmentName);
                    table.AddCell(item.Designation);
                    table.AddCell(item.JoinDate.ToShortDateString());
                }

                document.Add(table);

                // ================= FOOTER =================
                int totalPages = pdf.GetNumberOfPages();

                for (int i = 1; i <= totalPages; i++)
                {
                    PdfPage page = pdf.GetPage(i);

                    Rectangle pageSize = page.GetPageSize();

                    PdfCanvas pdfCanvas =
                        new PdfCanvas(page.NewContentStreamAfter(),
                        page.GetResources(),
                        pdf);

                    Canvas canvas = new Canvas(pdfCanvas, pageSize);

                    canvas.ShowTextAligned(
                        new Paragraph("Page " + i),
                        pageSize.GetWidth() / 2,
                        20,
                        TextAlignment.CENTER);

                    canvas.Close();
                }

                document.Close();

                return File(ms.ToArray(),
                    "application/pdf",
                    "EmployeeReport.pdf");
            }

        }
        //---------------------------------------------------------------------------------
        public IActionResult PayrollReport(string search)
        {
            var data = _reportsDa.GetPayrollReport(search);

            return View(data);
        }
        public IActionResult Payslip(int payrollId)
        {
            var data = _reportsDa.GetPayslip(payrollId);

            return View(data);
        }
    }
}