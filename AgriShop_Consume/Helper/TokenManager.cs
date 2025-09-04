namespace AgriShop_Consume.Helper
{
   
        public static class TokenManager
        {
            public static string Token { get; set; }
            public static string Role { get; set; }
            public static string Email { get; set; }

           public static string Username { get; set; }

            public static bool IsAdmin => Role == "Admin";
        }

    

}
