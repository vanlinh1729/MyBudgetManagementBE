namespace MyBudgetManagement.Persistence.Seed;

public static class DefaultRoles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Guest = "Guest";
    }

    public static class DefaultPermissions
    {
        // User permissions
        public const string ViewUsers = "Permissions.Users.View";
        public const string CreateUsers = "Permissions.Users.Create";
        public const string UpdateUsers = "Permissions.Users.Update";
        public const string DeleteUsers = "Permissions.Users.Delete";
        
        // Role permissions
        public const string ViewRoles = "Permissions.Roles.View";
        public const string CreateRoles = "Permissions.Roles.Create";
        public const string UpdateRoles = "Permissions.Roles.Update";
        public const string DeleteRoles = "Permissions.Roles.Delete";
        
        // Transaction permissions
        public const string ViewTransactions = "Permissions.Transactions.View";
        public const string CreateTransactions = "Permissions.Transactions.Create";
        public const string UpdateTransactions = "Permissions.Transactions.Update";
        public const string DeleteTransactions = "Permissions.Transactions.Delete";
        
        // Add other permissions as needed
    }

    public static class DefaultRolePermissions
    {
        public static Dictionary<string, List<string>> Get()
        {
            return new Dictionary<string, List<string>>
            {
                {
                    DefaultRoles.SuperAdmin,
                    new List<string>
                    {
                        DefaultPermissions.ViewUsers,
                        DefaultPermissions.CreateUsers,
                        DefaultPermissions.UpdateUsers,
                        DefaultPermissions.DeleteUsers,
                        DefaultPermissions.ViewRoles,
                        DefaultPermissions.CreateRoles,
                        DefaultPermissions.UpdateRoles,
                        DefaultPermissions.DeleteRoles,
                        DefaultPermissions.ViewTransactions,
                        DefaultPermissions.CreateTransactions,
                        DefaultPermissions.UpdateTransactions,
                        DefaultPermissions.DeleteTransactions
                    }
                },
                {
                    DefaultRoles.Admin,
                    new List<string>
                    {
                        DefaultPermissions.ViewUsers,
                        DefaultPermissions.CreateUsers,
                        DefaultPermissions.UpdateUsers,
                        DefaultPermissions.ViewRoles,
                        DefaultPermissions.ViewTransactions,
                        DefaultPermissions.CreateTransactions,
                        DefaultPermissions.UpdateTransactions
                    }
                },
                {
                    DefaultRoles.User,
                    new List<string>
                    {
                        DefaultPermissions.ViewTransactions,
                        DefaultPermissions.CreateTransactions,
                        DefaultPermissions.UpdateTransactions
                    }
                },
                {
                    DefaultRoles.Guest,
                    new List<string>
                    {
                        DefaultPermissions.ViewTransactions
                    }
                }
            };
        }
    }