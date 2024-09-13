namespace EmailClient
{
    public class EmailAgentCredentials
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public EmailAgentCredentials(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
