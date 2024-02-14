namespace FacilitEase.Services
{
    public interface ILoginService
    {
        /// <summary>
        /// To check if the login user is present in the user table
        /// </summary>
        /// <param name="username">The email id of the user that sign ins</param>
        /// <returns>JWT token created by the application</returns>
        object CheckUser(string username);
    }
}