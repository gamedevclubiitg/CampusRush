using UnityEngine;
using UnityEngine.Networking;

public static class DBops
{
    public static WWWForm GetDataForm(string email, string dataType)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("type", dataType);

        return form;
    }

    public static WWWForm SetCodeForm(string teamID, string code)
    {
        WWWForm form = new WWWForm();
        form.AddField("teamID", teamID);
        form.AddField("joinCode", code);
        
        return form;
    }

    public static WWWForm DeleteCodeForm(string teamID)
    {
        WWWForm form = new WWWForm();
        form.AddField("teamID", teamID);
        
        return form;
    }

    public static WWWForm RegistrationForm(string email, string name, string rolln, string teamID)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("name", name);
        form.AddField("rolln", rolln);
        form.AddField("teamID", teamID);

        return form;
    }

    public static WWWForm LoginForm(string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);

        return form;
    }

    public static WWWForm SetScoreForm(string email, string game, string score, string isGreater)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("game", game);
        form.AddField("score", score);
        form.AddField("isGreater", isGreater);


        return form;
    }

    public static WWWForm SetTimeScoreForm(string email, string game)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("game", game);

        return form;
    }

    public static WWWForm GetLeaderboardForm(string game, string limit, string isAsc)
    {
        WWWForm form = new WWWForm();
        form.AddField("limit", limit);
        form.AddField("game", game);
        form.AddField("asc", isAsc);

        return form;
    }



    public static string[] GetResultFromRequest(UnityWebRequest request)
    {
        string[] result = request.downloadHandler.text.Split('\t');
        return result;
    }
    
}