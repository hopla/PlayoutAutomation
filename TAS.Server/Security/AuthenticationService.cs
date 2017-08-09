﻿using System;
using System.Collections.Generic;
using TAS.Remoting.Server;
using TAS.Server.Common;
using TAS.Server.Common.Interfaces;

namespace TAS.Server.Security
{
    public class AuthenticationService: DtoBase, IAuthenticationService, IAuthenticationServicePersitency
    {
        private readonly AcoHive<IUser> _users;
        private readonly AcoHive<IGroup> _groups;

        public AuthenticationService(List<User> users, List<Group> groups)
        {
            users.ForEach(u =>
            {
                u.AuthenticationService = this;
                u.PopulateGroups(groups);
            });
            groups.ForEach(g => g.AuthenticationService = this);

            _users = new AcoHive<IUser>(users);
            _users.AcoOperartion += Users_AcoOperation;

            _groups = new AcoHive<IGroup>(groups);
            _groups.AcoOperartion += Groups_AcoOperation;
        }

        public IList<IUser> Users => _users.Items;

        public IList<IGroup> Groups => _groups.Items;

        public IUser CreateUser() => new User(this);

        public IGroup CreateGroup() => new Group(this);

        public bool AddUser(IUser user) => _users.Add((User)user);
        
        public bool RemoveUser(IUser user) => _users.Remove((User)user);

        public bool AddGroup(IGroup group) => _groups.Add((Group)group);

        public bool RemoveGroup(IGroup group) => _groups.Remove((Group)group);

        public ISecurityObject FindSecurityObject(ulong id)
        {
            return (ISecurityObject)_groups.FindById(id) ?? _users.FindById(id);
        }


        public event EventHandler<CollectionOperationEventArgs<IUser>> UsersOperation;

        public event EventHandler<CollectionOperationEventArgs<IGroup>> GroupsOperation;

        protected override void DoDispose()
        {
            _users.AcoOperartion -= Users_AcoOperation;
            _groups.AcoOperartion -= Groups_AcoOperation;
            base.DoDispose();
        }

        private void Users_AcoOperation(object sender, CollectionOperationEventArgs<IUser> e)
        {
            UsersOperation?.Invoke(this, new CollectionOperationEventArgs<IUser>(e.Item, e.Operation));
        }

        private void Groups_AcoOperation(object sender, CollectionOperationEventArgs<IGroup> e)
        {
            GroupsOperation?.Invoke(this, new CollectionOperationEventArgs<IGroup>(e.Item, e.Operation));
        }
    }
}