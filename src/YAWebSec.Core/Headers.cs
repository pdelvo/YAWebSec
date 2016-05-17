namespace YAWebSec.Core {
    public static class Headers {
        public const string XssProtection = "X-Xss-Protection";
        public const string XFrameOptions = "X-Frame-Options";
        public const string StrictTransportSecurity = "Strict-Transport-Security";
        public const string Location = "Location";
        
        public const string ContentSecurityPolicy = "Content-Security-Policy";
        public const string ContentSecurityPolicyReportOnly = "Content-Security-Policy-Report-Only";
        public const string PublicKeyPinning = "Public-Key-Pins";

    }
}