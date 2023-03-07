public enum UserParameters
{
    ID = 0,
    IDGT = 1,
    IDLT = 2,
    USERNAME = 3,
    PASSWORD = 4,
    FIRSTNAME = 5,
    LASTNAME = 6,
    LN_LOWER_LETTER = 7,
    LN_UPPER_LETTER = 8,
    PHONE = 9,
    EMAIL = 10,
    IS_REVIEWER = 11,
    IS_ADMIN = 12
}

public static class UserParametersExtensions
{
    private static readonly Dictionary<UserParameters, string> _parameterStrings = new Dictionary<UserParameters, string>
    {
        { UserParameters.ID, "id={id}" },
        { UserParameters.IDGT, "idgt={idgt}" },
        { UserParameters.IDLT, "idlt={idlt}" },
        { UserParameters.USERNAME, "username={username}" },
        { UserParameters.PASSWORD, "password={password}" },
        { UserParameters.FIRSTNAME, "firstname={firstname}" },
        { UserParameters.LASTNAME, "lastname={lastname}" },
        { UserParameters.LN_LOWER_LETTER, "ln-lower-letter={lnLowerLetter}" },
        { UserParameters.LN_UPPER_LETTER, "ln-upper-letter={lnUpperLetter}" },
        { UserParameters.PHONE, "phone={phone}" },
        { UserParameters.EMAIL, "email={email}" },
        { UserParameters.IS_REVIEWER, "is-reviewer={isReviewer}" },
        { UserParameters.IS_ADMIN, "is-admin={isAdmin}" },
    };

    public static string ToParameterString(this UserParameters parameter)
    {
        if (_parameterStrings.TryGetValue(parameter, out var parameterString))
        {
            return parameterString;
        }

        return null;
    }
}
