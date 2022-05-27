using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FireFightersApp.Authorization
{
    public class CallOperations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = Constants.CreateOperationName };
        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement { Name = Constants.ReadOperationName };
        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = Constants.UpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = Constants.DeleteOperationName };
        public static OperationAuthorizationRequirement Assigned =
            new OperationAuthorizationRequirement { Name = Constants.AssignedOperationName };
        public static OperationAuthorizationRequirement Completed =
            new OperationAuthorizationRequirement { Name = Constants.CompletedOperationName };
    }

    public class Constants
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperationName = "Read";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";

        public static readonly string AssignedOperationName = "Assigned";
        public static readonly string CompletedOperationName = "Completed";

        public static readonly string CallDispatcherRole = "CallDispatcher";
        public static readonly string CallAdminRole = "CallAdmin";
    }
}
