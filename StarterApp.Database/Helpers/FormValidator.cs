namespace StarterApp.Database.Helpers;

public static class FormValidator
{
    public static bool ValidateItemForm(string title, string description, string dailyRate, string category, out string error)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            error = "Title is required";
            return false;
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            error = "Description is required";
            return false;
        }

        if (string.IsNullOrWhiteSpace(dailyRate))
        {
            error = "Daily rate is required";
            return false;
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            error = "Please enter a category";
            return false;
        }

        if (!decimal.TryParse(dailyRate, out var rate))
        {
            error = "Daily rate must be a valid decimal number";
            return false;
        }

        if (rate < 0)
        {
            error = "DailyRate cannot be negative";
            return false;
        }

        error = string.Empty;
        return true;
    }

    public static bool ValidateReviewForm(string name, string description, string rating, out string error)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            error = "Name is required";
            return false;
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            error = "Description is required";
            return false;
        }

        if (string.IsNullOrWhiteSpace(rating))
        {
            error = "Rating is required";
            return false;
        }

        if (!decimal.TryParse(rating, out var r) || r < 1 || r > 5)
        {
            error = "Rating must be a number between 1 and 5";
            return false;
        }

        error = string.Empty;
        return true;
    }
}
