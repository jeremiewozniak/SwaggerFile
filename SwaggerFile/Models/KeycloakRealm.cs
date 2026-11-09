using Newtonsoft.Json;

namespace SwaggerFile.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Account
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool composite { get; set; }
        public bool clientRole { get; set; }
        public string containerId { get; set; }
        public Attributes attributes { get; set; }
        public Composites composites { get; set; }
        public string client { get; set; }
        public List<string> roles { get; set; }
    }

    public class AdminCli
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool composite { get; set; }
        public bool clientRole { get; set; }
        public string containerId { get; set; }
        public Attributes attributes { get; set; }
    }

    public class ArieneTestOpenid
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool composite { get; set; }
        public bool clientRole { get; set; }
        public string containerId { get; set; }
        public Attributes attributes { get; set; }
        public string client { get; set; }
        public List<string> roles { get; set; }
    }

    public class ArieneTestRestricted
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool composite { get; set; }
        public bool clientRole { get; set; }
        public string containerId { get; set; }
        public Attributes attributes { get; set; }
    }

    public class Attributes
    {
        [JsonProperty("post.logout.redirect.uris")]
        public string postlogoutredirecturis { get; set; }

        [JsonProperty("pkce.code.challenge.method")]
        public string pkcecodechallengemethod { get; set; }

        [JsonProperty("id.token.as.detached.signature")]
        public string idtokenasdetachedsignature { get; set; }

        [JsonProperty("saml.assertion.signature")]
        public string samlassertionsignature { get; set; }

        [JsonProperty("client.secret.creation.time")]
        public string clientsecretcreationtime { get; set; }

        [JsonProperty("saml.force.post.binding")]
        public string samlforcepostbinding { get; set; }

        [JsonProperty("saml.multivalued.roles")]
        public string samlmultivaluedroles { get; set; }

        [JsonProperty("saml.encrypt")]
        public string samlencrypt { get; set; }

        [JsonProperty("oauth2.device.authorization.grant.enabled")]
        public string oauth2deviceauthorizationgrantenabled { get; set; }

        [JsonProperty("backchannel.logout.revoke.offline.tokens")]
        public string backchannellogoutrevokeofflinetokens { get; set; }

        [JsonProperty("saml.server.signature")]
        public string samlserversignature { get; set; }

        [JsonProperty("saml.server.signature.keyinfo.ext")]
        public string samlserversignaturekeyinfoext { get; set; }

        [JsonProperty("use.refresh.tokens")]
        public string userefreshtokens { get; set; }

        [JsonProperty("exclude.session.state.from.auth.response")]
        public string excludesessionstatefromauthresponse { get; set; }

        [JsonProperty("oidc.ciba.grant.enabled")]
        public string oidccibagrantenabled { get; set; }

        [JsonProperty("saml.artifact.binding")]
        public string samlartifactbinding { get; set; }

        [JsonProperty("backchannel.logout.session.required")]
        public string backchannellogoutsessionrequired { get; set; }

        [JsonProperty("client_credentials.use_refresh_token")]
        public string client_credentialsuse_refresh_token { get; set; }
        public string saml_force_name_id_format { get; set; }

        [JsonProperty("require.pushed.authorization.requests")]
        public string requirepushedauthorizationrequests { get; set; }

        [JsonProperty("saml.client.signature")]
        public string samlclientsignature { get; set; }

        [JsonProperty("tls.client.certificate.bound.access.tokens")]
        public string tlsclientcertificateboundaccesstokens { get; set; }

        [JsonProperty("saml.authnstatement")]
        public string samlauthnstatement { get; set; }

        [JsonProperty("display.on.consent.screen")]
        public string displayonconsentscreen { get; set; }

        [JsonProperty("saml.onetimeuse.condition")]
        public string samlonetimeusecondition { get; set; }

        [JsonProperty("token.endpoint.auth.signing.alg")]
        public string tokenendpointauthsigningalg { get; set; }
        public string login_theme { get; set; }

        [JsonProperty("tls-client-certificate-bound-access-tokens")]
        public string tlsclientcertificateboundaccesstokens2 { get; set; }

        [JsonProperty("acr.loa.map")]
        public string acrloamap { get; set; }

        [JsonProperty("token.response.type.bearer.lower-case")]
        public string tokenresponsetypebearerlowercase { get; set; }

        [JsonProperty("consent.screen.text")]
        public string consentscreentext { get; set; }

        [JsonProperty("include.in.token.scope")]
        public string includeintokenscope { get; set; }
        public string cibaBackchannelTokenDeliveryMode { get; set; }
        public string cibaAuthRequestedUserHint { get; set; }
        public string clientOfflineSessionMaxLifespan { get; set; }
        public string oauth2DevicePollingInterval { get; set; }
        public string clientSessionIdleTimeout { get; set; }

        [JsonProperty("actionTokenGeneratedByUserLifespan-execute-actions")]
        public string actionTokenGeneratedByUserLifespanexecuteactions { get; set; }
        public string userProfileEnabled { get; set; }

        [JsonProperty("actionTokenGeneratedByUserLifespan-verify-email")]
        public string actionTokenGeneratedByUserLifespanverifyemail { get; set; }
        public string clientOfflineSessionIdleTimeout { get; set; }

        [JsonProperty("actionTokenGeneratedByUserLifespan-reset-credentials")]
        public string actionTokenGeneratedByUserLifespanresetcredentials { get; set; }
        public string cibaInterval { get; set; }
        public string realmReusableOtpCode { get; set; }
        public string cibaExpiresIn { get; set; }
        public string oauth2DeviceCodeLifespan { get; set; }

        [JsonProperty("actionTokenGeneratedByUserLifespan-idp-verify-account-via-email")]
        public string actionTokenGeneratedByUserLifespanidpverifyaccountviaemail { get; set; }
        public string parRequestUriLifespan { get; set; }
        public string clientSessionMaxLifespan { get; set; }
        public string frontendUrl { get; set; }
        public string shortVerificationUri { get; set; }
        public List<string> group_info { get; set; }
        public List<string> organization_code { get; set; }
        public List<string> organization_id { get; set; }
        public List<string> site_id { get; set; }
        public List<string> site_code { get; set; }
        public List<string> id { get; set; }
    }

    public class AuthenticationExecution
    {
        public string authenticator { get; set; }
        public bool authenticatorFlow { get; set; }
        public string requirement { get; set; }
        public int priority { get; set; }
        public bool autheticatorFlow { get; set; }
        public bool userSetupAllowed { get; set; }
        public string authenticatorConfig { get; set; }
        public string flowAlias { get; set; }
    }

    public class AuthenticationFlow
    {
        public string id { get; set; }
        public string alias { get; set; }
        public string description { get; set; }
        public string providerId { get; set; }
        public bool topLevel { get; set; }
        public bool builtIn { get; set; }
        public List<AuthenticationExecution> authenticationExecutions { get; set; }
    }

    public class AuthenticationFlowBindingOverrides
    {
    }

    public class AuthenticatorConfig
    {
        public string id { get; set; }
        public string alias { get; set; }
        public Config config { get; set; }
    }

    public class AuthorizationSettings
    {
        public bool allowRemoteResourceManagement { get; set; }
        public string policyEnforcementMode { get; set; }
        public List<Resource> resources { get; set; }
        public List<Policy> policies { get; set; }
        public List<Scope> scopes { get; set; }
        public string decisionStrategy { get; set; }
    }

    public class Broker
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool composite { get; set; }
        public bool clientRole { get; set; }
        public string containerId { get; set; }
        public Attributes attributes { get; set; }
    }

    public class BrowserSecurityHeaders
    {
        public string contentSecurityPolicyReportOnly { get; set; }
        public string xContentTypeOptions { get; set; }
        public string xRobotsTag { get; set; }
        public string xFrameOptions { get; set; }
        public string contentSecurityPolicy { get; set; }
        public string xXSSProtection { get; set; }
        public string strictTransportSecurity { get; set; }
    }

    public class Client
    {
        public List<string> account { get; set; }

        [JsonProperty("ariene-test-openid")]
        public List<ArieneTestOpenid> arienetestopenid { get; set; }

        [JsonProperty("realm-management")]
        public List<RealmManagement> realmmanagement { get; set; }

        [JsonProperty("security-admin-console")]
        public List<object> securityadminconsole { get; set; }

        [JsonProperty("admin-cli")]
        public List<AdminCli> admincli { get; set; }

        [JsonProperty("account-console")]
        public List<object> accountconsole { get; set; }
        public List<Broker> broker { get; set; }

        [JsonProperty("ariene-test-restricted")]
        public List<ArieneTestRestricted> arienetestrestricted { get; set; }
    }

    public class Client5
    {
        public string id { get; set; }
        public string clientId { get; set; }
        public string name { get; set; }
        public string rootUrl { get; set; }
        public string baseUrl { get; set; }
        public bool surrogateAuthRequired { get; set; }
        public bool enabled { get; set; }
        public bool alwaysDisplayInConsole { get; set; }
        public string clientAuthenticatorType { get; set; }
        public List<string> redirectUris { get; set; }
        public List<string> webOrigins { get; set; }
        public int notBefore { get; set; }
        public bool bearerOnly { get; set; }
        public bool consentRequired { get; set; }
        public bool standardFlowEnabled { get; set; }
        public bool implicitFlowEnabled { get; set; }
        public bool directAccessGrantsEnabled { get; set; }
        public bool serviceAccountsEnabled { get; set; }
        public bool publicClient { get; set; }
        public bool frontchannelLogout { get; set; }
        public string protocol { get; set; }
        public Attributes attributes { get; set; }
        public AuthenticationFlowBindingOverrides authenticationFlowBindingOverrides { get; set; }
        public bool fullScopeAllowed { get; set; }
        public int nodeReRegistrationTimeout { get; set; }
        public List<string> defaultClientScopes { get; set; }
        public List<string> optionalClientScopes { get; set; }
        public List<ProtocolMapper> protocolMappers { get; set; }
        public string description { get; set; }
        public string adminUrl { get; set; }
        public string secret { get; set; }
        public bool? authorizationServicesEnabled { get; set; }
        public AuthorizationSettings authorizationSettings { get; set; }
    }

    public class ClientPolicies
    {
        public List<object> policies { get; set; }
    }

    public class ClientProfiles
    {
        public List<object> profiles { get; set; }
    }

    public class ClientRoles
    {
        [JsonProperty("ariene-test-restricted")]
        public List<string> arienetestrestricted { get; set; }

        [JsonProperty("admin-cli")]
        public List<string> admincli { get; set; }

        [JsonProperty("realm-management")]
        public List<string> realmmanagement { get; set; }
    }

    public class ClientScope
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string protocol { get; set; }
        public Attributes attributes { get; set; }
        public List<ProtocolMapper> protocolMappers { get; set; }
    }

    public class ClientScopeMappings
    {
        [JsonProperty("realm-management")]
        public List<RealmManagement> realmmanagement { get; set; }

        [JsonProperty("ariene-test-openid")]
        public List<ArieneTestOpenid> arienetestopenid { get; set; }
        public List<Account> account { get; set; }
    }

    public class KcComponents
    {
        [JsonProperty("org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy")]
        public List<OrgKeycloakServicesClientregistrationPolicyClientRegistrationPolicy> orgkeycloakservicesclientregistrationpolicyClientRegistrationPolicy { get; set; }

        [JsonProperty("org.keycloak.userprofile.UserProfileProvider")]
        public List<OrgKeycloakUserprofileUserProfileProvider> orgkeycloakuserprofileUserProfileProvider { get; set; }

        [JsonProperty("org.keycloak.storage.UserStorageProvider")]
        public List<OrgKeycloakStorageUserStorageProvider> orgkeycloakstorageUserStorageProvider { get; set; }

        [JsonProperty("org.keycloak.keys.KeyProvider")]
        public List<OrgKeycloakKeysKeyProvider> orgkeycloakkeysKeyProvider { get; set; }
    }

    public class Composites
    {
        public List<string> realm { get; set; }
        public Client client { get; set; }
    }

    public class Config
    {
        [JsonProperty("user.session.note")]
        public string usersessionnote { get; set; }

        [JsonProperty("id.token.claim")]
        public string idtokenclaim { get; set; }

        [JsonProperty("access.token.claim")]
        public string accesstokenclaim { get; set; }

        [JsonProperty("claim.name")]
        public string claimname { get; set; }

        [JsonProperty("jsonType.label")]
        public string jsonTypelabel { get; set; }

        [JsonProperty("userinfo.token.claim")]
        public string userinfotokenclaim { get; set; }

        [JsonProperty("user.attribute")]
        public string userattribute { get; set; }

        [JsonProperty("claim.value")]
        public string claimvalue { get; set; }

        [JsonProperty("access.tokenResponse.claim")]
        public string accesstokenResponseclaim { get; set; }
        public string multivalued { get; set; }

        [JsonProperty("included.client.audience")]
        public string includedclientaudience { get; set; }

        [JsonProperty("aggregate.attrs")]
        public string aggregateattrs { get; set; }

        [JsonProperty("full.path")]
        public string fullpath { get; set; }

        [JsonProperty("usermodel.clientRoleMapping.clientId")]
        public string usermodelclientRoleMappingclientId { get; set; }
        public string code { get; set; }
        public string defaultResourceType { get; set; }
        public string applyPolicies { get; set; }
        public string roles { get; set; }
        public string clients { get; set; }
        public string resources { get; set; }
        public string scopes { get; set; }

        [JsonProperty("user.attribute.formatted")]
        public string userattributeformatted { get; set; }

        [JsonProperty("user.attribute.country")]
        public string userattributecountry { get; set; }

        [JsonProperty("user.attribute.postal_code")]
        public string userattributepostal_code { get; set; }

        [JsonProperty("user.attribute.street")]
        public string userattributestreet { get; set; }

        [JsonProperty("user.attribute.region")]
        public string userattributeregion { get; set; }

        [JsonProperty("user.attribute.locality")]
        public string userattributelocality { get; set; }

        [JsonProperty("allow-default-scopes")]
        public List<string> allowdefaultscopes { get; set; }

        [JsonProperty("max-clients")]
        public List<string> maxclients { get; set; }

        [JsonProperty("host-sending-registration-request-must-match")]
        public List<string> hostsendingregistrationrequestmustmatch { get; set; }

        [JsonProperty("client-uris-must-match")]
        public List<string> clienturismustmatch { get; set; }

        [JsonProperty("allowed-protocol-mapper-types")]
        public List<string> allowedprotocolmappertypes { get; set; }

        [JsonProperty("ldap.attribute")]
        public List<string> ldapattribute { get; set; }

        [JsonProperty("is.mandatory.in.ldap")]
        public List<string> ismandatoryinldap { get; set; }

        [JsonProperty("read.only")]
        public List<string> @readonly { get; set; }

        [JsonProperty("always.read.value.from.ldap")]
        public List<string> alwaysreadvaluefromldap { get; set; }

        [JsonProperty("user.model.attribute")]
        public List<string> usermodelattribute { get; set; }

        [JsonProperty("membership.attribute.type")]
        public List<string> membershipattributetype { get; set; }

        [JsonProperty("group.name.ldap.attribute")]
        public List<string> groupnameldapattribute { get; set; }

        [JsonProperty("preserve.group.inheritance")]
        public List<string> preservegroupinheritance { get; set; }

        [JsonProperty("membership.user.ldap.attribute")]
        public List<string> membershipuserldapattribute { get; set; }

        [JsonProperty("groups.dn")]
        public List<string> groupsdn { get; set; }
        public List<string> mode { get; set; }

        [JsonProperty("user.roles.retrieve.strategy")]
        public List<string> userrolesretrievestrategy { get; set; }

        [JsonProperty("membership.ldap.attribute")]
        public List<string> membershipldapattribute { get; set; }

        [JsonProperty("ignore.missing.groups")]
        public List<string> ignoremissinggroups { get; set; }

        [JsonProperty("group.object.classes")]
        public List<string> groupobjectclasses { get; set; }

        [JsonProperty("memberof.ldap.attribute")]
        public List<string> memberofldapattribute { get; set; }

        [JsonProperty("groups.path")]
        public List<string> groupspath { get; set; }

        [JsonProperty("drop.non.existing.groups.during.sync")]
        public List<string> dropnonexistinggroupsduringsync { get; set; }

        [JsonProperty("write.only")]
        public List<string> writeonly { get; set; }

        [JsonProperty("ldap.full.name.attribute")]
        public List<string> ldapfullnameattribute { get; set; }
        public List<string> fullSyncPeriod { get; set; }
        public List<string> pagination { get; set; }
        public List<string> startTls { get; set; }
        public List<string> usersDn { get; set; }
        public List<string> connectionPooling { get; set; }
        public List<string> cachePolicy { get; set; }
        public List<string> useKerberosForPasswordAuthentication { get; set; }
        public List<string> importEnabled { get; set; }
        public List<string> enabled { get; set; }
        public List<string> changedSyncPeriod { get; set; }
        public List<string> usernameLDAPAttribute { get; set; }
        public List<string> bindDn { get; set; }
        public List<string> bindCredential { get; set; }
        public List<string> lastSync { get; set; }
        public List<string> vendor { get; set; }
        public List<string> uuidLDAPAttribute { get; set; }
        public List<string> allowKerberosAuthentication { get; set; }
        public List<string> connectionUrl { get; set; }
        public List<string> syncRegistrations { get; set; }
        public List<string> authType { get; set; }
        public List<string> useTruststoreSpi { get; set; }
        public List<string> usePasswordModifyExtendedOp { get; set; }
        public List<string> trustEmail { get; set; }
        public List<string> userObjectClasses { get; set; }
        public List<string> rdnLDAPAttribute { get; set; }
        public List<string> editMode { get; set; }
        public List<string> validatePasswordPolicy { get; set; }
        public List<string> priority { get; set; }
        public List<string> algorithm { get; set; }

        [JsonProperty("require.password.update.after.registration")]
        public string requirepasswordupdateafterregistration { get; set; }

        [JsonProperty("update.profile.on.first.login")]
        public string updateprofileonfirstlogin { get; set; }
    }

    public class DefaultRole
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool composite { get; set; }
        public bool clientRole { get; set; }
        public string containerId { get; set; }
    }

    public class Group
    {
        public string id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public Attributes attributes { get; set; }
        public List<string> realmRoles { get; set; }
        public ClientRoles clientRoles { get; set; }
        public List<SubGroup> subGroups { get; set; }
    }

    public class OrgKeycloakKeysKeyProvider
    {
        public string id { get; set; }
        public string name { get; set; }
        public string providerId { get; set; }
        public SubComponents subComponents { get; set; }
        public Config config { get; set; }
    }

    public class OrgKeycloakServicesClientregistrationPolicyClientRegistrationPolicy
    {
        public string id { get; set; }
        public string name { get; set; }
        public string providerId { get; set; }
        public string subType { get; set; }
        public SubComponents subComponents { get; set; }
        public Config config { get; set; }
    }

    public class OrgKeycloakStorageLdapMappersLDAPStorageMapper
    {
        public string id { get; set; }
        public string name { get; set; }
        public string providerId { get; set; }
        public SubComponents subComponents { get; set; }
        public Config config { get; set; }
    }

    public class OrgKeycloakStorageUserStorageProvider
    {
        public string id { get; set; }
        public string name { get; set; }
        public string providerId { get; set; }
        public SubComponents subComponents { get; set; }
        public Config config { get; set; }
    }

    public class OrgKeycloakUserprofileUserProfileProvider
    {
        public string id { get; set; }
        public string providerId { get; set; }
        public SubComponents subComponents { get; set; }
        public Config config { get; set; }
    }

    public class Policy
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string logic { get; set; }
        public string decisionStrategy { get; set; }
        public Config config { get; set; }
    }

    public class ProtocolMapper
    {
        public string id { get; set; }
        public string name { get; set; }
        public string protocol { get; set; }
        public string protocolMapper { get; set; }
        public bool consentRequired { get; set; }
        public Config config { get; set; }
    }

    public class Realm
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool composite { get; set; }
        public bool clientRole { get; set; }
        public string containerId { get; set; }
        public Attributes attributes { get; set; }
        public Composites composites { get; set; }
    }

    public class RealmManagement
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool composite { get; set; }
        public bool clientRole { get; set; }
        public string containerId { get; set; }
        public Attributes attributes { get; set; }
        public Composites composites { get; set; }
        public string client { get; set; }
        public List<string> roles { get; set; }
    }

    public class RequiredAction
    {
        public string alias { get; set; }
        public string name { get; set; }
        public string providerId { get; set; }
        public bool enabled { get; set; }
        public bool defaultAction { get; set; }
        public int priority { get; set; }
        public Config config { get; set; }
    }

    public class Resource
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool ownerManagedAccess { get; set; }
        public Attributes attributes { get; set; }
        public string _id { get; set; }
        public List<string> uris { get; set; }
        public string displayName { get; set; }
        public List<Scope> scopes { get; set; }
        public string icon_uri { get; set; }
    }

    public class Roles
    {
        public List<Realm> realm { get; set; }
        public Client client { get; set; }
    }

    public class Root
    {
        public string id { get; set; }
        public string realm { get; set; }
        public int notBefore { get; set; }
        public string defaultSignatureAlgorithm { get; set; }
        public bool revokeRefreshToken { get; set; }
        public int refreshTokenMaxReuse { get; set; }
        public int accessTokenLifespan { get; set; }
        public int accessTokenLifespanForImplicitFlow { get; set; }
        public int ssoSessionIdleTimeout { get; set; }
        public int ssoSessionMaxLifespan { get; set; }
        public int ssoSessionIdleTimeoutRememberMe { get; set; }
        public int ssoSessionMaxLifespanRememberMe { get; set; }
        public int offlineSessionIdleTimeout { get; set; }
        public bool offlineSessionMaxLifespanEnabled { get; set; }
        public int offlineSessionMaxLifespan { get; set; }
        public int clientSessionIdleTimeout { get; set; }
        public int clientSessionMaxLifespan { get; set; }
        public int clientOfflineSessionIdleTimeout { get; set; }
        public int clientOfflineSessionMaxLifespan { get; set; }
        public int accessCodeLifespan { get; set; }
        public int accessCodeLifespanUserAction { get; set; }
        public int accessCodeLifespanLogin { get; set; }
        public int actionTokenGeneratedByAdminLifespan { get; set; }
        public int actionTokenGeneratedByUserLifespan { get; set; }
        public int oauth2DeviceCodeLifespan { get; set; }
        public int oauth2DevicePollingInterval { get; set; }
        public bool enabled { get; set; }
        public string sslRequired { get; set; }
        public bool registrationAllowed { get; set; }
        public bool registrationEmailAsUsername { get; set; }
        public bool rememberMe { get; set; }
        public bool verifyEmail { get; set; }
        public bool loginWithEmailAllowed { get; set; }
        public bool duplicateEmailsAllowed { get; set; }
        public bool resetPasswordAllowed { get; set; }
        public bool editUsernameAllowed { get; set; }
        public bool bruteForceProtected { get; set; }
        public bool permanentLockout { get; set; }
        public int maxFailureWaitSeconds { get; set; }
        public int minimumQuickLoginWaitSeconds { get; set; }
        public int waitIncrementSeconds { get; set; }
        public int quickLoginCheckMilliSeconds { get; set; }
        public int maxDeltaTimeSeconds { get; set; }
        public int failureFactor { get; set; }
        public Roles roles { get; set; }
        public List<Group> groups { get; set; }
        public DefaultRole defaultRole { get; set; }
        public List<string> requiredCredentials { get; set; }
        public string passwordPolicy { get; set; }
        public string otpPolicyType { get; set; }
        public string otpPolicyAlgorithm { get; set; }
        public int otpPolicyInitialCounter { get; set; }
        public int otpPolicyDigits { get; set; }
        public int otpPolicyLookAheadWindow { get; set; }
        public int otpPolicyPeriod { get; set; }
        public bool otpPolicyCodeReusable { get; set; }
        public List<string> otpSupportedApplications { get; set; }
        public string webAuthnPolicyRpEntityName { get; set; }
        public List<string> webAuthnPolicySignatureAlgorithms { get; set; }
        public string webAuthnPolicyRpId { get; set; }
        public string webAuthnPolicyAttestationConveyancePreference { get; set; }
        public string webAuthnPolicyAuthenticatorAttachment { get; set; }
        public string webAuthnPolicyRequireResidentKey { get; set; }
        public string webAuthnPolicyUserVerificationRequirement { get; set; }
        public int webAuthnPolicyCreateTimeout { get; set; }
        public bool webAuthnPolicyAvoidSameAuthenticatorRegister { get; set; }
        public List<object> webAuthnPolicyAcceptableAaguids { get; set; }
        public string webAuthnPolicyPasswordlessRpEntityName { get; set; }
        public List<string> webAuthnPolicyPasswordlessSignatureAlgorithms { get; set; }
        public string webAuthnPolicyPasswordlessRpId { get; set; }
        public string webAuthnPolicyPasswordlessAttestationConveyancePreference { get; set; }
        public string webAuthnPolicyPasswordlessAuthenticatorAttachment { get; set; }
        public string webAuthnPolicyPasswordlessRequireResidentKey { get; set; }
        public string webAuthnPolicyPasswordlessUserVerificationRequirement { get; set; }
        public int webAuthnPolicyPasswordlessCreateTimeout { get; set; }
        public bool webAuthnPolicyPasswordlessAvoidSameAuthenticatorRegister { get; set; }
        public List<object> webAuthnPolicyPasswordlessAcceptableAaguids { get; set; }
        public List<User> users { get; set; }
        public List<ScopeMapping> scopeMappings { get; set; }
        public ClientScopeMappings clientScopeMappings { get; set; }
        public List<Client> clients { get; set; }
        public List<ClientScope> clientScopes { get; set; }
        public List<string> defaultDefaultClientScopes { get; set; }
        public List<string> defaultOptionalClientScopes { get; set; }
        public BrowserSecurityHeaders browserSecurityHeaders { get; set; }
        public SmtpServer smtpServer { get; set; }
        public string loginTheme { get; set; }
        public string accountTheme { get; set; }
        public string adminTheme { get; set; }
        public string emailTheme { get; set; }
        public bool eventsEnabled { get; set; }
        public int eventsExpiration { get; set; }
        public List<string> eventsListeners { get; set; }
        public List<string> enabledEventTypes { get; set; }
        public bool adminEventsEnabled { get; set; }
        public bool adminEventsDetailsEnabled { get; set; }
        public List<object> identityProviders { get; set; }
        public List<object> identityProviderMappers { get; set; }
        public KcComponents components { get; set; }
        public bool internationalizationEnabled { get; set; }
        public List<string> supportedLocales { get; set; }
        public string defaultLocale { get; set; }
        public List<AuthenticationFlow> authenticationFlows { get; set; }
        public List<AuthenticatorConfig> authenticatorConfig { get; set; }
        public List<RequiredAction> requiredActions { get; set; }
        public string browserFlow { get; set; }
        public string registrationFlow { get; set; }
        public string directGrantFlow { get; set; }
        public string resetCredentialsFlow { get; set; }
        public string clientAuthenticationFlow { get; set; }
        public string dockerAuthenticationFlow { get; set; }
        public Attributes attributes { get; set; }
        public string keycloakVersion { get; set; }
        public bool userManagedAccessAllowed { get; set; }
        public ClientProfiles clientProfiles { get; set; }
        public ClientPolicies clientPolicies { get; set; }
    }

    public class Scope
    {
        public string name { get; set; }
        public string id { get; set; }
        public string displayName { get; set; }
        public string iconUri { get; set; }
    }

    public class ScopeMapping
    {
        public string client { get; set; }
        public List<string> roles { get; set; }
        public string clientScope { get; set; }
    }

    public class SmtpServer
    {
        public string replyToDisplayName { get; set; }
        public string starttls { get; set; }
        public string auth { get; set; }
        public string envelopeFrom { get; set; }
        public string ssl { get; set; }
        public string password { get; set; }
        public string port { get; set; }
        public string host { get; set; }
        public string replyTo { get; set; }
        public string from { get; set; }
        public string fromDisplayName { get; set; }
        public string user { get; set; }
    }

    public class SubComponents
    {
        [JsonProperty("org.keycloak.storage.ldap.mappers.LDAPStorageMapper")]
        public List<OrgKeycloakStorageLdapMappersLDAPStorageMapper> orgkeycloakstorageldapmappersLDAPStorageMapper { get; set; }
    }

    public class SubGroup
    {
        public string id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public Attributes attributes { get; set; }
        public List<object> realmRoles { get; set; }
        public ClientRoles clientRoles { get; set; }
        public List<object> subGroups { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public object createdTimestamp { get; set; }
        public string username { get; set; }
        public bool enabled { get; set; }
        public bool totp { get; set; }
        public bool emailVerified { get; set; }
        public string serviceAccountClientId { get; set; }
        public List<object> disableableCredentialTypes { get; set; }
        public List<object> requiredActions { get; set; }
        public List<string> realmRoles { get; set; }
        public ClientRoles clientRoles { get; set; }
        public int notBefore { get; set; }
        public List<object> groups { get; set; }
    }


}
