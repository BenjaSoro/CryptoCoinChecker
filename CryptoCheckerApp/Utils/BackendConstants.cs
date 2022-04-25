namespace CryptoChecker.Utils
{
    /// <summary>
    /// The backend util class constants.
    /// </summary>
    public static class BackendConstants
    {
        /*
         * Using an Android Emulator then asking for the localhost web service won't work,
         * because you're looking at the localhost of the emulator. How can you fix this?
         * Well, Android Emulator has a magic address http://10.0.2.2:your_port that points to 127.0.0.1:your_port on your host machine.
         * With IIS Express need to go into your project solution, in .vs folder->config->applicationhost.config
         * and change this <binding protocol="http" bindingInformation="*:PORT:localhost" /> into
         * <binding protocol="http" bindingInformation="*:PORT:127.0.0.1" />
         */

        /// <summary>
        /// The root backend url.
        /// </summary>
        public const string RootBackendUrl = "http://10.0.2.2:9989/";
    }
}