using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace ToolPreSelectionEmbeddings.Plugins;

public class FinancePlugin
{
    [KernelFunction, Description("Calculates compound interest")]
    public double CalculateCompoundInterest(
        [Description("Principal amount")] double principal,
        [Description("Annual interest rate (as percentage)")] double rate,
        [Description("Time in years")] int years,
        [Description("Compounds per year")] int compoundsPerYear = 12)
    {
        double r = rate / 100.0;
        return principal * Math.Pow(1 + r / compoundsPerYear, compoundsPerYear * years);
    }

    [KernelFunction, Description("Calculates monthly loan payment")]
    public double CalculateLoanPayment(
        [Description("Loan amount")] double principal,
        [Description("Annual interest rate (as percentage)")] double rate,
        [Description("Loan term in years")] int years)
    {
        double monthlyRate = rate / 1200.0;
        int months = years * 12;
        return principal * monthlyRate * Math.Pow(1 + monthlyRate, months) 
               / (Math.Pow(1 + monthlyRate, months) - 1);
    }

    [KernelFunction, Description("Formats currency value")]
    public string FormatCurrency(
        [Description("Amount to format")] double amount,
        [Description("Currency code (USD, EUR, etc.)")] string currencyCode = "USD")
    {
        return currencyCode.ToUpper() switch
        {
            "USD" => $"${amount:N2}",
            "EUR" => $"€{amount:N2}",
            "GBP" => $"£{amount:N2}",
            "JPY" => $"¥{amount:N0}",
            _ => $"{amount:N2} {currencyCode}"
        };
    }
} 