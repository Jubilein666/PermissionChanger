using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace PermissionChanger
{
    [Serializable]
    class DirectoryInformations : List<DirectoryControl>
    {

    }
    [Serializable]
    public class DirectoryControl : ISerializable
    {
        #region Properties

        public MyAuthorizationRuleCollection OriginalAuthorizationRuleCollection { get; }

        public string Directory { get; }

        #endregion

        #region Fields

        private DirectoryInfo _directoryInfo;

        #endregion

        #region Constructor

        public DirectoryControl(string path)
        {
            this.Directory = path;
            _directoryInfo = new DirectoryInfo(path);

            RemoveInherited();

            DirectorySecurity dSecurity = _directoryInfo.GetAccessControl();

            OriginalAuthorizationRuleCollection = new MyAuthorizationRuleCollection(dSecurity.GetAccessRules(true, false, typeof(NTAccount)));
        }

        #endregion

        #region Methods

        private void RemoveInherited()
        {
            DirectorySecurity dSecurity = _directoryInfo.GetAccessControl();

            dSecurity.SetAccessRuleProtection(true, true);

            _directoryInfo.SetAccessControl(dSecurity);
        }

        public void RemoveFullControl()
        {
            DirectorySecurity dSecurity = _directoryInfo.GetAccessControl();

            foreach (FileSystemAccessRule item in dSecurity.GetAccessRules(true, false, typeof(NTAccount)))
            {
                dSecurity.RemoveAccessRule(item);

                dSecurity.AddAccessRule(new FileSystemAccessRule(item.IdentityReference.Value, FileSystemRights.ReadAndExecute | FileSystemRights.Synchronize, item.InheritanceFlags, item.PropagationFlags, AccessControlType.Allow));
            }

            _directoryInfo.SetAccessControl(dSecurity);
        }

        public void AddFullControl()
        {
            DirectorySecurity dSecurity = _directoryInfo.GetAccessControl();

            foreach (FileSystemAccessRule item in dSecurity.GetAccessRules(true, false, typeof(NTAccount)))
            {
                dSecurity.RemoveAccessRule(item);
            }
            foreach (var item in OriginalAuthorizationRuleCollection)
            {
                dSecurity.AddAccessRule(new FileSystemAccessRule(item.IdentityReferenceValue, item.FileSystemRights, item.InheritanceFlags, item.PropagationFlags, item.AccessControlType));
            }

            _directoryInfo.SetAccessControl(dSecurity);
        }

        public bool CheckRestictedPermission()
        {
            DirectorySecurity dSecurity = _directoryInfo.GetAccessControl();

            if (dSecurity.GetAccessRules(true, false, typeof(NTAccount)).Cast<FileSystemAccessRule>().Any(x => x.FileSystemRights != (FileSystemRights.ReadAndExecute | FileSystemRights.Synchronize)))
                return false;
            return true;
        }

        #endregion

        #region Serialization

        protected DirectoryControl(SerializationInfo info, StreamingContext context)
        {
            this.Directory = info.GetString("Directory");
            OriginalAuthorizationRuleCollection = (MyAuthorizationRuleCollection)info.GetValue("OriginalAuthorizationRuleCollection", typeof(MyAuthorizationRuleCollection));

            _directoryInfo = new DirectoryInfo(this.Directory);
        }

        [SecurityPermission(SecurityAction.LinkDemand,
            Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Directory", this.Directory);
            info.AddValue("OriginalAuthorizationRuleCollection", OriginalAuthorizationRuleCollection);
        }

        #endregion
    }

    [Serializable]
    public class MyAuthorizationRuleCollection : List<MyFileSystemAccessRule>
    {
        public MyAuthorizationRuleCollection(AuthorizationRuleCollection authorizationRuleCollection) : base()
        {
            foreach (FileSystemAccessRule item in authorizationRuleCollection)
            {
                this.Add(new MyFileSystemAccessRule(item));
            }
        }
    }

    [Serializable]
    public class MyFileSystemAccessRule
    {
        #region Properties

        public AccessControlType AccessControlType { get; set; }

        public FileSystemRights FileSystemRights { get; set; }

        public string IdentityReferenceValue { get; set; }

        public InheritanceFlags InheritanceFlags { get; set; }

        public bool IsInherited { get; set; }

        public PropagationFlags PropagationFlags { get; set; }

        #endregion

        #region Constructor

        public MyFileSystemAccessRule(FileSystemAccessRule fileSystemAccessRule)
        {
            this.AccessControlType = fileSystemAccessRule.AccessControlType;
            this.FileSystemRights = fileSystemAccessRule.FileSystemRights;
            this.IdentityReferenceValue = fileSystemAccessRule.IdentityReference.Value;
            this.InheritanceFlags = fileSystemAccessRule.InheritanceFlags;
            this.IsInherited = fileSystemAccessRule.IsInherited;
            this.PropagationFlags = fileSystemAccessRule.PropagationFlags;
        }

        #endregion
    }
}
