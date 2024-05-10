using System;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class PostalCodeAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            // Postal code is not required, so it's considered valid if it's null or empty
            return true;
        }

        // Regular expression for postal code validation (adjust as needed for your specific requirements)
        string postalCodePattern = @"^[1-9]\d{4}$"; // 5 digits, not starting with 

        return System.Text.RegularExpressions.Regex.IsMatch(value.ToString(), postalCodePattern);
    }
}