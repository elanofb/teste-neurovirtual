using DAL.Repository.Base;

namespace System {

    
    public static class DataContextExtensions {

        public static void disableTrackChanges(this DataContext db) {

            db.Configuration.AutoDetectChangesEnabled = false;

            db.Configuration.ValidateOnSaveEnabled = false;

        }
    }

}
