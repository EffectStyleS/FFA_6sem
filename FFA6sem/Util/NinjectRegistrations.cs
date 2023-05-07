using FFA6sem.Model.Interfaces;
using FFA6sem.Model.Services;
using Ninject.Modules;

namespace FFA6sem.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IBudgetService>().To<BudgetService>();
            Bind<IExpenseService>().To<ExpenseService>();
            Bind<IExpenseTypeService>().To<ExpenseTypeService>();
            Bind<IIncomeService>().To<IncomeService>();
            Bind<IIncomeTypeService>().To<IncomeTypeService>();
            Bind<IPlannedExpensesService>().To<PlannedExpensesService>();
            Bind<IPlannedIncomesService>().To<PlannedIncomesService>();
            Bind<IReportService>().To<ReportService>();
            Bind<ITimePeriodService>().To<TimePeriodService>();
            Bind<IUserService>().To<UserService>();
            Bind<IDbCrud>().To<DbDataOperation>();
        }
    }
}
