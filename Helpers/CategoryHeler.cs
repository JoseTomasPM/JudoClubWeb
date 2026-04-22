namespace JudoClubWeb.Helpers;

public static class CategoryHelper
{
    public static string ToLabel(string category) => category switch
    {
        "Benjamin" => "Benjamín (Sub-11)",
        "Alevin" => "Alevín (Sub-13)",
        "Infantil" => "Infantil (Sub-15)",
        "Cadete" => "Cadete (Sub-17)",
        "Junior" => "Júnior (Sub-20)",
        "Sub23" => "Sub-23",
        "Senior" => "Senior",
        _ => category
    };
}