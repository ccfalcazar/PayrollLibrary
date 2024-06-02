using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayrollLibrary;

namespace KarpaUnitTesting
{
    [TestClass]
    public class PayrollTesting
    {
        private Payroll pay;
        private double[] SSSEmployeeContributionTable =
           {36.30,54.5,72.7,90.80,109.00,127.20,145.30,
            163.50, 181.70,199.80, 218.00,236.20,254.30,
            272.50, 290.70, 308.80, 327.00, 345.20, 363.30,
            381.50, 399.70,417.80,436.00,454.20,472.30,490.50,
            508.70,526.80,545.00,563.20,581.30};
        double SSSBracket = 1250;
        double PhilHealthBracket = 9000;
        double PhilHealthContribution = 100;
        int index = 0;

        private void Initialize()
        {
            pay = new Payroll(TaxBracketType.MONTHLY);
        }

        private void Initialize(double Rate, double DaysWorked)
        {
            Initialize();
            pay.DaysWorked = DaysWorked;
            pay.Rate = Rate;
        }

        #region SSSComputation
        private void LoopThroughIndex()
        {
            while (index < 30)
            {
                TestIfBasicSalaryIsGreaterThanSalaryBracket();
                Assert.AreEqual(SSSEmployeeContributionTable[index], pay.SSSContribution);
                pay.DaysWorked += 0.1;
            }
        }

        private void TestIfBasicSalaryIsGreaterThanSalaryBracket()
        {
            if (pay.GrossSalary > SSSBracket)
                IncrementIndexAndBracket();
        }

        private void IncrementIndexAndBracket()
        {
            SSSBracket += 500;
            index++;
        }
        #endregion

        #region PhilHealthComputation
        private void TestIfBasicSalaryIsGreaterThanPhilHealthBracket()
        {
            if (pay.GrossSalary >= PhilHealthBracket)
            {
                IncrementBracketAndContribution();
            }
        }

        private void IncrementBracketAndContribution()
        {
            PhilHealthBracket += 1000;
            PhilHealthContribution += 12.5;
        }
        #endregion

        #region TestMethods
        [TestMethod]
        public void ShouldBeAbleToHaveAnEmployeeName()
        {
            Initialize();
            pay.EmployeeName = "Carlo Alcazar";
            StringAssert.Equals(pay.EmployeeName, "Carlo Alcazar");
        }
        [TestMethod]
        public void ShouldBeAbleToTestForTheLowestSSSContributionToTheHighestContribution()
        {
            Initialize(1000, 1);
            LoopThroughIndex();
        }
        [TestMethod]
        public void ShouldBeAbleToTestForTheLowestPhilHealthContributionToTheHighestContribution()
        {
            Initialize(1000, 1);
            while (pay.GrossSalary < 36000)
            {
                TestIfBasicSalaryIsGreaterThanPhilHealthBracket();
                Assert.AreEqual(PhilHealthContribution, pay.PhilhealthContribution);
                pay.DaysWorked += 0.1;
            }
        }
        [TestMethod]
        public void ShouldBeAbleToComputeForThePagIbigContribution()
        {
            Initialize();
            Assert.AreEqual(100, pay.PagIbigContribution);
        }
        #endregion

        [TestMethod]
        public void ShouldBeAbleToComputeForEmployerContribution()
        {
            Payroll generate = new Payroll(TaxBracketType.MONTHLY);
            generate.DaysWorked = 1;
            generate.Rate = 13250;

            Assert.AreEqual(1004.5, generate.EmployerShare);
        }
    }

}
