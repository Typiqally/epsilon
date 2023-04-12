namespace NetCore.Lti;

public static class LtiClaimType
{
    public const string MessageType = "https://purl.imsglobal.org/spec/lti/claim/message_type";
    public const string Version = "https://purl.imsglobal.org/spec/lti/claim/version";
    public const string DeploymentId = "https://purl.imsglobal.org/spec/lti/claim/deployment_id";
    public const string TargetLinkUri = "https://purl.imsglobal.org/spec/lti/claim/target_link_uri";
    public const string ResourceLink = "https://purl.imsglobal.org/spec/lti/claim/resource_link";
    public const string Roles = "https://purl.imsglobal.org/spec/lti/claim/roles";
    public const string Context = "https://purl.imsglobal.org/spec/lti/claim/context";
    public const string ToolPlatform = "https://purl.imsglobal.org/spec/lti/claim/tool_platform";
    public const string RoleScopeMentor = "https://purl.imsglobal.org/spec/lti/claim/role_scope_mentor";
    public const string LaunchPresentation = "https://purl.imsglobal.org/spec/lti/claim/launch_presentation";
    public const string LearningInformationServices = "https://purl.imsglobal.org/spec/lti/claim/lis";
    public const string Custom = "https://purl.imsglobal.org/spec/lti/claim/custom";
    
    // These are used for migration between LTI 1.1* to LTI 1.3
    public const string Lti1P1 = "https://purl.imsglobal.org/spec/lti/claim/lti1p1";
    public const string Lti1P1LegacyUserId = "https://purl.imsglobal.org/spec/lti/claim/lti11_legacy_user_id";
}