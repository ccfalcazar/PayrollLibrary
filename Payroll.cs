using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace PayrollLibrary
{
    public class Payroll
    {
        private string _EmployeeID;
        private double _Rate;
        private double _DaysWorked;
        private double[] SSSEmployeeContributionTable =
            {36.30,54.5,72.7,90.80,109.00,127.20,145.30,
            163.50, 181.70,199.80, 218.00,236.20,254.30,
            272.50, 290.70, 308.80, 327.00, 345.20, 363.30,
            381.50, 399.70,417.80,436.00,454.20,472.30,490.50,
            508.70,526.80,545.00,563.20,581.30};
        private WithHoldingTaxTable _TaxTable;
        private TaxBracketType _BracketType;
        private DataTable _Payables;
        private int _Dependents;
        private int _SalaryRange;
        private string _SSSID, _PhilHealthID, _PagIbigID;

        public Payroll(TaxBracketType BracketType)
        {
            _BracketType = BracketType;
            _Payables = new DataTable();
        }
        public void FillGovernmentID(string SSSID, string PhilHealthID, string PagIbigID)
        {
            _SSSID = SSSID;
            _PhilHealthID = PhilHealthID;
            _PagIbigID = PagIbigID;
        }
        #region Deductions
        public double SSSContribution
        {
            get
            {
                if (_SSSID == "")
                    return 0;

                int ContributionIndex = 0;
                for (_SalaryRange = 1250; GrossSalary >= _SalaryRange; _SalaryRange += 500)
                {
                    if(ContributionIndex < 30)
                    ContributionIndex++;
                }
                return SSSEmployeeContributionTable[ContributionIndex];
            }
        }
        public double EmployerShare
        {
            get
            {
                double EmployeeContribution = SSSContribution;
                double Additional = 10;
                if (_SalaryRange > 14500)
                    Additional = 30;
                return TotalContributionShare - EmployeeContribution + Additional;
            }
        }
        private double TotalContributionShare
        {
            get
            {
                return (_SalaryRange - 250.0) * 0.11;
            }
        }
        public double PhilhealthContribution
        {
            get
            {
                if (_PhilHealthID == "")
                    return 0;

                double PhilHealthSalaryBracket = 35000;
                double _PhilHealthContribution = 437.50;

                do
                {
                    if (GrossSalary >= PhilHealthSalaryBracket)
                        return _PhilHealthContribution;
                    else
                    {
                        PhilHealthSalaryBracket -= 1000;
                        _PhilHealthContribution -= 12.50;
                    }
                } while (PhilHealthSalaryBracket > 8000);

                return _PhilHealthContribution;
            }
        }
        public double PagIbigContribution
        {
            get
            {
                if (_PagIbigID == "")
                    return 0;

                return 100;
            }
        }
        #endregion()

        #region Properties
        public double WithHoldingTax
        {
            get
            {
                //_TaxTable = new WithHoldingTaxTable(TotalSalary, _BracketType, _Dependents);
                return 0;
            }
        }
        public int Dependents
        {
            get
            {
                return _Dependents;
            }
            set
            {
                _Dependents = value;
            }
        }
        public double TotalSalary
        {
            get
            {
                if (DaysWorked == 0)
                    return 0;
                return GrossSalary - GrossDeduction;
            }
        }
        public double PartialNetSalary
        {
            get
            {
                return TotalSalary - WithHoldingTax;
            }
        }
        public double GrossSalary
        {
            get
            {
                return _Rate * _DaysWorked;
            }
        }
        public double GrossDeduction
        {
            get
            {
                return SSSContribution + PagIbigContribution + PhilhealthContribution;
            }
        }
        public string EmployeeName
        {
            get
            {
                return _EmployeeID;
            }
            set
            {
                _EmployeeID = value;
            }
        }
        public double Rate
        {
            get
            {
                return _Rate;
            }
            set
            {
                _Rate = value;
            }
        }
        public double DaysWorked
        {
            get
            {
                return _DaysWorked;
            }
            set
            {
                _DaysWorked = value;
            }
        }
        public DataTable Payables
        {
            get
            {
                return _Payables;
            }
            set
            {
                _Payables = value;
            }
        }
        public double NetSalary
        {
            get
            {
                return PartialNetSalary + ComputePayables();
            }
        }
        private double ComputePayables()
        {
            double Total = 0;
            try
            {
                for (int index = 0; index < _Payables.Rows.Count; index++)
                    Total += double.Parse(_Payables.Rows[index]["Amount"].ToString());
            }
            catch (Exception)
            {
                Total = 0;
            }            
            return Total;
        }
        #endregion
    }
}
