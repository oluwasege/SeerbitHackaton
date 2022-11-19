using SeerbitHackaton.Core.Extensions;

namespace SeerbitHackaton.Core.DataAccess.EfCore
{
    public static class EntityAuditingHelper
    {
        public static void SetCreationAuditProperties(
            object entityAsObj,
            long? userId)
        {
            if (!(entityAsObj is IHasCreationTime entityWithCreationTime))
            {
                //Object does not implement IHasCreationTime
                return;
            }

            if (entityWithCreationTime.CreationTime == default(DateTime))
            {
                entityWithCreationTime.CreationTime = Clock.Now;
            }

            if (!(entityAsObj is ICreationAudited))
            {
                //Object does not implement ICreationAudited
                return;
            }

            if (!userId.HasValue)
            {
                //Unknown user
                return;
            }

            var entity = entityAsObj as ICreationAudited;
            if (entity.CreatorUserId != null)
            {
                //CreatorUserId is already set
                return;
            }

            //Finally, set CreatorUserId!
            entity.CreatorUserId = userId;
        }

        public static void SetModificationAuditProperties(
            object entityAsObj,
            long? userId)
        {
            if (entityAsObj is IHasModificationTime)
            {
                entityAsObj.As<IHasModificationTime>().LastModificationTime = Clock.Now;
            }

            if (!(entityAsObj is IModificationAudited))
            {
                //Entity does not implement IModificationAudited
                return;
            }

            var entity = entityAsObj.As<IModificationAudited>();

            if (userId == null)
            {
                //Unknown user
                entity.LastModifierUserId = null;
                return;
            }

            //Finally, set LastModifierUserId!
            entity.LastModifierUserId = userId;
        }
    }
}