using FluentValidation;
using System;
using Weapsy.Cqrs.Domain;
using Weapsy.Framework.Domain;
using Weapsy.Domain.EmailAccounts.Commands;
using Weapsy.Domain.EmailAccounts.Events;

namespace Weapsy.Domain.EmailAccounts
{
    public class EmailAccount : AggregateRoot
    {
        public Guid SiteId { get; private set; }
        public string Address { get; private set; }
        public string DisplayName { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public bool DefaultCredentials { get; private set; }
        public bool Ssl { get; private set; }
        public EmailAccountStatus Status { get; private set; }

        public EmailAccount() { }

        private EmailAccount(CreateEmailAccount cmd) : base(cmd.Id)
        {
            AddEvent(new EmailAccountCreated
            {
                SiteId = cmd.SiteId,
                AggregateRootId = Id,
                Address = cmd.Address,
                DisplayName = cmd.DisplayName,
                Host = cmd.Host,
                Port = cmd.Port,
                UserName = cmd.UserName,
                Password = cmd.Password,
                DefaultCredentials = cmd.DefaultCredentials,
                Ssl = cmd.Ssl,
                Status = EmailAccountStatus.Active
            });
        }

        public static EmailAccount CreateNew(CreateEmailAccount cmd, IValidator<CreateEmailAccount> validator)
        {
            validator.ValidateCommand(cmd);

            return new EmailAccount(cmd);
        }

        public void UpdateDetails(UpdateEmailAccountDetails cmd, IValidator<UpdateEmailAccountDetails> validator)
        {
            validator.ValidateCommand(cmd);

            AddEvent(new EmailAccountDetailsUpdated
            {
                SiteId = SiteId,
                AggregateRootId = Id,
                Address = cmd.Address,
                DisplayName = cmd.DisplayName,
                Host = cmd.Host,
                Port = cmd.Port,
                UserName = cmd.UserName,
                Password = cmd.Password,
                DefaultCredentials = cmd.DefaultCredentials,
                Ssl = cmd.Ssl
            });
        }

        public void Delete(DeleteEmailAccount cmd, IValidator<DeleteEmailAccount> validator)
        {
            validator.ValidateCommand(cmd);

            if (Status == EmailAccountStatus.Deleted)
                throw new Exception("Email account already deleted.");

            AddEvent(new EmailAccountDeleted
            {
                SiteId = SiteId,
                AggregateRootId = Id
            });
        }

        private void Apply(EmailAccountCreated @event)
        {
            Id = @event.AggregateRootId;
            SiteId = @event.SiteId;
            Address = @event.Address;
            DisplayName = @event.DisplayName;
            Host = @event.Host;
            Port = @event.Port;
            UserName = @event.UserName;
            Password = @event.Password;
            DefaultCredentials = @event.DefaultCredentials;
            Ssl = @event.Ssl;
            Status = EmailAccountStatus.Active;
        }

        private void Apply(EmailAccountDetailsUpdated @event)
        {
            Address = @event.Address;
            DisplayName = @event.DisplayName;
            Host = @event.Host;
            Port = @event.Port;
            UserName = @event.UserName;
            Password = @event.Password;
            DefaultCredentials = @event.DefaultCredentials;
            Ssl = @event.Ssl;
        }

        private void Apply(EmailAccountDeleted @event)
        {
            Status = EmailAccountStatus.Deleted;
        }
    }
}
