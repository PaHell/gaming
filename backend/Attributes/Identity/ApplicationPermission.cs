using Backend.Data.Enums.Identity;

namespace Backend.Attributes.Identity
{
      [AttributeUsage(AttributeTargets.Method)]
      public class ApplicationPermissionAttribute : Attribute
      {
            public ApplicationRole LowestRequiredRole { get; }

            public ApplicationPermissionAttribute(ApplicationRole lowestRequiredRole)
            {
                  LowestRequiredRole = lowestRequiredRole;
            }
      }
}