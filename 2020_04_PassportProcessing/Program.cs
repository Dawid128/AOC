using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

var input = File.ReadAllText($"Resources\\Input.txt");
var stopwatch = Stopwatch.StartNew();

//Part 1
stopwatch.Start();
var output = Part1(input, new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" });
stopwatch.Stop();
Console.WriteLine($"Output Part1: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

//Part 2
stopwatch.Start();
output = Part2(input, new PassportValidator());
stopwatch.Stop();
Console.WriteLine($"Output Part2: {output}");
Console.WriteLine($"Done in Time: {stopwatch.ElapsedMilliseconds} ms");

object Part1(string input, IList<string> requiredFields)
{
    var passports = input.Split($"{Environment.NewLine}{Environment.NewLine}")
                         .Select(TakePassportFields)
                         .ToList();

    int count = 0;
    foreach (var passport in passports.Select(x => x.Keys))
        if (requiredFields.Intersect(passport).Count() == requiredFields.Count)
            count++;

    return count;
}

object Part2(string input, PassportValidator passportValidator)
{
    var passports = input.Split($"{Environment.NewLine}{Environment.NewLine}")
                         .Select(TakePassportFields)
                         .ToList();

    int count = 0;
    foreach (var passport in passports)
        if (passportValidator.Validate(passport))
            count++;

    return count;
}

Dictionary<string, string> TakePassportFields(string input)
{
    var fields = input.Split(Environment.NewLine)
                      .SelectMany(x => x.Split(" "));

    var result = new Dictionary<string, string>();
    foreach (var field in fields)
    {
        var split = field.Split(":");
        result.Add(split[0], split[1]);
    }

    return result;
}

partial class PassportValidator
{

    [GeneratedRegex("^(#[a-f0-9]{6})$")]
    private static partial Regex HairColorRegex();

    [GeneratedRegex("^(\\d+cm)$")]
    private static partial Regex HeightCMRegex();

    [GeneratedRegex("^(\\d+in)$")]
    private static partial Regex HeightINRegex();

    [GeneratedRegex("^(\\d{9})$")]
    private static partial Regex PassportIDRegex();

    private static List<string> _eyeColorList = new() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

    public bool Validate(Dictionary<string, string> properties)
    {
        var validateMethods = typeof(PassportValidator).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                                                       .Where(method => Attribute.IsDefined(method, typeof(MethodValidateAttribute)))
                                                       .Select(x => (MethodInfo: x, MethodAttribute: Attribute.GetCustomAttribute(x, typeof(MethodValidateAttribute)) as MethodValidateAttribute))
                                                       .Where(x => x.MethodAttribute is not null)
                                                       .Select(x => (x.MethodInfo, MethodAttribute: x.MethodAttribute!))
                                                       .ToList();

        if (validateMethods.Select(x => x.MethodAttribute.ShortName).Intersect(properties.Keys).Count() != validateMethods.Count)
            return false;

        foreach (var (methodInfo, methodAttribute) in validateMethods)
        {
            if (!properties.TryGetValue(methodAttribute.ShortName, out var value))
                return false;

            if (methodInfo.Invoke(this, new[] { value }) is not true)
                return false;
        }

        return true;
    }

    [MethodValidate("byr")]
    private bool ValidateBirthYear(string value) => ValidateNumberInRange(value, 1920, 2002);

    [MethodValidate("iyr")]
    private bool ValidateIssueYear(string value) => ValidateNumberInRange(value, 2010, 2020);

    [MethodValidate("eyr")]
    private bool ValidateExpirationYear(string value) => ValidateNumberInRange(value, 2020, 2030);

    [MethodValidate("hgt")]
    private bool ValidateHeight(string value)
    {
        if (ValidateRegex(value, HeightCMRegex()))
            return ValidateNumberInRange(value.Replace("cm", string.Empty), 150, 193);

        if (ValidateRegex(value, HeightINRegex()))
            return ValidateNumberInRange(value.Replace("in", string.Empty), 59, 76);

        return false;
    }

    [MethodValidate("hcl")]
    private bool ValidateHairColor(string value) => ValidateRegex(value, HairColorRegex());

    [MethodValidate("ecl")]
    private bool ValidateEyeColor(string value) => ValidateStringInList(value, _eyeColorList);

    [MethodValidate("pid")]
    private bool ValidatePassportID(string value) => ValidateRegex(value, PassportIDRegex());

    private bool ValidateNumberInRange(string value, int min, int max)
    {
        if (!int.TryParse(value, out var result))
            return false;

        if (result < min || result > max)
            return false;

        return true;
    }

    private bool ValidateRegex(string value, Regex regex)
    {
        if (!regex.IsMatch(value))
            return false;

        return true;
    }

    private bool ValidateStringInList(string value, IList<string> items)
    {
        if (!items.Contains(value))
            return false;

        return true;
    }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
sealed class MethodValidateAttribute : Attribute
{
    public string ShortName { get; }

    public MethodValidateAttribute(string shortName)
    {
        ShortName = shortName;
    }
}