using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrsUtilities.UserUrl;

public class UserUrlBuilder
{
    private string? id, idgt, idlt, lnLowerLetter, lnUpperLetter, username, password, firstname, lastname, phone, email, isReviewer, isAdmin;

    public UserUrlBuilder(string? id, string? idgt, string? idlt, string? lnLowerLetter, string? lnUpperLetter, string? username, string? firstname, 
        string? lastname, string? phone, string? email, string? isReviewer, string? isAdmin)
    {
        this.id = id;
        this.idgt = idgt;
        this.idlt = idlt;
        this.username = username;
        this.firstname = firstname;
        this.lastname = lastname;
        this.lnLowerLetter = lnLowerLetter;
        this.lnUpperLetter = lnUpperLetter;
        this.phone = phone;
        this.email = email;
        this.isReviewer = isReviewer;
        this.isAdmin = isAdmin;
    }

    public UserUrlBuilder(string username, string firstname,
       string lastname, string? phone, string? email, string isReviewer, string isAdmin)
    {             
        this.username = username;        
        this.firstname = firstname;
        this.lastname = lastname;        
        this.phone = phone;
        this.email = email;
        this.isReviewer = isReviewer;
        this.isAdmin = isAdmin;
    }

    public string BuildUrl()
    {
        var urlBuilder = new StringBuilder("/api/User/search?");

        if (id != null)
        {
            string param = UserParameters.ID.ToParameterString();
            string query = $"{param.Replace("{id}", id.ToString())}&";
            urlBuilder.Append(query);
        }

        if (idgt != null)
        {
            string param = UserParameters.IDGT.ToParameterString();
            string query = $"{param.Replace("{idgt}", idgt.ToString())}&";
            urlBuilder.Append(query);
        }

        if (idlt != null)
        {
            string param = UserParameters.IDLT.ToParameterString();
            string query = $"{param.Replace("{idlt}", idlt.ToString())}&";
            urlBuilder.Append(query);

        }
        if (username != null)
        {            
            string param = UserParameters.USERNAME.ToParameterString();
            string query = $"{param.Replace("{username}", username.ToString())}&";
            urlBuilder.Append(query);

        }        
        if (firstname != null)
        {         
            string param = UserParameters.FIRSTNAME.ToParameterString();
            string query = $"{param.Replace("{firstname}", firstname.ToString())}&";
            urlBuilder.Append(query);

        }
        if (lastname != null)
        {         
            string param = UserParameters.LASTNAME.ToParameterString();
            string query = $"{param.Replace("{lastname}", lastname.ToString())}&";
            urlBuilder.Append(query);
        }

        if (lnLowerLetter != null)
        {         
            string param = UserParameters.LN_LOWER_LETTER.ToParameterString();
            string query = $"{param.Replace("{lnLowerLetter}", lnLowerLetter.ToString())}&";
            urlBuilder.Append(query);
        }

        if (lnUpperLetter != null)
        {         
            string param = UserParameters.LN_UPPER_LETTER.ToParameterString();
            string query = $"{param.Replace("{lnUpperLetter}", lnUpperLetter.ToString())}&";
            urlBuilder.Append(query);
        }
        if (phone != null)
        {
            string param = UserParameters.PHONE.ToParameterString();
            string query = $"{param.Replace("{phone}", phone.ToString())}&";
            urlBuilder.Append(query);

        }
        if (email != null)
        {         
            string param = UserParameters.EMAIL.ToParameterString();
            string query = $"{param.Replace("{email}", email.ToString())}&";
            urlBuilder.Append(query);

        }
        if (isReviewer != null)
        {
            string param = UserParameters.IS_REVIEWER.ToParameterString();
            string query = $"{param.Replace("{isReviewer}", isReviewer.ToString())}&";
            urlBuilder.Append(query);

        }
        if (isAdmin != null)
        {
            string param = UserParameters.IS_ADMIN.ToParameterString();
            string query = $"{param.Replace("{isAdmin}", isAdmin.ToString())}&";
            urlBuilder.Append(query);

        }
        string url = urlBuilder.ToString().TrimEnd('&');
        Debug.WriteLine($"the query string is: {url}");

        return urlBuilder.ToString();
    }
}
