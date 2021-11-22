using System.Text.RegularExpressions;

namespace WSRecursosHumanos.Utils
{
    public class UsuarioUtils
    {
        public static bool IsUserSintaxCorrect(string user)
        {
            Regex rgx = new Regex(@"^[a-zA-Z0-9]+$");
            return rgx.IsMatch(user);
        }

        public static bool IsPasswordSintaxCorrect(string password)
        {
            Regex rgx = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
            return rgx.IsMatch(password);
        }
    }
}