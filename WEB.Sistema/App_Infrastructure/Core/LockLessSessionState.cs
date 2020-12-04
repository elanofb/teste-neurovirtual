using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;
    
namespace WEB.App_Infrastructure.Core {
    
    public class LocklessInProcSessionStateStore : SessionStateStoreProviderBase {
        
        private SessionStateStoreProviderBase _store;

        public override void Initialize(string name, NameValueCollection config) {
            base.Initialize(name, config);

            var storeType = typeof(SessionStateStoreProviderBase).Assembly.GetType("System.Web.SessionState.InProcSessionStateStore");
            _store = (SessionStateStoreProviderBase) Activator.CreateInstance(storeType);
            _store.Initialize(name, config);
        }

        public override void Dispose() {
            _store.Dispose();
        }

        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback) {
            return _store.SetItemExpireCallback(expireCallback);
        }

        public override void InitializeRequest(HttpContext context) {
            _store.InitializeRequest(context);
        }

        public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId,
            out SessionStateActions actions) {
            var returnValue = _store.GetItem(context, id, out locked, out lockAge, out lockId, out actions);
            if (returnValue == null && lockId != null) {
                _store.ReleaseItemExclusive(context, id, lockId);
                returnValue = _store.GetItem(context, id, out locked, out lockAge, out lockId, out actions);
            }

            return returnValue;
        }

        public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge,
            out object lockId, out SessionStateActions actions) {
            var returnValue = _store.GetItemExclusive(context, id, out locked, out lockAge, out lockId, out actions);
            if (returnValue == null && lockId != null) {
                _store.ReleaseItemExclusive(context, id, lockId);
                returnValue = _store.GetItemExclusive(context, id, out locked, out lockAge, out lockId, out actions);
            }

            return returnValue;
        }

        public override void ReleaseItemExclusive(HttpContext context, string id, object lockId) {
            _store.ReleaseItemExclusive(context, id, lockId);
        }

        public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem) {
            _store.SetAndReleaseItemExclusive(context, id, item, lockId, newItem);
        }

        public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item) {
            _store.RemoveItem(context, id, lockId, item);
        }

        public override void ResetItemTimeout(HttpContext context, string id) {
            _store.ResetItemTimeout(context, id);
        }

        public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout) {
            return _store.CreateNewStoreData(context, timeout);
        }

        public override void CreateUninitializedItem(HttpContext context, string id, int timeout) {
            _store.CreateUninitializedItem(context, id, timeout);
        }

        public override void EndRequest(HttpContext context) {
            _store.EndRequest(context);
        }
    }
}
