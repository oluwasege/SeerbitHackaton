using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.Utils
{
    public class SmtpConfigSettings
    {
        /// <summary>
        /// Gets or sets the Host
        /// </summary>
        /// <value>The Host</value>
        public string Host { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [use s sl].
        /// </summary>
        /// <value><c>true</c> if [use s sl]; otherwise, <c>false</c>.</value>
        public int Port { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        /// <value>The password.</value>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the Sender Email Address
        /// </summary>
        /// <value>The Email</value>
        public string Mail { get; set; }
        /// <summary>
        /// Gets or sets the DisplayName
        /// </summary>
        /// <value>The DisplayName</value>
        public string DisplayName { get; set; }
    }
}
