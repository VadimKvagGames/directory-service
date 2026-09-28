namespace DirectoryService.Domain.Shared;

public static class StringUtils
{
    public  static string TrimSpaces(this string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input == null)
        {
            return "";
        }

        return input.Trim();
    }

    public static string FirstCharacterToUpper(this string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input == null)
        {
            return "";
        }

        char[] characters = input.ToCharArray();
        char first = characters[0];
        string uppercase = first.ToString().ToUpper();
        char[] other = characters[1..characters.Length];
        string result = $"{uppercase}{new string(other)}";
        return result;
    }

}
