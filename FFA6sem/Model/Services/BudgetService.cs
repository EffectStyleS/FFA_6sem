using DAL.Interfaces;
using FFA6sem.Model.Interfaces;
using FFA6sem.Model.Models;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Table = iText.Layout.Element.Table;

namespace FFA6sem.Model.Services
{
    public class BudgetService : IBudgetService
    {
        IDbRepos _db;

        public BudgetService(IDbRepos db)
        {
            _db = db;
        }

        public async Task CreateProperties(BudgetModel budget)
        {
            var help = await _db.TimePeriod.GetItem(budget.TimePeriodId);
            var timePeriod = help.Name;

            var startDate = budget.StartDate.ToShortDateString();

            budget.Properties = $"{timePeriod} с {startDate}";
        }

        public DateTime GetEndDate(BudgetModel budget)
        {
            DateTime endDate = new DateTime();
            switch (budget.TimePeriodId)
            {
                case 1:
                    endDate = budget.StartDate.AddMonths(1);
                    break;
                case 2:
                    endDate = budget.StartDate.AddMonths(3);
                    break;
                case 3:
                    endDate = budget.StartDate.AddYears(1);
                    break;
            }

            return endDate;
        }
        public async Task SavePDF(BudgetModel budget, string filePath)
        {
            var helpPlannedExpenses = await _db.PlannedExpenses.GetAll();
            var plannedExpenses = helpPlannedExpenses
                .Where(e => e.BudgetId == budget.Id)
                .Select(e => new PlannedExpensesModel(e))
                .ToList();

            var helpPlannedIncomes = await _db.PlannedIncomes
                .GetAll();
            var plannedIncomes = helpPlannedIncomes
                .Where(e => e.BudgetId == budget.Id)
                .Select(e => new PlannedIncomesModel(e))
                .ToList();

            var helpExpenseTypes = await _db.ExpenseType.GetAll();
            List<ExpenseTypeModel> expenseTypes = helpExpenseTypes.Select(e => new ExpenseTypeModel(e)).ToList();

            var helpIncomeTypes = await _db.IncomeType.GetAll();
            List<IncomeTypeModel> incomeTypes = helpIncomeTypes.Select(i => new IncomeTypeModel(i)).ToList();

            await CreateProperties(budget);

            FontProgram fontProgram = FontProgramFactory.CreateFont(@"C:\Windows\Fonts\arial.ttf");
            PdfFont arial = PdfFontFactory.CreateFont(fontProgram, "Cp1251");

            PdfWriter writer = new PdfWriter(filePath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);
            Paragraph header = new Paragraph($"Бюджет на {budget.Properties}")
                .SetFont(arial)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(18);

            document.Add(header);

            LineSeparator ls = new LineSeparator(new SolidLine());
            document.Add(ls);

            #region PlannedExpensesTable
            Table plannedExpensesTable = new Table(2, false)
                .SetMarginTop(10)
                .SetHorizontalBorderSpacing(5);

            Cell cellPlannedExpensesHeader = new Cell(1, 2)
                .SetFontSize(16)
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetPadding(3)
                .Add(new Paragraph("Запланированные расходы").SetFont(arial));

            Cell cellPlannedExpensesType = new Cell(1, 1)
                .SetFontSize(14)
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetPadding(3)
                .Add(new Paragraph("Категория").SetFont(arial));

            Cell cellPlannedExpensesSum = new Cell(1, 1)
                .SetFontSize(14)
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetPadding(3)
                .Add(new Paragraph("Сумма").SetFont(arial));


            plannedExpensesTable.AddCell(cellPlannedExpensesHeader);
            plannedExpensesTable.AddCell(cellPlannedExpensesType);
            plannedExpensesTable.AddCell(cellPlannedExpensesSum);

            for (int i = 0; i < plannedExpenses.Count; i++)
            {
                plannedExpenses[i].ExpenseType = expenseTypes[i].Name;

                var cellType = new Cell(1, 1)
                    .SetFontSize(14)
                    .SetBackgroundColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(3)
                    .Add(new Paragraph(plannedExpenses[i].ExpenseType).SetFont(arial));

                var cellSum = new Cell(1, 1)
                    .SetFontSize(14)
                    .SetBackgroundColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(3)
                    .Add(new Paragraph(plannedExpenses[i].Sum.ToString()).SetFont(arial));

                plannedExpensesTable.AddCell(cellType);
                plannedExpensesTable.AddCell(cellSum);
            }
            document.Add(plannedExpensesTable);
            #endregion

            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            #region PlannedIncomesTable
            Table plannedIncomesTable = new Table(2, false)
                .SetHorizontalBorderSpacing(5);

            Cell cellPlannedIncomesHeader = new Cell(1, 2)
                .SetFontSize(16)
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetPadding(3)
                .Add(new Paragraph("Запланированные доходы").SetFont(arial));

            Cell cellPlannedIncomesType = new Cell(1, 1)
                .SetFontSize(14)
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetPadding(3)
                .Add(new Paragraph("Категория").SetFont(arial));

            Cell cellPlannedIncomesSum = new Cell(1, 1)
                .SetFontSize(14)
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetPadding(3)
                .Add(new Paragraph("Сумма").SetFont(arial));

            plannedIncomesTable.AddCell(cellPlannedIncomesHeader);
            plannedIncomesTable.AddCell(cellPlannedIncomesType);
            plannedIncomesTable.AddCell(cellPlannedIncomesSum);

            for (int i = 0; i < plannedIncomes.Count; i++)
            {
                plannedIncomes[i].IncomeType = incomeTypes[i].Name;

                var cellType = new Cell(1, 1)
                    .SetFontSize(14)
                    .SetBackgroundColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(3)
                    .Add(new Paragraph(plannedIncomes[i].IncomeType).SetFont(arial));

                var cellSum = new Cell(1, 1)
                    .SetFontSize(14)
                    .SetBackgroundColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(3)
                    .Add(new Paragraph(plannedIncomes[i].Sum.ToString()).SetFont(arial));

                plannedIncomesTable.AddCell(cellType);
                plannedIncomesTable.AddCell(cellSum);
            }
            document.Add(plannedIncomesTable);
            #endregion

            document.Close();
        }
        public bool BudgetExists(int id)
        {
            return _db.Budget.Exists(id);
        }
    }
}
