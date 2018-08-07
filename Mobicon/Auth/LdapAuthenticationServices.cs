using System;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;

namespace Mobicon.Auth
{
    public class LdapConfig
    {
        public string Url { get; set; }
        public string BindDn { get; set; }
        public string BindCredentials { get; set; }
        public string SearchBase { get; set; }
        public string SearchFilter { get; set; }
    }

    public class LdapAuthenticationService
    {
        private const string MemberOfAttribute = "memberOf";
        private const string DisplayNameAttribute = "displayName";
        private const string SAMAccountNameAttribute = "sAMAccountName";

        private readonly LdapConfig _config;
        private readonly LdapConnection _connection;

        public LdapAuthenticationService(IOptions<LdapConfig> config)
        {
            _config = config.Value;
            _connection = new LdapConnection();
        }

        public ApplicationUser Login(string username, string password)
        {
            _connection.Connect(_config.Url, LdapConnection.DEFAULT_SSL_PORT);
            _connection.Bind(_config.BindDn, _config.BindCredentials);

            var searchFilter = string.Format(_config.SearchFilter, username);
            var result = _connection.Search(
                _config.SearchBase,
                LdapConnection.SCOPE_SUB,
                searchFilter,
                new[] { MemberOfAttribute, DisplayNameAttribute, SAMAccountNameAttribute },
                false
            );

            try
            {
                var user = result.Next();
                if (user != null)
                {
                    _connection.Bind(user.DN, password);
                    if (_connection.Bound)
                    {
                        return new ApplicationUser
                        {
                            UserName = user.getAttribute(SAMAccountNameAttribute).StringValue,
                            FullName = user.getAttribute(DisplayNameAttribute).StringValue
                        };
                    }
                }
            }
            catch
            {
                throw new Exception("Login failed.");
            }
            _connection.Disconnect();
            return null;
        }

    }
}
